using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class HintUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI hintText;      // Hiển thị số lượng hint
    public Button rewardButton;
    public Button useHintButton;
    public GameObject hintPanel;
    public TextMeshProUGUI hintMessage;   // 🔹 Text hiển thị hint (bạn setup sẵn text ở đây trong Editor)

    [Header("Hint Config")]
    public int defaultHint = 3;
    public int maxHint = 1000;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("hintCount"))
        {
            PlayerPrefs.SetInt("hintCount", defaultHint);
            PlayerPrefs.Save();
        }

        UpdateHintUI();

        if (rewardButton != null)
            rewardButton.onClick.AddListener(OnWatchAdClicked);

        if (useHintButton != null)
            useHintButton.onClick.AddListener(OnUseHintClicked);

        if (hintPanel != null)
            hintPanel.SetActive(false);
    }

    private void Update()
    {
        UpdateHintUI();
    }

    private void UpdateHintUI()
    {
        int hint = PlayerPrefs.GetInt("hintCount", 0);
        if (hintText != null)
            hintText.text = hint.ToString();
    }

    private void OnWatchAdClicked()
    {
        Debug.Log("🎬 Người chơi xem quảng cáo nhận hint...");

        if (AdsManager.Instance != null)
            AdsManager.Instance.ShowRewardedAd();
        else
            Debug.LogWarning("⚠️ AdsManager chưa được khởi tạo!");
    }

    private void OnUseHintClicked()
    {
        int hint = PlayerPrefs.GetInt("hintCount", 0);

        if (hint > 0)
        {
            hint--;
            PlayerPrefs.SetInt("hintCount", hint);
            PlayerPrefs.Save();
            UpdateHintUI();

            if (hintPanel != null)
            {
                hintPanel.SetActive(true);
                // 🔹 Không cần set text ở đây nữa — text đã được gắn sẵn trong hintMessage (và có thể là text dịch)
                StartCoroutine(HideHintPanelAfterDelay(3f));
            }

            Debug.Log($"💡 Dùng 1 hint! Còn lại: {hint}");
        }
        else
        {
            Debug.Log("⚠️ Không còn hint để dùng!");
        }
    }

    private IEnumerator HideHintPanelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (hintPanel != null)
            hintPanel.SetActive(false);
    }

    // Các hàm quản lý hint vẫn giữ nguyên
    public void AddHint(int amount = 1)
    {
        int hint = PlayerPrefs.GetInt("hintCount", 0);
        hint = Mathf.Min(hint + amount, maxHint);
        PlayerPrefs.SetInt("hintCount", hint);
        PlayerPrefs.Save();
        UpdateHintUI();
    }

    public void RemoveHint(int amount = 1)
    {
        int hint = PlayerPrefs.GetInt("hintCount", 0);
        hint = Mathf.Max(hint - amount, 0);
        PlayerPrefs.SetInt("hintCount", hint);
        PlayerPrefs.Save();
        UpdateHintUI();
    }

    public void ResetHint()
    {
        PlayerPrefs.SetInt("hintCount", defaultHint);
        PlayerPrefs.Save();
        UpdateHintUI();
    }
}
