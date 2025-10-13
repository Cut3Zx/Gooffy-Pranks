using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private LoadingUI loadingUI;

    private void Start()
    {
        loadingUI.LoadingGame();
    }
    private void OnEnable()
    {
        // Đăng ký sự kiện
        GameManager.OnGameStateChanged += HandleGameStateChanged;
    }

    private void OnDisable()
    {
        // Hủy đăng ký để tránh memory leak
        GameManager.OnGameStateChanged -= HandleGameStateChanged;
    }

    private void HandleGameStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.MainMenu:
                ShowMainMenu();
                break;

            case GameState.Playing:
                ShowGameplayUI();
                break;

            case GameState.Paused:
                ShowPauseMenu();
                break;

            case GameState.GameOver:
                ShowGameOver();
                break;
        }
    }

    // --- Các hàm UI đơn giản ---
    public void ShowMainMenu()
    {
        loadingUI.LoadingGame();
    }
    public void ShowGameplayUI() => Debug.Log("Hiện GameplayUI");
    public void ShowPauseMenu() => Debug.Log("Hiện PauseMenu");
    public void ShowGameOver() => Debug.Log("Hiện GameOver");
}