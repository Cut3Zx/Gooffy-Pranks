using UnityEngine;

public class FindDifferenceManager : MonoBehaviour
{
    public static FindDifferenceManager Instance { get; private set; }

    [Header("Tổng số điểm khác biệt cần tìm")]
    public int totalDifferences = 5;

    [Header("UI thắng khi tìm đủ")]
    public GameObject winUI;

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
            ShowWin();
    }

    private void ShowWin()
    {
        if (winUI != null)
            winUI.SetActive(true);

        Debug.Log("🏆 Tìm đủ điểm khác biệt — WIN!");
    }
}
