using UnityEngine;
using System.Collections;

public class FindDifferenceManager : MonoBehaviour
{
    public static FindDifferenceManager Instance { get; private set; }

    [Header("Tổng số điểm khác biệt cần tìm")]
    public int totalDifferences = 5;

    [Header("UI thắng khi tìm đủ")]
    public GameObject winUI;

    [Header("⏱️ Thời gian chờ trước khi hiển thị win (giây)")]
    public float winDelay = 0.8f;

    private int foundCount = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void RegisterFound()
    {
        foundCount++;
        Debug.Log($"🔍 Đã tìm được {foundCount}/{totalDifferences}");

        if (foundCount >= totalDifferences)
            StartCoroutine(ShowWinWithDelay());
        SFXManager.Instance.PlaySFX("Dung");
    }

    private IEnumerator ShowWinWithDelay()
    {
        Debug.Log("🎯 Đã tìm đủ khác biệt — chuẩn bị hiện Win!");
        yield return new WaitForSeconds(winDelay);

        // Hiện UI thắng
        if (winUI != null)
            winUI.SetActive(true);

        // 🏆 Gọi GameManager nếu có
        if (GameManager.Instance != null)
            GameManager.Instance.EndGame(true);

        // 🔓 Mở khóa màn kế tiếp (nếu có hệ thống mở khóa)


        Debug.Log("🏆 Tìm đủ điểm khác biệt — WIN!");
    }
}
