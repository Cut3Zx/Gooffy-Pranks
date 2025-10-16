using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class ChalkDrag : BaseObjectManager
{
    [Header("References")]
    public GameObject compassOutline;   // Hình compa chưa tô màu
    public GameObject compassColored;   // Hình compa đã tô màu
    public GameObject compassReal;      // Hình compa thật

    [Header("Timing")]
    public float revealDelay = 2f;      // Thời gian chờ để hiện compa thật

    private bool hasDrawn = false;
    private Vector2 startPos;

    // ============================================
    // 🔹 Khởi tạo
    // ============================================
    protected override void Awake()
    {
        base.Awake(); // Gọi base để gán rectTransform, canvas, startPos

        if (rectTransform != null)
            startPos = rectTransform.anchoredPosition;

        // ✅ Trạng thái ban đầu
        if (compassOutline != null) compassOutline.SetActive(true);
        if (compassColored != null) compassColored.SetActive(false);
        if (compassReal != null) compassReal.SetActive(false);
    }

    // ============================================
    // 🔹 Khi bắt đầu kéo
    // ============================================
    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (hasDrawn) return;
        HandleDragStart();
        transform.SetAsLastSibling(); // Đưa phấn lên trên cùng
        Debug.Log("✏️ Bắt đầu kéo phấn!");
    }

    // ============================================
    // 🔹 Khi đang kéo
    // ============================================
    public override void OnDrag(PointerEventData eventData)
    {
        if (hasDrawn) return;
        if (rectTransform != null && canvas != null)
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    // ============================================
    // 🔹 Khi kết thúc kéo (thả chuột)
    // ============================================
    public override void OnEndDrag(PointerEventData eventData)
    {
        HandleDragEnd();

        if (hasDrawn) return;

        // ✅ Kiểm tra chạm đúng hình chưa tô
        if (IsTouching(compassOutline))
        {
            Debug.Log("🧽 Phấn chạm vào hình compa chưa tô!");
            hasDrawn = true;
            StartCoroutine(DrawAndReveal());
        }
        else
        {
            // ↩️ Không trúng → trả phấn về chỗ cũ
            if (rectTransform != null)
                rectTransform.anchoredPosition = startPos;
            else
                transform.position = startPos;

            Debug.Log("↩️ Không chạm đúng, trả phấn về chỗ cũ.");
        }
    }

    // ============================================
    // 🔹 Kiểm tra chạm giữa phấn và hình
    // ============================================
    private bool IsTouching(GameObject target)
    {
        if (target == null || canvas == null) return false;
        RectTransform targetRect = target.GetComponent<RectTransform>();
        if (targetRect == null) return false;

        return RectTransformUtility.RectangleContainsScreenPoint(targetRect, Input.mousePosition, canvas.worldCamera);
    }

    // ============================================
    // 🔹 Coroutine: Tô hình → hiện compa thật → phấn biến mất
    // ============================================
    private IEnumerator DrawAndReveal()
    {
        // ✅ Hiện hình tô màu
        if (compassColored != null) compassColored.SetActive(true);
        Debug.Log("🎨 Đang tô màu compa...");

        // ⏳ Chờ vài giây
        yield return new WaitForSeconds(revealDelay);

        // ✅ Ẩn hình chưa tô và hình tô → hiện compa thật
        if (compassOutline != null) compassOutline.SetActive(false);
        if (compassColored != null) compassColored.SetActive(false);
        if (compassReal != null) compassReal.SetActive(true);

        Debug.Log("🧭 Compa thật đã xuất hiện, phấn sẽ biến mất...");

        // 💨 Ẩn phấn sau khi hoàn tất
        gameObject.SetActive(false);
    }
}
