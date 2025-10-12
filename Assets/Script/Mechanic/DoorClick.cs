using UnityEngine;
using UnityEngine.EventSystems;

public class DoorClick : MonoBehaviour, IPointerClickHandler
{
    [Header("Object xuất hiện sau khi bấm")]
    public GameObject chicken; // con gà sẽ xuất hiện

    [Header("Tuỳ chọn")]
    public bool hideDoor = true; // ẩn cửa khi bấm

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("🚪 Door clicked!");

        // Ẩn cửa nếu bật tuỳ chọn
        if (hideDoor)
            gameObject.SetActive(false);

        // Hiện con gà
        if (chicken != null)
            chicken.SetActive(true);
    }
}
