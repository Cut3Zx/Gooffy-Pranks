using UnityEngine;
using UnityEngine.EventSystems;

public class PickaxeDrag : BaseObjectManager
{
    [Header("Âm Thanh sau khi chạm")]
    public string hitSoundName;
    private Canvas canvas;
    private Vector2 startPos;

    protected override void Awake()
    {
        base.Awake(); // gọi hàm Awake() của BaseObjectManager
        canvas = GetComponentInParent<Canvas>();
        if (rectTransform != null)
            startPos = rectTransform.anchoredPosition;
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        HandleDragStart(); // log / hiệu ứng kéo nếu có
        transform.SetAsLastSibling(); // Đưa cuốc lên trên cùng
    }

    public override void OnDrag(PointerEventData eventData)
    {
        HandleDragging(eventData); // dùng hàm di chuyển từ Base
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        HandleDragEnd(); // log / âm thanh kết thúc nếu có

        // 🔍 Tìm object Rock
        GameObject rock = GameObject.Find("Rock"); // Đặt đúng tên cục đá
        if (rock != null)
        {
            RectTransform rockRect = rock.GetComponent<RectTransform>();

            // Nếu thả trúng vùng cục đá
            if (RectTransformUtility.RectangleContainsScreenPoint(rockRect, Input.mousePosition, canvas.worldCamera))
            {
                RockInteraction rockScript = rock.GetComponent<RockInteraction>();
                if (rockScript != null)
                    rockScript.BreakRock();
                // Phát âm thanh chạm
                if (SFXManager.Instance != null && !string.IsNullOrEmpty(hitSoundName))
                {
                    SFXManager.Instance.PlaySFX(hitSoundName);
                }
            }
        }

        // Trả cuốc về chỗ cũ
        if (rectTransform != null)
            rectTransform.anchoredPosition = startPos;
    }
}
