using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Ump.Api;

public class ConsentManager : MonoBehaviour
{
    public static ConsentManager Instance { get; private set; }

    public bool IsReady { get; private set; }
    public bool IsNpa { get; private set; } // true = non-personalized ads

    private const string KeyPolicyChoice = "policy_choice";
    private ConsentForm consentForm;

    [Header("Links")]
    [SerializeField] private string privacyPolicyUrl = "https://springmuch05.github.io/Idiot-Odyssey/";

    [Header("Optional: Popup Privacy trong Scene (SimplePopup)")]
    [SerializeField] private SimplePopup popupPanel;

    public event Action OnConsentFlowFinished;

    private bool consentChecked = false; // 🔹 tránh gọi lại khi đổi scene

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // ✅ giữ lại khi đổi scene
    }

    void Start()
    {
        // 🔹 Đảm bảo chỉ chạy 1 lần duy nhất trong toàn bộ vòng đời game
        if (consentChecked)
            return;

        consentChecked = true;

        // ✅ Nếu người dùng đã chấp nhận privacy trước đó, bỏ qua luôn
        if (PlayerPrefs.GetInt(KeyPolicyChoice, 0) == 1)
        {
            Debug.Log("🟢 Consent previously accepted — skipping popup.");
            IsNpa = false;
            IsReady = true;
            OnConsentFlowFinished?.Invoke();
            return;
        }

        // 🔹 Khởi tạo SDK sớm (bắt buộc với UMP)
        MobileAds.Initialize(_ => Debug.Log("✅ Mobile Ads initialized."));

        // 🔹 Gọi request cho UMP
        var request = new ConsentRequestParameters();

#if UNITY_EDITOR || DEVELOPMENT_BUILD
        var debug = new ConsentDebugSettings { DebugGeography = DebugGeography.EEA };
        request.ConsentDebugSettings = debug;
#endif

        ConsentInformation.Update(request, OnConsentInfoUpdated);
    }

    private void OnConsentInfoUpdated(FormError error)
    {
        if (error != null)
        {
            Debug.LogWarning("UMP Update error: " + error.Message);
            HandleOutsideEEA();
            return;
        }

        if (ConsentInformation.IsConsentFormAvailable())
        {
            ConsentForm.Load((ConsentForm form, FormError loadErr) =>
            {
                if (loadErr != null)
                {
                    Debug.LogWarning("UMP Load form error: " + loadErr.Message);
                    HandleOutsideEEA();
                    return;
                }

                consentForm = form;
                ShowUMPFormIfRequired();
            });
        }
        else
        {
            HandleOutsideEEA();
        }
    }

    private void ShowUMPFormIfRequired()
    {
        consentForm.Show((FormError showErr) =>
        {
            if (showErr != null)
                Debug.LogWarning("UMP Show form error: " + showErr.Message);

            // 🟩 Người dùng accept → lưu lại trạng thái (vĩnh viễn)
            PlayerPrefs.SetInt(KeyPolicyChoice, 1);
            PlayerPrefs.Save();

            ResolveConsentStatusAndFinish();
        });
    }

    private void HandleOutsideEEA()
    {
        // 🔹 Nếu người chơi đã từng accept → không hiện lại nữa
        if (PlayerPrefs.GetInt(KeyPolicyChoice, 0) == 1)
        {
            Debug.Log("🟢 Player already accepted privacy, skipping popup.");
            IsNpa = false;
            Finish(true);
            return;
        }

        Debug.Log("🌏 Outside EEA → showing in-game privacy popup for first time.");

        if (popupPanel != null)
        {
            popupPanel.Accepted -= OnPopupAccepted;
            popupPanel.Declined -= OnPopupDeclined;
            popupPanel.Accepted += OnPopupAccepted;
            popupPanel.Declined += OnPopupDeclined;

            // ✅ Bật object lên (vì trong scene bạn đã tắt Active)
            popupPanel.ShowIfNeeded();

        }
        else
        {
            IsNpa = true;
            Finish(true);
        }
    }

    private void OnPopupAccepted()
    {
        // ✅ Lưu trạng thái accept
        PlayerPrefs.SetInt(KeyPolicyChoice, 1);
        PlayerPrefs.Save();

        // ✅ Tắt popup để không hiện lại nữa
        if (popupPanel != null)
            popupPanel.gameObject.SetActive(false);

        IsNpa = false;
        Finish(true);
    }


    

    private void OnPopupDeclined()
    {
        IsNpa = true;
        Finish(true);
    }

    private void ResolveConsentStatusAndFinish()
    {
        var status = ConsentInformation.ConsentStatus;
        IsNpa = (status != ConsentStatus.Obtained);
        Finish(true);
    }

    private void Finish(bool ok)
    {
        if (IsReady) return;
        IsReady = ok;
        Debug.Log($"✅ Consent finished. IsNPA: {IsNpa}");
        OnConsentFlowFinished?.Invoke();
    }

    public void OpenPrivacyOptions()
    {
        ConsentForm.LoadAndShowConsentFormIfRequired((FormError err) =>
        {
            ResolveConsentStatusAndFinish();
        });
    }

    public void OpenPrivacyPolicy()
    {
        Application.OpenURL(privacyPolicyUrl);
    }

    public AdRequest BuildAdRequest()
    {
        var request = new AdRequest();
        try
        {
            var extrasProp = typeof(AdRequest).GetProperty("Extras");
            if (extrasProp != null)
            {
                var extras = extrasProp.GetValue(request) as IDictionary<string, string>;
                if (extras == null)
                {
                    extras = new Dictionary<string, string>();
                    extrasProp.SetValue(request, extras);
                }

                if (IsNpa)
                    extras["npa"] = "1";
                else if (extras.ContainsKey("npa"))
                    extras.Remove("npa");
            }
        }
        catch { }
        return request;
    }
}
