using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    [Header("Timer")]
    public float timeLimit = 30f; // thời gian giới hạn (giây)
    private float currentTime;
    public TextMeshProUGUI timerText;

    [Header("UI References")]
    public GameObject winUI;
    public GameObject loseUI;

    private bool gameEnded = false;

    void Start()
    {
        currentTime = timeLimit;
        UpdateTimerText();
    }

    void Update()
    {
        if (gameEnded) return;

        currentTime -= Time.deltaTime;
        UpdateTimerText();

        if (currentTime <= 0)
        {
            EndGame(false); // thua vì hết giờ
        }

        // Kiểm tra nếu đã tìm đủ gà
        if (CountingChick.Instance != null &&
            CountingChick.Instance.GetFoundCount() >= CountingChick.Instance.GetTotalCount() &&
            CountingChick.Instance.GetTotalCount() > 0)
        {
            EndGame(true);
        }
    }

    void UpdateTimerText()
    {
        if (timerText != null)
        {
            timerText.text = Mathf.CeilToInt(currentTime).ToString() + "s";
        }
    }

    public void EndGame(bool isWin)
    {
        gameEnded = true;

        if (isWin)
        {
            if (winUI != null) winUI.SetActive(true);
        }
        else
        {
            if (loseUI != null) loseUI.SetActive(true);
        }

        // Dừng thời gian game
        Time.timeScale = 0f;
    }

    public void RestartLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }
}
