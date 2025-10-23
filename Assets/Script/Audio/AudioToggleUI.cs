using UnityEngine;
using UnityEngine.UI;

public class AudioToggleUI : MonoBehaviour
{
    [Header("Button")]
    public Button btnMusic;

    [Header("Icon Image")]
    public Image imgMusic;
    public Sprite iconOn;
    public Sprite iconOff;

    private bool musicOn;

    void Start()
    {
        // Lấy trạng thái lưu từ PlayerPrefs (mặc định bật)
        musicOn = PlayerPrefs.GetInt("MusicMuted", 0) == 0;
        UpdateUI();

        btnMusic.onClick.AddListener(OnToggleMusic);
    }

    void OnToggleMusic()
    {
        SoundManager.Instance.ToggleMusic();

        musicOn = !musicOn;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (imgMusic != null)
            imgMusic.sprite = musicOn ? iconOn : iconOff;
    }
}
