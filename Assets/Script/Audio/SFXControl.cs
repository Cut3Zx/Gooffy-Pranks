using UnityEngine;
using UnityEngine.Audio;
// using UnityEngine.UI; // Không cần thiết nữa

public class SFXControl : MonoBehaviour
{
    [Header("Audio Mixer")]
    [Tooltip("Kéo AudioMixer Asset của bạn vào đây")]
    [SerializeField] private AudioMixer mainMixer;

    [Header("Tên Tham Số (Parameter)")]
    [Tooltip("Tên chính xác của tham số Volume SFX đã expose")]
    [SerializeField] private string sfxVolumeParam = "SFXVolume";

    // Đã loại bỏ phần [Header("UI (Tùy chọn)")]

    private const float MIN_VOLUME = -80f;
    private const float MAX_VOLUME = 0f;

    // Biến để lưu trạng thái hiện tại
    private bool isMuted;

    private void Start()
    {
        // 1. Load trạng thái đã lưu (0 = bật, 1 = tắt)
        isMuted = PlayerPrefs.GetInt("SFXMuted", 0) == 1;

        // 2. Áp dụng trạng thái này ngay khi bắt đầu
        ApplyMuteState(isMuted);
    }

    /// <summary>
    /// Hàm này sẽ được gán vào sự kiện OnClick của Button trong Inspector.
    /// </summary>
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

    /// <summary>
    /// Hàm riêng để cập nhật Audio Mixer
    /// </summary>
    private void ApplyMuteState(bool muteState)
    {
        if (muteState)
        {
            // TẮT TIẾNG
            mainMixer.SetFloat(sfxVolumeParam, MIN_VOLUME);
            // Đã loại bỏ code cập nhật UI
        }
        else
        {
            // BẬT TIẾNG
            mainMixer.SetFloat(sfxVolumeParam, MAX_VOLUME);
            // Đã loại bỏ code cập nhật UI
        }
    }
}