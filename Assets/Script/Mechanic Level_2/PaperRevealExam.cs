using UnityEngine;
using UnityEngine.EventSystems;

public class PaperRevealExam : BaseObjectManager
{
    [Header("Bài kiểm tra sẽ được bật khi click vào tờ giấy")]
    public GameObject examObject;

    public override void OnBeginDrag(PointerEventData eventData)
    {
        // Không cho kéo
    }

    public override void OnDrag(PointerEventData eventData)
    {
        // Không cho kéo
    }

    public override void OnEndDrag(PointerEventData eventData)
    {
        // Không cho kéo
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        // 👉 Gọi hàm xử lý click cơ bản từ class cha
        HandleClick();

        // 👉 Logic riêng cho tờ giấy vò
        if (examObject != null)
        {
            examObject.SetActive(true);   // Hiện bài kiểm tra
            Debug.Log("📄 Đã mở bài kiểm tra!");
            gameObject.SetActive(false);  // Ẩn tờ giấy vò
        }
    }
}
