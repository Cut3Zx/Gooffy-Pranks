using UnityEngine;
using UnityEngine.EventSystems;

public class LadderDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 startPos;

    [Header("State")]
    public bool isPlaced = false;

    [Header("References")]
    public RectTransform snapZone;        // vùng để thang cố định
    public GameObject chickenOnTree;      // gà trên cây
    public Vector2 snapOffset;            // tinh chỉnh vị trí

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
            rectTransform.anchoredPosition = startPos;
        }
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
