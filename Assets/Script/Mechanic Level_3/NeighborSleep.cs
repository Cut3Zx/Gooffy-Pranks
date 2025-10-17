using UnityEngine;
using UnityEngine.EventSystems;

public class NeighborSleep : BaseObjectManager
{
    [Header("Objects chuyển trạng thái")]
    public GameObject neighborSleepObj;   // 💤 Ông đang ngủ
    public GameObject neighborAwakeObj;   // 😮 Ông đã thức

    [Header("Mèo sẽ bị ẩn khi ông dậy")]
    public GameObject catObject;

    [Header("Tham chiếu đến GameManager")]
    public GameManager gameManager;

    private bool isAwake = false;

    protected override void Awake()
    {
        base.Awake();

        // ✅ Đảm bảo trạng thái ban đầu
        if (neighborSleepObj != null) neighborSleepObj.SetActive(true);
        if (neighborAwakeObj != null) neighborAwakeObj.SetActive(false);
    }

    public void WakeUp()
    {
        if (isAwake) return;
        isAwake = true;

        Debug.Log("😮 Ông hàng xóm đã tỉnh dậy!");

        // 🔄 Ẩn bản ngủ - Hiện bản thức
        if (neighborSleepObj != null) neighborSleepObj.SetActive(false);
        if (neighborAwakeObj != null) neighborAwakeObj.SetActive(true);

        // 🐈 Ẩn mèo (nếu có)
        if (catObject != null)
        {
            catObject.SetActive(false);
            Debug.Log("🐈 Mèo bị ẩn khi ông dậy.");
        }

        // 🏆 Gọi GameManager thắng
        if (gameManager == null)
            gameManager = FindFirstObjectByType<GameManager>();

        if (gameManager != null)
        {
            Debug.Log("🏆 Chiến thắng! Ông hàng xóm tỉnh dậy!");
            gameManager.EndGame(true);
        }
        else
        {
            Debug.LogWarning("⚠️ Không tìm thấy GameManager trong scene!");
        }
    }

    // ❌ Không cho click hoặc kéo
    public override void OnPointerClick(PointerEventData e) { }
    public override void OnBeginDrag(PointerEventData e) { }
    public override void OnDrag(PointerEventData e) { }
    public override void OnEndDrag(PointerEventData e) { }
}
