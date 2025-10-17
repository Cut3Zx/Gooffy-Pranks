using UnityEngine;
using UnityEngine.EventSystems;

public class DoorClick : BaseObjectManager
{
    [Header("References")]
    public GameObject chicken;
    public bool hideDoor = true;

    // 🔹 Chỉ cho phép click — không dùng drag
    public override void OnBeginDrag(PointerEventData eventData)
    {
        // Không làm gì cả (vô hiệu hóa kéo)
    }

    public override void OnDrag(PointerEventData eventData)
    {
        // Không làm gì cả
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        // Không làm gì cả
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        HandleClick(); // log “clicked” từ class cha
        Debug.Log("🚪 Door clicked!");

        if (hideDoor)
            gameObject.SetActive(false);

        if (chicken != null)
            chicken.SetActive(true);
    }
}
