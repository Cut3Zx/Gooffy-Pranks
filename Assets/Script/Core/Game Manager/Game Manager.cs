using UnityEngine;
using TMPro;
using System;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState { MainMenu, Playing, Paused, GameOver }

public class GameManager : MonoBehaviour
{
    [Header("Timer Settings")]
    public float timeLimit = 30f;
    private float currentTime;
    public TextMeshProUGUI timerText;

    [Header("UI References")]
    public GameObject winUI, loseUI, winImage;
    public GameObject bagImage, winBackground;
    public Image flashImage;
    public RectTransform winTarget;

    [Header("Animation Settings")]
    public float flashSpeed = 0.2f, flashAlpha = 1f;
    public float winDelay = 1.2f, flyDuration = 1.2f;
    public float rotateSpeed = 360f, shrinkScale = 0.3f;

    private bool gameEnded;
    private GameState currentState;
    public static GameManager Instance { get; private set; }
    public static event Action<GameState> OnGameStateChanged;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }
        ChangeState(GameState.MainMenu);
    }

    private void OnEnable() => SceneManager.sceneLoaded += OnSceneLoaded;
    private void OnDisable() => SceneManager.sceneLoaded -= OnSceneLoaded;

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        timerText ??= GameObject.Find("TimerText")?.GetComponent<TextMeshProUGUI>();
        winImage ??= GameObject.Find("WinImage");
        winUI ??= GameObject.Find("Congrat") ?? GameObject.Find("UI Manager/WL Manager/Congrat");
        loseUI ??= GameObject.Find("GameOver") ?? GameObject.Find("UI Manager/WL Manager/GameOver");
        flashImage ??= GameObject.Find("FlashEffect")?.GetComponent<Image>();
        winBackground ??= GameObject.Find("WinBG");
        bagImage ??= GameObject.Find("BagImage");
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

        if (currentTime <= 0) EndGame(false);

        var cm = CollectibleManager.Instance;
        if (cm != null && cm.GetCollectedCount() >= cm.GetTotalCount() && cm.GetTotalCount() > 0)
            EndGame(true);
    }

    private void UpdateTimerText()
    {
        if (timerText) timerText.text = Mathf.CeilToInt(currentTime) + "s";
    }

    public void EndGame(bool isWin)
    {
        if (gameEnded) return;
        gameEnded = true;

        if (isWin) StartCoroutine(HandleWinSequence());
        else HandleLose();
        
    }

    private IEnumerator HandleWinSequence()
    {
        SoundManager.Instance?.PlayWin();

        yield return new WaitForSeconds(0.5f);
        if (flashImage) yield return FlashEffectRoutine();

        winBackground?.SetActive(true);
        bagImage?.SetActive(true);

        if (winImage)
        {
            winImage.SetActive(true);
            yield return new WaitForSeconds(3f); // 👈 hiển thị 1 giây trước khi xoay

            var rect = winImage.GetComponent<RectTransform>();
            if (winTarget) yield return AnimateWinImage(rect, winTarget.anchoredPosition);
        }

        UnlockPhoto();
        yield return new WaitForSeconds(winDelay);
        winUI?.SetActive(true);

        FindObjectOfType<HintUI>()?.AddHint(1);
        UnlockNextLevel();

        Time.timeScale = 0f;
        ChangeState(GameState.GameOver);
        string sceneName = SceneManager.GetActiveScene().name;
        int currentLevel = 0;
        if (sceneName.StartsWith("Level"))
        {
            string numeric = System.Text.RegularExpressions.Regex.Replace(sceneName, "[^0-9]", "");
            int.TryParse(numeric, out currentLevel);
        }

        // AdsManager.Instance?.OnLevelCompleted(currentLevel);

    }

    private IEnumerator FlashEffectRoutine()
    {
        if (!flashImage) yield break;
        flashImage.gameObject.SetActive(true);
        var c = flashImage.color;

        for (float t = 0; t < 1; t += Time.unscaledDeltaTime / flashSpeed)
        {
            c.a = Mathf.Lerp(0, flashAlpha, t);
            flashImage.color = c;
            yield return null;
        }

        for (float t = 0; t < 1; t += Time.unscaledDeltaTime / flashSpeed)
        {
            c.a = Mathf.Lerp(flashAlpha, 0, t);
            flashImage.color = c;
            yield return null;
        }

        flashImage.gameObject.SetActive(false);
    }

    private IEnumerator AnimateWinImage(RectTransform rect, Vector2 targetPos)
    {
        if (!rect) yield break;
        var group = rect.GetComponent<CanvasGroup>() ?? rect.gameObject.AddComponent<CanvasGroup>();
        Vector2 startPos = rect.anchoredPosition;
        Vector3 startScale = rect.localScale;
        float elapsed = 0f, rotation = 0f;

        while (elapsed < flyDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = elapsed / flyDuration;
            rect.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            rect.localScale = Vector3.Lerp(startScale, startScale * shrinkScale, t);
            rotation += rotateSpeed * Time.unscaledDeltaTime;
            rect.localRotation = Quaternion.Euler(0, 0, rotation);
            group.alpha = Mathf.Lerp(1, 0, t);
            yield return null;
        }

        rect.gameObject.SetActive(false);
    }

    private void HandleLose()
    {
        SoundManager.Instance?.PlayLose();
        loseUI?.SetActive(true);
        Time.timeScale = 0f;
        ChangeState(GameState.GameOver);
    }

    private void UnlockPhoto()
    {
        string scene = SceneManager.GetActiveScene().name;
        if (!scene.StartsWith("Level_")) return;

        try
        {
            int level = int.Parse(scene.Replace("Level_", "").Trim());
            string key = $"Collected_Level_{level}";

            if (PlayerPrefs.GetInt(key, 0) == 0)
            {
                PlayerPrefs.SetInt(key, 1);
                PlayerPrefs.Save();
                Debug.Log($"📸 Đã mở khóa ảnh cho Level {level}");
            }
        }
        catch (Exception e)
        {
            Debug.LogWarning($"⚠️ Lỗi xác định level: {e.Message}");
        }
    }

    private void UnlockNextLevel()
    {
        string scene = SceneManager.GetActiveScene().name;
        if (!scene.StartsWith("Level")) return;

        try
        {
            int level = int.Parse(scene.Replace("Level", "").Trim('_'));
            string nextKey = $"Level_{level + 1}_Unlocked";

            if (PlayerPrefs.GetInt(nextKey, 0) == 0)
            {
                PlayerPrefs.SetInt(nextKey, 1);
                PlayerPrefs.Save();
                Debug.Log($"🔓 Mở khóa Level {level + 1}");
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"❌ Lỗi mở khóa kế: {e.Message}");
        }
    }

    public void RestartLevel() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    public void ChangeState(GameState newState)
    {
        if (currentState == newState) return;
        currentState = newState;
        OnGameStateChanged?.Invoke(newState);
    }

    public void resetGame()
    {
        ResetTimerUI();
        ChangeState(GameState.Playing);
    }

    private void ResetTimerUI()
    {
        StopAllCoroutines();
        gameEnded = false;
        currentTime = timeLimit;
        Time.timeScale = 1f;

        winUI?.SetActive(false);
        loseUI?.SetActive(false);
        winImage?.SetActive(false);
        bagImage?.SetActive(false);
        winBackground?.SetActive(false);
        if (flashImage) flashImage.gameObject.SetActive(false);

        UpdateTimerText();
    }
}
