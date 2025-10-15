using UnityEngine;
using UnityEngine.EventSystems;

public class ShowPencilOnClick : MonoBehaviour, IPointerClickHandler
{
    [Header("Pencil cần được bật khi click")]
    public GameObject pencil;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (pencil != null)
            pencil.SetActive(true); // Hiện bút chì

        Debug.Log("✏️ Đã tìm thấy cây bút chì!");

        // Ẩn chính object hộp bút sau khi click
        gameObject.SetActive(false);
    }
}
