using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIControl : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuUI;
    public GameObject levelSelectUI;
    public GameObject pauseUI;

    public void WaybackHome()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ShowPauseUI()
    {
        if (pauseUI != null) pauseUI.SetActive(true);
        if (mainMenuUI != null) mainMenuUI.SetActive(false);
        Time.timeScale = 0f;
    }

    public void HidePauseUI()
    {
        if (pauseUI != null) pauseUI.SetActive(false);
        if (mainMenuUI != null) mainMenuUI.SetActive(true);
        Time.timeScale = 1f;
    }

    public void HideSelectUI()
    {
        if (levelSelectUI != null) levelSelectUI.SetActive(false);
    }

    public void TogglePauseUI()
    {
        if (pauseUI == null) return;
        bool isActive = pauseUI.activeSelf;
        pauseUI.SetActive(!isActive);
        Time.timeScale = isActive ? 1f : 0f;
    }

    public void OnPlayButton()
    {
        mainMenuUI.SetActive(false);
        levelSelectUI.SetActive(true);
    }

    public void OnBackButton()
    {
        levelSelectUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }

    public void OnSelectLevel(string sceneName)
    {
        Debug.Log("🔹 Load scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    public void AutoNextLevel()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene.StartsWith("Level"))
        {
            try
            {
                int currentLevel = int.Parse(currentScene.Replace("Level_", ""));
                int nextLevel = currentLevel + 1;
                string nextSceneName = $"Level_{nextLevel}";
                Debug.Log($"➡️ Chuyển từ {currentScene} sang {nextSceneName}...");
                SceneManager.LoadScene(nextSceneName);
            }
            catch
            {
                Debug.LogWarning("⚠️ Không thể xác định level hiện tại!");
            }
        }
        else
        {
            Debug.LogWarning("⚠️ Không phải scene Level_...");
        }
    }

    // ✅ NEW — Retry tự động lấy tên scene hiện tại
    public void RetryCurrentLevel()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        Debug.Log($"🔁 Retry lại scene: {currentScene}");

        // Nếu có GameManager, reset lại dữ liệu
        if (GameManager.Instance != null)
        {
            GameManager.Instance.resetGame();
        }

        // Load lại chính scene hiện tại
        SceneManager.LoadScene(currentScene);
    }

    public void OnExit()
    {
        Application.Quit();
    }

    public void ResetLevel(int levelNumber)
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.resetGame();
        }

        string sceneName = $"Level_{levelNumber}";
        SceneManager.LoadScene(sceneName);
    }
}
