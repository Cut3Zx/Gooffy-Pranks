using UnityEngine;
using TMPro;
using System; // để dùng Action<>

// 🌟 Enum định nghĩa trạng thái của game
public enum GameState
{
    MainMenu = 0,
    Playing = 1,
    Paused = 2,
    GameOver = 3
}

// 🌟 Quản lý toàn bộ logic game (thời gian, UI, state, v.v.)
public class GameManager : MonoBehaviour
{
    [Header("Timer Settings")]
    public float timeLimit = 30f; // thời gian giới hạn (giây)
    private float currentTime;
    public TextMeshProUGUI timerText;

    [Header("UI References")]
    public GameObject winUI;
    public GameObject loseUI;

    private bool gameEnded = false;

    // 🔔 Sự kiện khi thay đổi trạng thái game
    public static event Action<GameState> OnGameStateChanged;

    // 🔧 Singleton (chỉ 1 GameManager trong scene)
    public static GameManager Instance { get; private set; }

    private GameState currentState;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        ChangeState(GameState.MainMenu);
    }

    private void Start()
    {
        Time.timeScale = 1f;
        currentTime = timeLimit;
        UpdateTimerText();
    }

    private void Update()
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
            EndGame(true); // thắng
        }
    }

    // 🕒 Cập nhật thời gian hiển thị
    private void UpdateTimerText()
    {
        if (timerText != null)
        {
            timerText.text = Mathf.CeilToInt(currentTime).ToString() + "s";
        }
    }

    // 🎯 Kết thúc game (thắng hoặc thua)
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

        Time.timeScale = 0f;
        ChangeState(GameState.GameOver);
    }

    // 🔄 Restart lại màn chơi
    public void RestartLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
        );
    }

    // 🔁 Thay đổi trạng thái game
    public void ChangeState(GameState newState)
    {
        if (currentState == newState) return;
        currentState = newState;

        Debug.Log($"Game State changed to {newState}");
        OnGameStateChanged?.Invoke(newState);
    }

    // 🧩 Lấy state hiện tại
    public GameState GetCurrentState()
    {
        return currentState;
    }
}
