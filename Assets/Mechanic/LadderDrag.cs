using UnityEngine;
using UnityEngine.EventSystems;

public class LadderDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 startPos;
    private bool isPlaced = false;

    [Header("References")]
    public RectTransform snapZone;        // vùng để thang cố định
    public GameObject chickenOnTree;      // gà trên cây (sẽ bật click khi thang đúng chỗ)
    public Vector2 snapOffset;            // tinh chỉnh vị trí dính so với zone

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        startPos = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isPlaced) return;
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isPlaced) return;
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isPlaced) return;

        // Kiểm tra nếu thả gần zone
        if (RectTransformUtility.RectangleContainsScreenPoint(snapZone, Input.mousePosition, canvas.worldCamera))
        {
            Debug.Log("🪜 Thang chạm vùng snap zone!");

            // Đặt thang cố định tại zone
            rectTransform.anchoredPosition = snapZone.anchoredPosition + snapOffset;
            isPlaced = true;

            EnableChickenInteraction(true);
        }
        else
        {
            // Không đúng chỗ thì quay về vị trí cũ
            rectTransform.anchoredPosition = startPos;
        }
    }

    private void EnableChickenInteraction(bool state)
    {
        if (chickenOnTree != null)
        {
            var clickScript = chickenOnTree.GetComponent<ChickenClick>();
            var image = chickenOnTree.GetComponent<UnityEngine.UI.Image>();

            if (clickScript != null)
                clickScript.enabled = state;

            if (image != null)
                image.raycastTarget = state;

            Debug.Log($"🐔 Gà {(state ? "có thể click" : "bị khóa")}.");
        }
    }
}
