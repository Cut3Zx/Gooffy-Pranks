using UnityEngine;
using UnityEngine.EventSystems;

public class ChickenClickOnTree : BaseObjectManager
{
    [Header("Hiệu ứng hoàn thành khi bắt gà")]
    public GameObject completeEffect;

    [Header("Thang liên kết")]
    public LadderDrag ladder; // thang có biến isPlaced

    private bool canClick = false;

    // ✅ Cho phép hoặc khóa click (từ script khác)
    public void SetCanClick(bool state)
    {
        canClick = state;
        Debug.Log($"🐔 Gà trên cây {(state ? "có thể click" : "bị khóa")}.");
    }

    // ✅ Khi click vào con gà
    public override void OnPointerClick(PointerEventData eventData)
    {
        HandleClick(); // log click cơ bản

        // ⚠️ Kiểm tra điều kiện
        if (!canClick || (ladder != null && !ladder.isPlaced))
        {
            Debug.Log("🚫 Thang chưa tới, chưa thể bắt gà!");
            return;
        }

        Debug.Log("🐣 Bắt được con gà!");

        // Ẩn con gà
        gameObject.SetActive(false);

        // Hiện hiệu ứng
        if (completeEffect != null)
            completeEffect.SetActive(true);

        // Gửi thông báo cho hệ thống CollectibleManager
        if (CollectibleManager.Instance != null)
            CollectibleManager.Instance.RegisterCollected(gameObject);
    }

    // ❌ Không cho phép kéo
    public override void OnBeginDrag(PointerEventData e) { }
    public override void OnDrag(PointerEventData e) { }
    public override void OnEndDrag(PointerEventData e) { }
}
