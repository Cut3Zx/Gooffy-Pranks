using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelCompleteManager : MonoBehaviour
{
    public void OnLevelComplete()
    {
        string currentScene = SceneManager.GetActiveScene().name;

        // Lấy số level hiện tại (ví dụ: Level_1 -> 1)
        int currentLevelNum = int.Parse(currentScene.Split('_')[1]);
        int nextLevelNum = currentLevelNum + 1;

        string nextKey = $"Level_{nextLevelNum}_Unlocked";

        // Mở khóa level kế tiếp
        PlayerPrefs.SetInt(nextKey, 1);
        PlayerPrefs.Save();

        Debug.Log($"✅ Mở khóa Level {nextLevelNum}");

        // Quay về màn chọn level (nếu muốn)
        SceneManager.LoadScene("LevelSelect"); // đổi tên scene cho đúng
    }
}
