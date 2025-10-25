using UnityEngine;
using System.Collections;

public class TiltToFall_WithPivot_Final : MonoBehaviour
{
    [Header("⚙️ Cấu hình nghiêng (chỉ hoạt động trên điện thoại)")]
    [Tooltip("Giá trị gia tốc Y nhỏ hơn ngưỡng này sẽ tính là lật máy (âm = úp)")]
    public float tiltThreshold = -0.6f;

    [Tooltip("Độ thay đổi cần thiết để nhận lắc mạnh")]
    public float shakeSensitivity = 0.3f;

    [Tooltip("Thời gian trễ trước khi bắt đầu kiểm tra (tránh trigger sớm)")]
    public float startDelay = 1f;

    [Header("🎭 Cấu hình ngã")]
    [Tooltip("Nhân vật sẽ ngã")]
    public GameObject prankster;

    [Tooltip("Điểm pivot để xoay ngã (gắn ở chân)")]
    public Transform fallPivot;

    [Tooltip("Tốc độ xoay ngã (độ/giây)")]
    public float fallSpeed = 300f;

    [Header("🏆 UI Chiến thắng")]
    public GameObject winUI;

    private bool hasFallen = false;
    private Vector3 lastAcceleration;
    private bool sensorReady = false;

    private void Start()
    {
        lastAcceleration = Vector3.zero;
        StartCoroutine(EnableSensorAfterDelay());
    }

    private IEnumerator EnableSensorAfterDelay()
    {
        yield return new WaitForSeconds(startDelay);
        sensorReady = true;
        Debug.Log("📱 Cảm biến đã sẵn sàng!");
    }

    private void Update()
    {
        if (hasFallen) return;

        // 💻 Nếu đang chạy trong Editor thì KHÔNG dùng cảm biến
        if (Application.isEditor)
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                Debug.Log("💻 Test ngã bằng phím T (Editor)");
                StartCoroutine(FallAndWin());
            }
            return; // 🚫 Dừng luôn, không check cảm biến nữa
        }

#if UNITY_ANDROID || UNITY_IOS
        // ✅ Chỉ hoạt động trên thiết bị thật
        if (!sensorReady) return;

        Vector3 current = Input.acceleration;
        float delta = (current - lastAcceleration).magnitude;
        lastAcceleration = current;

        // Lật úp hoặc lắc mạnh → ngã
        if ((current.y < tiltThreshold) || (delta > shakeSensitivity))
        {
            Debug.Log($"📱 Phát hiện lật/lắc mạnh (Δ={delta:F2}) → Bắt đầu ngã");
            StartCoroutine(FallAndWin());
        }
#endif
    }

    private IEnumerator FallAndWin()
    {
        hasFallen = true;

        float angle = 0f;
        float targetAngle = 90f;

        // Xoay dần nhân vật quanh pivot
        while (angle < targetAngle)
        {
            float step = Time.deltaTime * fallSpeed;
            angle += step;

            if (prankster != null)
            {
                if (fallPivot != null)
                {
                    prankster.transform.RotateAround(fallPivot.position, Vector3.forward, step);
                }
                else
                {
                    prankster.transform.rotation = Quaternion.Euler(0, 0, angle);
                }
            }

            yield return null;
        }

        Debug.Log("🏆 Prankster ngã rồi → Thắng!");
        if (GameManager.Instance != null)
            GameManager.Instance.EndGame(true);
        if (winUI != null)
            winUI.SetActive(true);
    }
}
