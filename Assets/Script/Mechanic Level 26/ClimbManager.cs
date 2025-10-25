using UnityEngine;
using System.Collections;

public class ClimbManager : MonoBehaviour
{
    public static ClimbManager Instance;

    [Header("⏳ Thời gian delay trước khi thắng (giây)")]
    public float delayTime = 1.5f;

    [Header("Tổng số bậc cần lắp đầy")]
    public int totalSteps = 3;
    private int filledSteps = 0;

    [Header("UI Chiến thắng")]
    public GameObject winUI;

    private bool hasWon = false; // để tránh gọi thắng nhiều lần

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegisterStepFilled()
    {
        filledSteps++;
        Debug.Log($"🧩 Bậc thang đã lắp: {filledSteps}/{totalSteps}");

        if (filledSteps >= totalSteps && !hasWon)
        {
            Debug.Log("🎉 Đủ bậc thang → bắt đầu đếm delay thắng!");
            StartCoroutine(DelayWinCoroutine());
        }
    }

    public void UnregisterStepFilled()
    {
        filledSteps = Mathf.Max(0, filledSteps - 1);
    }

    private IEnumerator DelayWinCoroutine()
    {
        hasWon = true; // tránh chạy trùng
        Debug.Log($"⏳ Chờ {delayTime}s trước khi hiện Win UI...");
        yield return new WaitForSeconds(delayTime);

        TriggerWin();
    }

    private void TriggerWin()
    {
        Debug.Log("🏆 Kích hoạt Win UI!");
        if (winUI != null)
            winUI.SetActive(true);

        if (GameManager.Instance != null)
            GameManager.Instance.EndGame(true);
        else
            Debug.LogWarning("⚠️ Không tìm thấy GameManager!");

        Time.timeScale = 0f;
    }
}
