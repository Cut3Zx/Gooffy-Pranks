using UnityEngine;
using UnityEngine.EventSystems;

public class ChickenClickOnTree : BaseObjectManager
{
    [Header("Hiệu ứng hoàn thành khi bắt gà")]
    public GameObject completeEffect;

    [Header("Thang liên kết")]
    public LadderDrag ladder; // thang có biến isPlaced

    [Header("Các phần hình ảnh con gà")]
    public GameObject chickenWingOnly; // chỉ hiện cánh ban đầu
    public GameObject fullChicken;     // con gà đầy đủ (ẩn ban đầu)

    private bool canClick = false;
    private bool isFullVisible = false; // đã hiện con gà đầy đủ chưa

    public void SetCanClick(bool state)
    {
        canClick = state;
        Debug.Log($"🐔 Gà trên cây {(state ? "có thể click" : "bị khóa")}.");
    }

    protected override void Awake()
    {
        base.Awake();
        if (chickenWingOnly != null) chickenWingOnly.SetActive(true);
        if (fullChicken != null) fullChicken.SetActive(false);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        HandleClick();

        // Nếu chưa được phép click (chưa có thang)
        if (!canClick || (ladder != null && !ladder.isPlaced))
        {
            Debug.Log("🚫 Thang chưa tới, chưa thể bắt gà!");
            return;
        }

        // 👀 Nếu chưa hiện toàn bộ con gà → hiện ra, chưa bắt luôn
        if (!isFullVisible)
        {
            Debug.Log("👀 Người chơi đã phát hiện ra con gà!");
            isFullVisible = true;

            if (chickenWingOnly != null) chickenWingOnly.SetActive(false);
            if (fullChicken != null) fullChicken.SetActive(true);

            return; // ⛔ Dừng ở đây, chưa bắt gà
        }

        // 🐣 Nếu đã hiện đầy đủ → cho phép bắt
        Debug.Log("🐣 Bắt được con gà!");

        // Ẩn con gà
        if (fullChicken != null)
            fullChicken.SetActive(false);

        // Hiện hiệu ứng
        if (completeEffect != null)
            completeEffect.SetActive(true);

        // Báo cho CollectibleManager
        if (CollectibleManager.Instance != null)
            CollectibleManager.Instance.RegisterCollected(gameObject);
    }

    // ❌ Không cho phép kéo
    public override void OnBeginDrag(PointerEventData e) { }
    public override void OnDrag(PointerEventData e) { }
    public override void OnEndDrag(PointerEventData e) { }
}
