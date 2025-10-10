using UnityEngine;
using UnityEngine.EventSystems;

public class ChickenClick : MonoBehaviour, IPointerClickHandler
{
    public GameObject completeEffect; // hiệu ứng hoặc thông báo hoàn thành (tùy chọn)
    private ChickItem chickItem; // tham chiếu đến script ChickItem cùng object

    void Awake()
    {
        chickItem = GetComponent<ChickItem>(); // tự tìm trong cùng GameObject
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"🐣 Bắt được con gà: {gameObject.name}");

        // Ẩn gà khi được click
        gameObject.SetActive(false);

        // Gọi hiệu ứng hoặc popup hoàn thành
        if (completeEffect != null)
            completeEffect.SetActive(true);

        // ✅ Gọi đếm trong hệ thống CountingChick
        if (chickItem != null)
        {
            chickItem.MarkFound();
        }
        else
        {
            Debug.LogWarning($"⚠️ {gameObject.name} không có ChickItem để đăng ký vào CountingChick!");
        }
    }
}
