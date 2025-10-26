using UnityEngine;
using UnityEngine.EventSystems;

public class TrashCanInteract : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("Thùng rác sau khi đổ")]
    public GameObject spilledTrashCan;   // prefab hoặc object hình thùng rác bị đổ (ẩn sẵn)

    [Header("Đống rác xuất hiện sau khi đổ")]
    public GameObject trashPile;         // đống rác (ẩn sẵn)

    private bool hasSpilled = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (hasSpilled) return;

        hasSpilled = true;
        Debug.Log("🗑️ Thùng rác bị đổ!");

        // Ẩn thùng rác cũ
        gameObject.SetActive(false);

        // Hiện thùng rác bị đổ
        if (spilledTrashCan != null)
            spilledTrashCan.SetActive(true);

        // Hiện đống rác
        if (trashPile != null)
            trashPile.SetActive(true);
    }

    // 🚫 Ngăn không cho drag
    public void OnBeginDrag(PointerEventData eventData) { /* không làm gì cả */ }
    public void OnDrag(PointerEventData eventData) { /* không làm gì cả */ }
    public void OnEndDrag(PointerEventData eventData) { /* không làm gì cả */ }
}
