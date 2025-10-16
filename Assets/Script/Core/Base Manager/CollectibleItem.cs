using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Quản lý trạng thái của MỌI vật phẩm có thể thu thập trong game.
/// Có thể là gà, bút, giấy, bài kiểm tra, chìa khóa,...
/// Không tự xử lý click/va chạm, chỉ dùng để đánh dấu, cộng điểm, và gọi hiệu ứng.
/// </summary>
public class CollectibleItem : MonoBehaviour
{
    [Header("Interaction Settings")]
    public bool enabledInteraction = true;   // có thể thu thập không
    public bool disableAfterCollect = true;  // tự ẩn object sau khi thu thập
    public bool canCollectOnce = true;       // chỉ thu thập 1 lần

    [Header("Scoring")]
    public int scoreValue = 10;              // điểm cộng khi thu thập
    public string itemType = "Generic";      // loại vật phẩm (Chicken, Pencil, Paper,...)

    [Header("Local Events")]
    public UnityEvent onCollected;           // hiệu ứng local (âm thanh, particle,...)

    private bool isCollected = false;

    /// <summary>
    /// Gọi khi người chơi nhặt, click, hoặc hoàn thành điều kiện để thu thập vật phẩm.
    /// </summary>
    public void MarkCollected()
    {
        if (!enabledInteraction)
        {
            Debug.Log($"⚠️ {gameObject.name} đang bị khóa, không thể thu thập!");
            return;
        }

        if (isCollected && canCollectOnce)
        {
            Debug.Log($"✅ {gameObject.name} đã được thu thập trước đó, bỏ qua!");
            return;
        }

        isCollected = true;

        // 🔹 Gọi hiệu ứng cục bộ (âm thanh, animation, particle,...)
        onCollected?.Invoke();

        // 🔹 Báo cho hệ thống trung tâm (CollectibleManager)
        if (CollectibleManager.Instance != null)
        {
            CollectibleManager.Instance.RegisterCollected(gameObject);
            Debug.Log($"🐥 {gameObject.name} đã được thu thập và gửi tới CollectibleManager!");
        }
        else
        {
            Debug.LogWarning($"⚠️ Không tìm thấy CollectibleManager trong Scene!");
        }

        // 🔹 Cộng điểm nếu có hệ thống điểm
        //if (GameScoreManager.Instance != null)
        //{
        //    GameScoreManager.Instance.AddScore(scoreValue);
        //}

        // 🔹 Ẩn object nếu được bật tuỳ chọn
        if (disableAfterCollect)
        {
            gameObject.SetActive(false);
        }

        Debug.Log($"🏆 {gameObject.name} ({itemType}) đã được thu thập, +{scoreValue} điểm!");
    }

    /// <summary>
    /// Cho phép reset trạng thái (dùng khi restart level).
    /// </summary>
    public void ResetCollected()
    {
        isCollected = false;
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);
        Debug.Log($"🔄 {gameObject.name} đã được reset trạng thái thu thập!");
    }

    /// <summary>
    /// Trả về trạng thái hiện tại của vật phẩm.
    /// </summary>
    public bool IsCollected() => isCollected;
}
