using UnityEngine;

public class ZoomPondOnly : MonoBehaviour
{
    [Header("🎯 Đối tượng cần zoom (ví dụ: hồ)")]
    public Transform pondTransform;

    [Header("📏 Giới hạn scale")]
    public float minScale = 1f;
    public float maxScale = 3f;

    [Header("🐟 Con cá sẽ xuất hiện khi zoom đủ gần")]
    public GameObject fishObject;

    [Header("📈 Zoom bao nhiêu thì hiện cá")]
    public float revealScaleThreshold = 2.2f;

    [Header("⏩ Tốc độ zoom")]
    public float zoomSpeed = 0.01f;

    private Vector3 originalScale;

    void Start()
    {
        if (pondTransform == null)
            pondTransform = transform;

        originalScale = pondTransform.localScale;

        if (fishObject != null)
            fishObject.SetActive(false);
    }

    void Update()
    {
        if (pondTransform == null) return;

        float scrollDelta = 0f;

        // 💻 Zoom test bằng chuột trên PC
        scrollDelta = Input.GetAxis("Mouse ScrollWheel");

        // 📱 Zoom bằng 2 ngón trên điện thoại
        if (Input.touchCount == 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            Vector2 prevPos0 = t0.position - t0.deltaPosition;
            Vector2 prevPos1 = t1.position - t1.deltaPosition;

            float prevMag = (prevPos0 - prevPos1).magnitude;
            float currentMag = (t0.position - t1.position).magnitude;

            float diff = currentMag - prevMag;
            scrollDelta = diff * zoomSpeed;
        }

        // 🎨 Cập nhật scale hồ
        float newScale = Mathf.Clamp(pondTransform.localScale.x + scrollDelta, minScale, maxScale);
        pondTransform.localScale = new Vector3(newScale, newScale, 1f);

        // 🐟 Kiểm tra hiện cá
        if (fishObject != null)
        {
            if (newScale >= revealScaleThreshold && !fishObject.activeSelf)
            {
                fishObject.SetActive(true);
                Debug.Log("🐟 Hồ được zoom đủ → Cá xuất hiện!");
            }
            else if (newScale < revealScaleThreshold && fishObject.activeSelf)
            {
                fishObject.SetActive(false);
            }
        }
    }
}
