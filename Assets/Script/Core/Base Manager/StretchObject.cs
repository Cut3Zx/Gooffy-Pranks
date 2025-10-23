using UnityEngine;
using UnityEngine.EventSystems;

public class StretchObject : BaseObjectManager
{
    [Header("Cấu hình kéo dài")]
    public float stretchSpeed = 0.01f;     // tốc độ kéo dài
    public float maxScale = 2.5f;          // giới hạn phóng to
    public float minScale = 1f;            // giới hạn thu nhỏ
    public bool stretchVertical = true;    // kéo theo chiều dọc
    public bool stretchHorizontal = false; // kéo theo chiều ngang

    private Vector2 lastPointerPosition;
    private Vector3 originalScale;
    private Vector3 originalPosition;

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (rectTransform == null) return;

        originalScale = rectTransform.localScale;
        originalPosition = rectTransform.anchoredPosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out lastPointerPosition
        );
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (rectTransform == null) return;

        Vector2 currentPointerPosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            rectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out currentPointerPosition
        );

        Vector2 delta = currentPointerPosition - lastPointerPosition;
        Vector3 newScale = rectTransform.localScale;

        // Giữ vị trí gốc (đầu trên)
        Vector3 pivotFix = rectTransform.position;

        // ✅ Kéo dài xuống dưới
        if (stretchVertical)
        {
            float newY = newScale.y + Mathf.Abs(delta.y) * stretchSpeed;
            newY = Mathf.Clamp(newY, minScale, maxScale);
            rectTransform.localScale = new Vector3(newScale.x, newY, newScale.z);

            // Giữ nguyên đầu trên (pivot top)
            float offsetY = (newY - newScale.y) * rectTransform.rect.height * rectTransform.pivot.y;
            rectTransform.position = pivotFix - new Vector3(0, offsetY, 0);
        }

        // ✅ Kéo sang phải (nếu bật)
        if (stretchHorizontal)
        {
            float newX = newScale.x + Mathf.Abs(delta.x) * stretchSpeed;
            newX = Mathf.Clamp(newX, minScale, maxScale);
            rectTransform.localScale = new Vector3(newX, rectTransform.localScale.y, newScale.z);

            // Giữ nguyên đầu trái (pivot left)
            float offsetX = (newX - newScale.x) * rectTransform.rect.width * rectTransform.pivot.x;
            rectTransform.position = pivotFix + new Vector3(offsetX, 0, 0);
        }

        lastPointerPosition = currentPointerPosition;
    }
}
