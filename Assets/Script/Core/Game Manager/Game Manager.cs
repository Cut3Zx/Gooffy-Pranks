using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public enum GameState
{
    MainMenu = 0,
    Playing = 1,
    Paused = 2,
    GameOver = 3
}

public class GameManager : MonoBehaviour
{
    [Header("Timer Settings")]
    public float timeLimit = 30f;
    private float currentTime;
    public TextMeshProUGUI timerText;

    [Header("UI References")]
    public GameObject winUI;
    public GameObject loseUI;

    private bool gameEnded = false;

    public static event Action<GameState> OnGameStateChanged;
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

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (timerText == null)
        {
            var timerObj = GameObject.Find("TimerText");
            if (timerObj != null)
                timerText = timerObj.GetComponent<TextMeshProUGUI>();
        }

        if (winUI == null)
        {
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

        if (winUI == null || loseUI == null)
            Debug.LogWarning("⚠️ GameManager chưa tìm thấy WinUI hoặc LoseUI trong scene mới!");

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
            EndGame(false);
        }

        if (CollectibleManager.Instance != null &&
            CollectibleManager.Instance.GetCollectedCount() >= CollectibleManager.Instance.GetTotalCount() &&
            CollectibleManager.Instance.GetTotalCount() > 0)
        {
            EndGame(true);
        }
    }

    private void UpdateTimerText()
    {
        if (timerText != null)
            timerText.text = Mathf.CeilToInt(currentTime).ToString() + "s";
    }

    public void EndGame(bool isWin)
    {
        gameEnded = true;

        if (isWin)
        {
            if (SoundManager.Instance != null)
                SoundManager.Instance.PlayWin();
            HintUI hintUI = FindObjectOfType<HintUI>();
            if (hintUI != null)
                hintUI.AddHint(1);
            if (winUI != null) winUI.SetActive(true);
            UnlockNextLevel();
        }
        else
        {
            if (SoundManager.Instance != null)
                SoundManager.Instance.PlayLose();
            if (loseUI != null) loseUI.SetActive(true);
        }

        Time.timeScale = 0f;
        ChangeState(GameState.GameOver);
    }

    private void UnlockNextLevel()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene.StartsWith("Level"))
        {
            try
            {
                int currentLevelNum = int.Parse(currentScene.Replace("Level", "").Trim('_'));
                int nextLevelNum = currentLevelNum + 1;

                // ✅ Đảm bảo không vượt quá số lượng level hiện có
                string nextKey = $"Level_{nextLevelNum}_Unlocked";
                if (PlayerPrefs.GetInt(nextKey, 0) == 0)
                {
                    PlayerPrefs.SetInt(nextKey, 1);
                    PlayerPrefs.Save();
                    Debug.Log($"🔓 Đã mở khóa Level {nextLevelNum}");
                }
            }
            catch (Exception e)
            {
                Debug.LogError("❌ Lỗi khi mở khóa level kế tiếp: " + e.Message);
            }
        }
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ChangeState(GameState newState)
    {
        if (currentState == newState) return;
        currentState = newState;

        Debug.Log($"Game State changed to {newState}");
        OnGameStateChanged?.Invoke(newState);
    }

    public GameState GetCurrentState() => currentState;

    public void resetGame()
    {
        ResetTimerUI();
        ChangeState(GameState.Playing);
    }

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
