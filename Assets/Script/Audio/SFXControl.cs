using UnityEngine;
using UnityEngine.Audio;

public class SFXControl : MonoBehaviour
{
    [Header("Audio Mixer")]
    [Tooltip("Kéo AudioMixer Asset của bạn vào đây")]
    [SerializeField] private AudioMixer mainMixer;

    [Header("Tên Tham Số (Parameter)")]
    [Tooltip("Tên chính xác của tham số Volume SFX đã expose")]
    [SerializeField] private string sfxVolumeParam = "SFXVolume";
    private const float MIN_VOLUME = -80f;
    private const float MAX_VOLUME = 0f;
    public static SFXControl Instance;

    private void Awake() 
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Biến để lưu trạng thái hiện tại
    private bool isMuted;

    private void Start()
    {
        // 1. Load trạng thái đã lưu (0 = bật, 1 = tắt)
        isMuted = PlayerPrefs.GetInt("SFXMuted", 0) == 1;

        // 2. Áp dụng trạng thái này ngay khi bắt đầu
        ApplyMuteState(isMuted);
    }

    public void ToggleMute()
    {
        // 1. Đảo ngược trạng thái
        isMuted = !isMuted;

        // 2. Áp dụng trạng thái mới
        ApplyMuteState(isMuted);

        // 3. Lưu lại lựa chọn của người dùng
        PlayerPrefs.SetInt("SFXMuted", isMuted ? 1 : 0);
        PlayerPrefs.Save();
    }


    private void ApplyMuteState(bool muteState)
    {
        if (muteState)
        {
            // TẮT TIẾNG
            mainMixer.SetFloat(sfxVolumeParam, MIN_VOLUME);
        }
        else
        {
            // BẬT TIẾNG
            mainMixer.SetFloat(sfxVolumeParam, MAX_VOLUME);
        }
    }
}