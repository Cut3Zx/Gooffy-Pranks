using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Quản lý trạng thái của từng con gà (đã tìm thấy hay chưa),
/// KHÔNG tự xử lý click, trigger, hoặc va chạm.
/// Chỉ có thể được đánh dấu tìm thấy bằng cách gọi MarkFound() từ script khác.
/// </summary>
public class ChickItem : MonoBehaviour
{
    [Header("Interaction")]
    public bool enabledInteraction = true;
    public bool disableAfterFound = true; // disable GameObject sau khi được tìm (tùy chọn)

    [Header("Local Events")]
    public UnityEvent onLocalFound; // hiệu ứng local khi được tìm

    private bool isFound = false;

    /// <summary>
    /// Gọi hàm này từ script khác (vd: khi người chơi click hoặc hoàn thành nhiệm vụ)
    /// để đánh dấu con gà đã được tìm thấy.
    /// </summary>
    public void MarkFound()
    {
        if (!enabledInteraction || isFound)
            return;

        isFound = true;

        // Gọi hiệu ứng cục bộ (animation, âm thanh, v.v.)
        onLocalFound?.Invoke();

        // Báo cho hệ thống CountingChick
        if (CountingChick.Instance != null)
        {
            CountingChick.Instance.RegisterFound(this.gameObject);
        }

        // Ẩn nếu cần
        if (disableAfterFound)
        {
            gameObject.SetActive(false);
        }

        Debug.Log($"🐣 {gameObject.name} đã được đánh dấu là tìm thấy!");
    }

    /// <summary>
    /// Cho phép reset lại trạng thái (nếu cần chơi lại level).
    /// </summary>
    public void ResetFound()
    {
        isFound = false;
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
    }
}
