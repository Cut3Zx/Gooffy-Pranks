using UnityEngine;
using System.Collections;

public class ClimbManager : MonoBehaviour
{
    public static ClimbManager Instance;

    [Header("⏳ Thời gian delay trước khi báo thắng (giây)")]
    public float delayTime = 1.5f;

    [Header("Tổng số bậc cần lắp đầy")]
    public int totalSteps = 3;
    private int filledSteps = 0;

    private bool hasWon = false; // tránh gọi nhiều lần

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegisterStepFilled()
    {
        if (hasWon) return; // nếu đã thắng thì bỏ qua

        filledSteps++;
        Debug.Log($"🧩 Bậc thang đã lắp: {filledSteps}/{totalSteps}");

        if (filledSteps >= totalSteps)
        {
            hasWon = true;
            Debug.Log("🎉 Đủ bậc thang → bắt đầu đếm delay thắng!");
            StartCoroutine(DelayWinCoroutine());
        }
    }

    public void UnregisterStepFilled()
    {
        if (hasWon) return; // khi đã thắng rồi không giảm nữa
        filledSteps = Mathf.Max(0, filledSteps - 1);
    }

    private IEnumerator DelayWinCoroutine()
    {
        Debug.Log($"⏳ Chờ {delayTime}s trước khi báo thắng...");
        yield return new WaitForSeconds(delayTime);

        TriggerWin();
    }

    private void TriggerWin()
    {
        Debug.Log("🏆 Báo thắng về GameManager");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.EndGame(true);
        }
        else
        {
            Debug.LogWarning("⚠️ Không tìm thấy GameManager trong scene!");
        }
    }
}
