using UnityEngine;
using UnityEngine.UI;

public class SFXToggleUI : MonoBehaviour
{
    [Header("Button")]
    public Button btnSFX;

    [Header("Icon Image")]
    public Image imgSFX;
    public Sprite iconOn;
    public Sprite iconOff;

    private bool sfxOn;

    void Start()
    {
        sfxOn = PlayerPrefs.GetInt("SfxMuted", 0) == 0;
        UpdateUI();

        btnSFX.onClick.AddListener(OnToggleSFX);
    }

    void OnToggleSFX()
    {
        if (SoundManager.Instance != null)
            SoundManager.Instance.ToggleSFX();

        sfxOn = !sfxOn;
        UpdateUI();
    }

    void UpdateUI()
    {
        if (imgSFX != null)
            imgSFX.sprite = sfxOn ? iconOn : iconOff;
    }
}
