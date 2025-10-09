using UnityEngine;
using UnityEngine.EventSystems;

public class PickaxeDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 startPos;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        startPos = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetAsLastSibling(); // Đưa cuốc lên trên cùng
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Di chuyển theo chuột/touch
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Tìm object Rock
        GameObject rock = GameObject.Find("Rock"); // đặt đúng tên của cục đá
        if (rock != null)
        {
            RectTransform rockRect = rock.GetComponent<RectTransform>();

            if (RectTransformUtility.RectangleContainsScreenPoint(rockRect, Input.mousePosition, canvas.worldCamera))
            {
                // Nếu thả vào đá → gọi phá
                rock.GetComponent<RockInteraction>().BreakRock();
            }
        }

        // Trả cuốc về chỗ cũ
        rectTransform.anchoredPosition = startPos;
    }
}
