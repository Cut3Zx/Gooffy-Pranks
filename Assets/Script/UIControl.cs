using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIControl : MonoBehaviour
{
    [Header("Các Game Object UI")]
    // Kéo thả panel Pause vào đây trong Inspector
    public GameObject pauseUI;
    public GameObject MainUI;

    public void WaybackHome()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    // Nếu game cần tạm dừng khi hiện Pause, dùng Time.timeScale = 0
    // Gọi ShowPauseUI() để hiện panel và tạm dừng game
    public void ShowPauseUI()
    {
        if (pauseUI != null)
        {
            pauseUI.SetActive(true);
        }
        if (MainUI != null)
        {
            MainUI.SetActive(false);
        }
        // Tạm dừng game
        Time.timeScale = 0f;
    }

    // Gọi HidePauseUI() để ẩn panel và tiếp tục game
    public void HidePauseUI()
    {
        if (pauseUI != null)
        {
            pauseUI.SetActive(false);
        }
        if (MainUI != null)
        {
            MainUI.SetActive(true);
        }
        Time.timeScale = 1f;
    }

    // Chuyển trạng thái hiện/ẩn
    public void TogglePauseUI()
    {
        if (pauseUI == null)
            return;

        bool isActive = pauseUI.activeSelf;
        pauseUI.SetActive(!isActive);
        Time.timeScale = isActive ? 1f : 0f;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
