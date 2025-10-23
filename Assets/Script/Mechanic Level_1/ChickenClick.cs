using UnityEngine;
using UnityEngine.EventSystems;

public class ChickenClick : BaseObjectManager
{
    private CollectibleItem collectibleItem; // Tham chiếu tới CollectibleItem (thay cho ChickItem cũ)

    protected override void Awake()
    {
        base.Awake(); // Gọi Awake() từ BaseObjectManager
        collectibleItem = GetComponent<CollectibleItem>(); // Tự tìm trong cùng GameObject
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        // ✅ Gọi xử lý click cơ bản từ class cha (âm thanh / hiệu ứng / log)
        HandleClick();

        // 🐣 Đánh dấu đã thu thập
        if (collectibleItem != null)
        {
            collectibleItem.MarkCollected();
            
        }
        else
        {
            Debug.LogWarning($"⚠️ {gameObject.name} không có CollectibleItem để đăng ký vào hệ thống!");
        }
    }
    public override void OnBeginDrag(PointerEventData e) { }
    public override void OnDrag(PointerEventData e) { }
    public override void OnEndDrag(PointerEventData e) { }
}

