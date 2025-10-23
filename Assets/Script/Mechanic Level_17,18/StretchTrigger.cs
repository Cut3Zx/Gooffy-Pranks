using UnityEngine;

public class StretchTrigger : MonoBehaviour
{
    [Header("Cài đặt theo dõi")]
    public FollowAndWin bearFollower;   // 🐻 Con gấu có script FollowAndWin
    public float triggerScaleY = 1.5f;  // Khi dây dài hơn giá trị này sẽ kích hoạt
    public bool triggerOnce = true;     // Chỉ kích hoạt 1 lần
    private bool hasTriggered = false;

    private RectTransform rect;

    private void Start()
    {
        rect = GetComponent<RectTransform>();

        if (bearFollower != null)
        {
            bearFollower.enabled = false; // ❌ Ban đầu chưa cho gấu chạy
        }
    }

    private void Update()
    {
        if (hasTriggered || rect == null) return;

        if (rect.localScale.y >= triggerScaleY)
        {
            hasTriggered = true;
            Debug.Log("🌿 Dây leo đủ dài! Cho gấu bắt đầu chạy tới!");

            if (bearFollower != null)
            {
                bearFollower.enabled = true; // ✅ Bật script FollowAndWin để gấu bắt đầu chạy
            }
        }
    }
}
