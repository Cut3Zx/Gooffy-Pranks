using UnityEngine;
using UnityEngine.EventSystems;

public class PaperRevealExam : MonoBehaviour, IPointerClickHandler
{
    [Header("Bài kiểm tra sẽ được bật khi click vào tờ giấy")]
    public GameObject examObject;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (examObject != null)
        {
            examObject.SetActive(true);   // Hiện bài kiểm tra
            Debug.Log("📄 Đã mở bài kiểm tra!");

            gameObject.SetActive(false);  // Ẩn tờ giấy vò sau khi mở
        }
    }
}
