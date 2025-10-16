using UnityEngine;
using UnityEngine.EventSystems;

public class RakeDrag : BaseObjectManager
{
    private Canvas canvas;

    [Header("References")]
    public GameObject hay;    // Đống lúa
    public GameObject chick;  // Gà con

    protected override void Awake()
    {
        base.Awake(); // gọi Awake() từ BaseObjectManager
        canvas = GetComponentInParent<Canvas>();
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        HandleDragStart(); // Gọi hàm của class cha (log / hiệu ứng)
        transform.SetAsLastSibling(); // Đưa cào lên trên cùng
    }

    public override void OnDrag(PointerEventData eventData)
    {
        HandleDragging(eventData); // Di chuyển theo chuột/touch
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        HandleDragEnd(); // Gọi hàm của class cha

        if (hay == null || chick == null) return;

        RectTransform hayRect = hay.GetComponent<RectTransform>();

        // Kiểm tra nếu thả trúng đống lúa
        if (RectTransformUtility.RectangleContainsScreenPoint(hayRect, Input.mousePosition, canvas.worldCamera))
        {
            Debug.Log("🧹 Cào chạm đống lúa — Gà con xuất hiện!");
            hay.SetActive(false);
            chick.SetActive(true);
        }

        // ✅ Trả cào về vị trí cũ (dùng hàm sẵn của BaseObjectManager)
        ResetPosition();
    }
}
