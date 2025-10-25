using UnityEngine;
using System.Collections;

public class CupShakeManager : MonoBehaviour
{
    [Header("🥤 Danh sách các cốc")]
    public CupObject[] cups;

    [Header("📱 Cấu hình cảm biến")]
    public float shakeThreshold = 2.5f;
    public float shakeCooldown = 1.0f;

    [Header("🏆 UI Chiến thắng")]
    public GameObject winUI;

    private bool hasShaken = false;
    private float lastShakeTime;

    private void Update()
    {
        // 💻 Test trong Editor bằng phím T
        if (!hasShaken && Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("💻 Test lắc bằng phím T");
            StartCoroutine(HandleShake());
        }

#if UNITY_ANDROID || UNITY_IOS
        // 📱 Kiểm tra cảm biến trên điện thoại thật
        Vector3 accel = Input.acceleration;
        float shake = accel.sqrMagnitude;

        if (!hasShaken && shake > shakeThreshold && Time.time - lastShakeTime > shakeCooldown)
        {
            lastShakeTime = Time.time;
            StartCoroutine(HandleShake());
        }
#endif
    }

    private IEnumerator HandleShake()
    {
        hasShaken = true;
        Debug.Log("📱 Phát hiện lắc điện thoại hoặc nhấn phím T!");

        // ⏱️ Delay giữa các cốc = 0.1s, tổng thời gian đổ = nhanh hơn
        foreach (CupObject cup in cups)
        {
            cup.OnShake();
            yield return new WaitForSeconds(0.1f);
        }

        // ⏱️ Thời gian delay sau khi tất cả đổ xong = 0.5s
        yield return new WaitForSeconds(0.5f);

        // ✅ Hiện UI thắng
        if (GameManager.Instance != null)
            GameManager.Instance.EndGame(true);
        if (winUI != null)
            winUI.SetActive(true);
    }
}
