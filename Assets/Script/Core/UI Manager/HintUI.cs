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

    private void Start()
    {
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
        hintText.text = $"{hint}";
    }

    private void OnWatchAdClicked()
    {
        Debug.Log("🎬 Người chơi bấm xem quảng cáo nhận hint...");

        if (AdsManager.Instance != null)
        {
            AdsManager.Instance.ShowRewardedAd();
        }
        else
        {
            Debug.LogError("❌ AdsManager chưa được khởi tạo!");
        }
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
}
