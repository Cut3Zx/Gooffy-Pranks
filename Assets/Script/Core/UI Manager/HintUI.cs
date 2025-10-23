using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class HintUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI hintText;
    public Button rewardButton;
    public Button useHintButton;
    public GameObject hintPanel;
    public TextMeshProUGUI hintMessage;

    [Header("Hint Content")]
    [TextArea(2, 3)]
    public string currentHint = "💭 Gợi ý mặc định";

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
            hintText.text = $"{hint}";
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
            hint-=0;
            PlayerPrefs.SetInt("hintCount", hint);
            PlayerPrefs.Save();
            UpdateHintUI();

            Debug.Log($"💡 Dùng 1 hint! Còn lại: {hint}");

            if (hintPanel != null && hintMessage != null)
            {
                hintPanel.SetActive(true);
                hintMessage.text = currentHint;
                StartCoroutine(HideHintPanelAfterDelay(3f));
            }
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

    // ===============================
    // 🧩 HÀM THÊM / GIẢM / RESET HINT TRỰC TIẾP = CODE
    // ===============================

    public void AddHint(int amount = 1)
    {
        int hint = PlayerPrefs.GetInt("hintCount", 0);
        hint = Mathf.Min(hint + amount, maxHint);
        PlayerPrefs.SetInt("hintCount", hint);
        PlayerPrefs.Save();
        UpdateHintUI();
        Debug.Log($"➕ Đã cộng {amount} hint! Tổng: {hint}");
    }

    public void RemoveHint(int amount = 1)
    {
        int hint = PlayerPrefs.GetInt("hintCount", 0);
        hint = Mathf.Max(hint - amount, 0);
        PlayerPrefs.SetInt("hintCount", hint);
        PlayerPrefs.Save();
        UpdateHintUI();
        Debug.Log($"➖ Đã trừ {amount} hint! Còn lại: {hint}");
    }

    public void ResetHint()
    {
        PlayerPrefs.SetInt("hintCount", defaultHint);
        PlayerPrefs.Save();
        UpdateHintUI();
        Debug.Log($"♻️ Reset hint về {defaultHint}");
    }
}
