using UnityEngine;
using UnityEngine.EventSystems;

public class LadderDrag : BaseObjectManager
{
    private Canvas canvas;

    [Header("State")]
    public bool isPlaced = false;

    [Header("References")]
    public RectTransform snapZone;        // vùng để thang cố định
    public GameObject chickenOnTree;      // gà trên cây
    public Vector2 snapOffset;            // tinh chỉnh vị trí

    protected override void Awake()
    {
        base.Awake(); // gọi Awake() từ BaseObjectManager
        canvas = GetComponentInParent<Canvas>();
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (isPlaced) return;
        HandleDragStart(); // hàm từ class cha
        transform.SetAsLastSibling();
    }

    public override void OnDrag(PointerEventData eventData)
    {
        if (isPlaced) return;
        HandleDragging(eventData); // hàm kéo từ class cha
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        if (isPlaced) return;

        // kiểm tra khi thả gần vùng snap zone (UI)
        if (RectTransformUtility.RectangleContainsScreenPoint(snapZone, Input.mousePosition, canvas.worldCamera))
        {
            Debug.Log("✅ Thang đã được đặt đúng vị trí!");
            rectTransform.anchoredPosition = snapZone.anchoredPosition + snapOffset;
            isPlaced = true;

            EnableChickenInteraction(true);
        }
        else
        {
            Debug.Log("↩️ Thang không đúng vị trí, quay về chỗ cũ!");
            ResetPosition(); // dùng hàm từ class cha
        }

        HandleDragEnd(); // gọi hàm cha (log / sound)
    }

    private void EnableChickenInteraction(bool state)
    {
        if (chickenOnTree != null)
        {
            var clickScript = chickenOnTree.GetComponent<ChickenClickOnTree>();
            if (clickScript != null)
                clickScript.SetCanClick(state);

            var image = chickenOnTree.GetComponent<UnityEngine.UI.Image>();
            if (image != null)
                image.raycastTarget = state;

            Debug.Log($"🐔 Gà {(state ? "có thể click" : "bị khóa")}.");
        }
    }
}
