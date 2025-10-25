using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] private GameObject loadingScreenUI; // UI của màn hình tải
    [SerializeField] private Slider progressBar; // Thanh tiến trình

    // Hàm để bắt đầu tải cảnh
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    // Coroutine để tải cảnh bất đồng bộ
    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // Hiển thị màn hình tải
        loadingScreenUI.SetActive(true);

        // Bắt đầu tải cảnh
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // Đảm bảo cảnh không tự động chuyển đổi khi tải xong
        operation.allowSceneActivation = false;

        // Cập nhật thanh tiến trình
        while (!operation.isDone)
        {
            // Tiến trình tải (0 đến 0.9 là tải, 0.9 đến 1 là chờ kích hoạt)
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            progressBar.value = progress;

            // Kích hoạt cảnh khi tải xong
            if (operation.progress >= 0.9f)
            {
                progressBar.value = 1f;
                operation.allowSceneActivation = true;
            }

            // Thêm độ trễ để làm chậm quá trình tải
            yield return new WaitForSeconds(1f); // Chờ 0.1 giây mỗi vòng lặp
        }

        // Ẩn màn hình tải sau khi hoàn tất
        loadingScreenUI.SetActive(false);
    }
}
