using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUIControl : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuUI;
    public GameObject levelSelectUI;
    public GameObject albumUI;
    public GameObject pauseUI;
    public GameObject shopUI;
    public GameObject adsblockUI; // MỚI THÊM

    [Header("Loading (Prefab)")]
    [SerializeField] private GameObject loadingPrefab;
    [SerializeField] private string loadingResourceName = "SceneLoading";

    // ---------- Helpers ----------
    void EnsureLoader()
    {
        if (SimpleSceneLoader.I != null) return;

        if (loadingPrefab != null)
            Instantiate(loadingPrefab);
        else
        {
            var go = Resources.Load<GameObject>(loadingResourceName);
            if (go != null) Instantiate(go);
        }
    }

    void LoadWithLoading(string sceneName)
    {
        Time.timeScale = 1f;
        EnsureLoader();

        if (SimpleSceneLoader.I != null)
            SimpleSceneLoader.I.Load(sceneName);
        else
            SceneManager.LoadScene(sceneName);
    }
    // -----------------------------

    private void Start()
    {
        int shouldOpenSelect = PlayerPrefs.GetInt("OpenLevelSelect", 0);

        if (shouldOpenSelect == 1)
        {
            // ✅ Reset flag sau khi mở 1 lần
            PlayerPrefs.SetInt("OpenLevelSelect", 0);
            PlayerPrefs.Save();

            if (mainMenuUI) mainMenuUI.SetActive(false);
            if (levelSelectUI) levelSelectUI.SetActive(true);

            Debug.Log("📜 Tự động mở Level Select sau khi quay lại Main Menu!");
        }
        else
        {
            // ✅ Trạng thái mặc định: hiện menu chính
            if (mainMenuUI) mainMenuUI.SetActive(true);
            if (levelSelectUI) levelSelectUI.SetActive(false);
        }

        // Ẩn tất cả các panel phụ khi bắt đầu
        if (albumUI) albumUI.SetActive(false);
        if (shopUI) shopUI.SetActive(false);
        if (adsblockUI) adsblockUI.SetActive(false); // MỚI THÊM
    }

    public void WaybackHome() => LoadWithLoading("MainMenu");

    // ✅ Gọi hàm này để quay về MainMenu + mở LevelSelect đúng lúc
    public void GoToLevelSelect()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene != "MainMenu")
        {
            Debug.Log("🏠 Quay về MainMenu và mở Level Select...");
            PlayerPrefs.SetInt("OpenLevelSelect", 1);
            PlayerPrefs.Save();
            LoadWithLoading("MainMenu");
        }
        else
        {
            // Nếu đang ở menu sẵn → chỉ mở bình thường
            if (mainMenuUI) mainMenuUI.SetActive(false);
            if (levelSelectUI) levelSelectUI.SetActive(true);
            if (albumUI) albumUI.SetActive(false);
            if (shopUI) shopUI.SetActive(false);
            if (adsblockUI) adsblockUI.SetActive(false); // MỚI THÊM
        }
    }

    public void ShowPauseUI()
    {
        if (pauseUI) pauseUI.SetActive(true);
        if (mainMenuUI) mainMenuUI.SetActive(false);
        Time.timeScale = 0f;
    }

    public void HidePauseUI()
    {
        if (pauseUI) pauseUI.SetActive(false);
        if (mainMenuUI) mainMenuUI.SetActive(true);
        Time.timeScale = 1f;
    }

    public void HideSelectUI()
    {
        if (levelSelectUI) levelSelectUI.SetActive(false);
    }

    public void TogglePauseUI()
    {
        if (!pauseUI) return;
        bool isActive = pauseUI.activeSelf;
        pauseUI.SetActive(!isActive);
        Time.timeScale = isActive ? 1f : 0f;
    }

    public void OnPlayButton()
    {
        if (mainMenuUI) mainMenuUI.SetActive(false);
        if (levelSelectUI) levelSelectUI.SetActive(true);
        if (albumUI) albumUI.SetActive(false);
        if (shopUI) shopUI.SetActive(false);
        if (adsblockUI) adsblockUI.SetActive(false); // MỚI THÊM
    }

    public void OnBackButton()
    {
        if (levelSelectUI) levelSelectUI.SetActive(false);
        if (mainMenuUI) mainMenuUI.SetActive(true);
    }

    public void OnSelectLevel(string sceneName)
    {
        Debug.Log("🔹 Load scene: " + sceneName);
        LoadWithLoading(sceneName);
    }

    public void AutoNextLevel()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene.StartsWith("Level_"))
        {
            try
            {
                int currentLevel = int.Parse(currentScene.Replace("Level_", ""));
                string nextSceneName = $"Level_{currentLevel + 1}";

                if (Application.CanStreamedLevelBeLoaded(nextSceneName))
                {
                    Debug.Log($"➡️ {currentScene} -> {nextSceneName}");
                    LoadWithLoading(nextSceneName);
                }
                else
                {
                    int randomLevel = Random.Range(1, currentLevel + 1);
                    LoadWithLoading($"Level_{randomLevel}");
                }
            }
            catch { Debug.LogWarning("⚠️ Không thể xác định level hiện tại!"); }
        }
        else Debug.LogWarning("⚠️ Không phải scene Level_...");
    }

    public void RetryCurrentLevel()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        Debug.Log($"🔁 Retry: {currentScene}");
        if (GameManager.Instance) GameManager.Instance.resetGame();
        LoadWithLoading(currentScene);
    }

    public void OnExit() => Application.Quit();

    public void ResetLevel(int levelNumber)
    {
        if (GameManager.Instance) GameManager.Instance.resetGame();
        LoadWithLoading($"Level_{levelNumber}");
    }

    // ----- Unlock / Lock All -----
    public void UnlockAllLevels()
    {
        int totalLevels = 50;
        for (int i = 1; i <= totalLevels; i++)
            PlayerPrefs.SetInt($"Level_{i}_Unlocked", 1);

        PlayerPrefs.Save();
        Debug.Log($"🔓 Mở khóa toàn bộ {totalLevels} màn!");
        var levelManager = FindObjectOfType<LevelSelectManager>();
        if (levelManager) levelManager.RefreshLevelsUI();
    }

    public void LockAllLevelsExceptFirst()
    {
        int totalLevels = 50;
        for (int i = 1; i <= totalLevels; i++)
            PlayerPrefs.SetInt($"Level_{i}_Unlocked", i == 1 ? 1 : 0);

        PlayerPrefs.Save();
        Debug.Log("🔒 Khóa toàn bộ (trừ Level_1).");
        var levelManager = FindObjectOfType<LevelSelectManager>();
        if (levelManager) levelManager.RefreshLevelsUI();
    }

    // ----- Album UI -----
    public void OnOpenAlbum()
    {
        if (mainMenuUI) mainMenuUI.SetActive(false);
        if (levelSelectUI) levelSelectUI.SetActive(false);
        if (albumUI) albumUI.SetActive(true);
        if (shopUI) shopUI.SetActive(false);
        if (adsblockUI) adsblockUI.SetActive(false); // MỚI THÊM

        var albumManager = albumUI.GetComponent<AlbumManager>();
        if (albumManager != null)
            albumManager.RefreshAlbum();
    }

    public void OnCloseAlbum()
    {
        if (albumUI) albumUI.SetActive(false);
        if (mainMenuUI) mainMenuUI.SetActive(true);
    }

    // ----- Shop UI -----
    public void OnOpenShop()
    {
        if (mainMenuUI) mainMenuUI.SetActive(false);
        if (levelSelectUI) levelSelectUI.SetActive(false);
        if (albumUI) albumUI.SetActive(false);
        if (shopUI) shopUI.SetActive(true);
        if (adsblockUI) adsblockUI.SetActive(false); // MỚI THÊM
    }

    public void OnCloseShop()
    {
        if (shopUI) shopUI.SetActive(false);
        if (mainMenuUI) mainMenuUI.SetActive(true);
    }

    // ----- Adsblock UI ----- // MỚI THÊM TOÀN BỘ PHẦN NÀY
    public void OnOpenAdsblock()
    {
        if (adsblockUI) adsblockUI.SetActive(true);
    }

    public void OnCloseAdsblock()
    {
        if (adsblockUI) adsblockUI.SetActive(false);
    }
}