using UnityEngine;
using TMPro;

public class ElephantOrderManager : MonoBehaviour
{
    public static ElephantOrderManager Instance;

    [Header("🐘 Danh sách các voi (gán đúng thứ tự từ dưới lên)")]
    public ElephantClick[] elephants;

    [Header("UI Thắng / Thua")]
    public GameObject winUI;
    public GameObject loseUI;

    private int currentClick = 0;
    private bool gameEnded = false;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        foreach (var e in elephants)
            e.ResetNumber();
    }

    public void OnElephantClicked(ElephantClick clicked)
    {
        if (gameEnded) return;
        if (currentClick >= elephants.Length) return;

        currentClick++;

        // ✅ Hiển thị số thứ tự bấm
        clicked.ShowNumber(currentClick);

        // Khi đã bấm hết 5 con → kiểm tra đúng thứ tự hay không
        if (currentClick >= elephants.Length)
        {
            CheckWinCondition();
        }
    }

    private void CheckWinCondition()
    {
        bool correct = true;

        // Kiểm tra thứ tự bấm có đúng theo danh sách không
        for (int i = 0; i < elephants.Length; i++)
        {
            if (elephants[i].clickOrder != i + 1)
            {
                correct = false;
                break;
            }
        }

        gameEnded = true;

        if (correct)
        {
            Debug.Log("🏆 Bấm đúng thứ tự → Thắng!");
            if (winUI != null) winUI.SetActive(true);
            if (GameManager.Instance != null)
                GameManager.Instance.EndGame(true);
        }
        else
        {
            Debug.Log("❌ Sai thứ tự → Thua!");
            if (loseUI != null) loseUI.SetActive(true);
            if (GameManager.Instance != null)
                GameManager.Instance.EndGame(false);
        }
    }
}
