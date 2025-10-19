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
        public string sceneName;        // Tên scene tương ứng (Level_1, Level_2,...)
        public Button button;           // Nút UI của level
        public TextMeshProUGUI label;   // Dòng chữ hiển thị số Level (tùy chọn)
        public GameObject lockIcon;     // Icon khóa (nếu có)
    }

    [Header("Danh sách Level Buttons")]
    public LevelButton[] levels;

    [Header("Màu hiển thị")]
    public Color unlockedColor = Color.white; // Màu sáng khi mở khóa
    public Color lockedColor = new Color(0.4f, 0.4f, 0.4f, 0.6f); // Màu xám mờ khi bị khóa

    private void Start()
    {
        // Mở khóa Level_1 mặc định nếu chưa có dữ liệu
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

            Button btn = levels[i].button;
            Image img = btn.GetComponent<Image>();
            TextMeshProUGUI txt = levels[i].label;
            GameObject lockIcon = levels[i].lockIcon;

            // Kích hoạt / vô hiệu hóa button
            btn.interactable = unlocked;

            // Làm sáng hoặc tối màu
            if (img != null)
                img.color = unlocked ? unlockedColor : lockedColor;

            if (txt != null)
                txt.color = unlocked ? Color.white : new Color(1f, 1f, 1f, 0.5f);

            // Ẩn/hiện icon khóa
            if (lockIcon != null)
                lockIcon.SetActive(!unlocked);
        }
    }

    public void LoadLevel(int index)
    {
        string sceneName = levels[index].sceneName;
        SceneManager.LoadScene(sceneName);
    }
}
