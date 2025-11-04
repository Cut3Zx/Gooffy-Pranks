using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AlbumManager : MonoBehaviour
{
    [System.Serializable]
    public class AlbumItem
    {
        [Header("Cấu hình ảnh")]
        public Image image;               // Ảnh chính hiển thị
        public TextMeshProUGUI label;     // Tên ảnh (Level 1, Level 2,...)
        public Sprite unlockedSprite;     // Ảnh thật khi mở khóa
        public TextMeshProUGUI description;   // Mô tả ảnh (nếu cần)
    }

    [Header("Danh sách ảnh Album (theo thứ tự Level)")]
    public AlbumItem[] albumItems;

    [Header("Ảnh khi chưa mở khóa")]
    public Sprite lockedSprite;           // ảnh đen hoặc khung mờ khi chưa mở khóa

    [Header("Màu hiển thị")]
    public Color unlockedColor = Color.white;
    public Color lockedColor = new Color(1f, 1f, 1f, 0.35f); // làm mờ

    private void Start()
    {
        RefreshAlbum();
    }

    public void RefreshAlbum()
    {
        for (int i = 0; i < albumItems.Length; i++)
        {
            string key = $"Collected_Level_{i + 1}";
            bool unlocked = PlayerPrefs.GetInt(key, 0) == 1;

            AlbumItem item = albumItems[i];

            if (item.image != null)
            {
                item.image.sprite = unlocked ? item.unlockedSprite : lockedSprite;
                item.image.color = unlocked ? unlockedColor : lockedColor;
            }

            if (item.label != null)
                item.label.text = $"Level {i + 1}";

            if (item.description != null)
                item.description.text = unlocked ? $"Ảnh Level {i + 1} đã mở khóa." : "Ảnh chưa được mở khóa.";

        }

        Debug.Log("📸 Album đã được cập nhật hiển thị!");
    }
}
