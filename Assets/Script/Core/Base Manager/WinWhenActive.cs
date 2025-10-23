using UnityEngine;
using System.Collections;

public class WinWhenActive : MonoBehaviour
{
    [Header("⏳ Thời gian delay trước khi thắng (giây)")]
    public float delayTime = 1.5f;

    private bool hasWon = false;
    private bool coroutineStarted = false;

    private void OnEnable()
    {
        // Khi object được bật trở lại, reset flag delay
        coroutineStarted = false;
    }

    private void Update()
    {
        if (!hasWon && gameObject.activeInHierarchy && !coroutineStarted)
        {
            coroutineStarted = true;
            StartCoroutine(DelayWinCoroutine());
        }
    }

    private IEnumerator DelayWinCoroutine()
    {
        Debug.Log($"⏳ {gameObject.name} đã active — chờ {delayTime}s để thắng...");
        yield return new WaitForSeconds(delayTime);

        // Đảm bảo object vẫn còn tồn tại và active
        if (this != null && gameObject.activeInHierarchy)
        {
            hasWon = true;
            Debug.Log($"🏆 {gameObject.name} => Thắng sau {delayTime}s");

            if (GameManager.Instance != null)
                GameManager.Instance.EndGame(true);
            else
                Debug.LogWarning("⚠️ Không tìm thấy GameManager!");
        }
    }
}
