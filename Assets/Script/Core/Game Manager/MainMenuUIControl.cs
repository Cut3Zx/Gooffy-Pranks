using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIControl : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuUI;     // chứa Play/Setting/Exit
    public GameObject levelSelectUI;  // panel chọn level
    public GameObject pauseUI;
    public void WaybackHome()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
    public void ShowPauseUI()
    {
        if (pauseUI != null)
        {
            pauseUI.SetActive(true);
        }
        if (mainMenuUI != null)
        {
            mainMenuUI.SetActive(false);
        }
        // Tạm dừng game
        Time.timeScale = 0f;
    }
    public void HidePauseUI()
    {
        if (pauseUI != null)
        {
            pauseUI.SetActive(false);
        }
        if (mainMenuUI != null)
        {
            mainMenuUI.SetActive(true);
        }
        Time.timeScale = 1f;
    }
    public void HideSelectUI()
    {
        if (levelSelectUI != null)
        {
            levelSelectUI.SetActive(false);
        }
    }

    public void TogglePauseUI()
    {
        if (pauseUI == null)
            return;

        bool isActive = pauseUI.activeSelf;
        pauseUI.SetActive(!isActive);
        Time.timeScale = isActive ? 1f : 0f;
    }
    // Gọi khi bấm nút Play
    public void OnPlayButton()
    {
        mainMenuUI.SetActive(false);
        levelSelectUI.SetActive(true);
    }

    // Gọi khi bấm nút Back trong menu level
    public void OnBackButton()
    {
        levelSelectUI.SetActive(false);
        mainMenuUI.SetActive(true);
    }

    // Gọi khi chọn level
    public void OnSelectLevel(string sceneName)
    {
        Debug.Log("🔹 Load scene: " + sceneName);
        SceneManager.LoadScene(sceneName);
    }

    // Thoát game
    public void OnExit()
    {
        Application.Quit();
    }
    public void ResetLevel(int levelNumber)
    {
        // Nếu có GameManager → reset dữ liệu trước
        if (GameManager.Instance != null)
        {
            GameManager.Instance.resetGame();
        }

        // 🔹 Tạo tên scene tự động, ví dụ "Level_1", "Level_2"...
        string sceneName = $"Level_{levelNumber}";

        // Load lại scene
        SceneManager.LoadScene(sceneName);
    }

}
