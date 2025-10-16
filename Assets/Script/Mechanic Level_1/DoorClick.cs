using UnityEngine;
using UnityEngine.EventSystems;

public class DoorClick : BaseObjectManager
{
    public GameObject chicken;
    public bool hideDoor = true;

    public override void OnPointerClick(PointerEventData eventData)
    {
        // Chỉ dùng chức năng click từ cha
        HandleClick();

        Debug.Log("🚪 Door clicked!");

        if (hideDoor)
            gameObject.SetActive(false);

        if (chicken != null)
            chicken.SetActive(true);
    }
}
