using UnityEngine;
using TMPro;
using System; // để dùng Action<>
using UnityEngine.SceneManagement; // để bắt sự kiện load scene

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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 🔁 Khi load scene mới, tìm lại các UI object bị mất tham chiếu
        if (timerText == null)
        {
            var timerObj = GameObject.Find("TimerText");
            if (timerObj != null)
                timerText = timerObj.GetComponent<TextMeshProUGUI>();
        }

        if (winUI == null)
        {
            // thử tìm theo tên hoặc theo đường dẫn phổ biến
            var w = GameObject.Find("Congrat");
            if (w == null) w = GameObject.Find("UI Manager/WL Manager/Congrat");
            if (w != null) winUI = w;
        }

        if (loseUI == null)
        {
            var l = GameObject.Find("GameOver");
            if (l == null) l = GameObject.Find("UI Manager/WL Manager/GameOver");
            if (l != null) loseUI = l;
        }

        // Nếu vẫn không tìm thấy, log cảnh báo
        if (winUI == null || loseUI == null)
            Debug.LogWarning("⚠️ GameManager chưa tìm thấy WinUI hoặc LoseUI trong scene mới!");


        // Sau khi tìm lại UI, reset timer
        ResetTimerUI();
        
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
        if (CollectibleManager.Instance != null &&
            CollectibleManager.Instance.GetCollectedCount() >= CollectibleManager.Instance.GetTotalCount() &&
            CollectibleManager.Instance.GetTotalCount() > 0)
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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

    // 🧭 Reset game logic (gọi khi replay)
    public void resetGame()
    {
        ResetTimerUI();
        ChangeState(GameState.Playing);
    }

    // 🔁 Hàm con đặt lại timer, ẩn UI và chạy lại game
    private void ResetTimerUI()
    {
        gameEnded = false;
        currentTime = timeLimit;
        Time.timeScale = 1f;

        if (winUI != null) winUI.SetActive(false);
        if (loseUI != null) loseUI.SetActive(false);

        UpdateTimerText();
    }
}
