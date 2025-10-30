using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelSelectManager : MonoBehaviour
{
    [System.Serializable]
    public class LevelButton
    {
        [Header("Cấu hình Level")]
        public string sceneName;          // Tên scene (Level_1, Level_2,...)
        public Button button;             // Button chính
        public Image backgroundImage;     // Ảnh nền / thumbnail của Level
        public TextMeshProUGUI label;     // Text "Level 1", "Level 2" ...
        public GameObject lockIcon;       // Icon khóa
    }

    [Header("Danh sách Level Buttons")]
    public LevelButton[] levels;

    [Header("Màu hiển thị")]
    public Color unlockedColor = Color.white; // sáng
    public Color lockedColor = new Color(1f, 1f, 1f, 0.35f); // mờ

    private void Start()
    {
        // ✅ Luôn mở khóa Level_1
        if (!PlayerPrefs.HasKey("Level_1_Unlocked"))
        {
            PlayerPrefs.SetInt("Level_1_Unlocked", 1);
            PlayerPrefs.Save();
        }

        UpdateLevelButtons();
    }

    public void UpdateLevelButtons()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            string key = $"Level_{i + 1}_Unlocked";
            bool unlocked = PlayerPrefs.GetInt(key, 0) == 1;

            var lv = levels[i];
            if (lv.button == null) continue;

            // ✅ Giao diện
            lv.button.interactable = unlocked;

            // Làm mờ ảnh nền
            if (lv.backgroundImage != null)
                lv.backgroundImage.color = unlocked ? unlockedColor : lockedColor;

            // Chữ "Level" luôn sáng, chỉ giảm alpha nhẹ
            if (lv.label != null)
                lv.label.color = unlocked ? Color.white : new Color(1f, 1f, 1f, 0.6f);

            // Ẩn/hiện ổ khóa
            if (lv.lockIcon != null)
                lv.lockIcon.SetActive(!unlocked);

            // ✅ Bắt sự kiện load scene
            int index = i;
            lv.button.onClick.RemoveAllListeners();
            lv.button.onClick.AddListener(() => LoadLevel(index));
        }
    }

    public void LoadLevel(int index)
    {
        if (index < 0 || index >= levels.Length) return;

        string sceneName = levels[index].sceneName;
        if (!string.IsNullOrEmpty(sceneName))
        {
            Debug.Log($"🎮 Loading: {sceneName}");
            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning($"⚠️ Scene name for Level {index + 1} is missing!");
        }
    }
    public void RefreshLevelsUI()
    {
        Debug.Log("🔁 Làm mới giao diện Level Select...");

        for (int i = 0; i < levels.Length; i++)
        {
            string key = $"Level_{i + 1}_Unlocked";
            bool unlocked = PlayerPrefs.GetInt(key, 0) == 1;

            var lv = levels[i];
            if (lv.button == null) continue;

            // ✅ Cập nhật trạng thái UI
            lv.button.interactable = unlocked;

            if (lv.backgroundImage != null)
                lv.backgroundImage.color = unlocked ? unlockedColor : lockedColor;

            if (lv.label != null)
                lv.label.color = unlocked ? Color.white : new Color(1f, 1f, 1f, 0.6f);

            if (lv.lockIcon != null)
                lv.lockIcon.SetActive(!unlocked);
        }

        Debug.Log("✅ Làm mới xong trạng thái tất cả Level Buttons!");
    }

}
