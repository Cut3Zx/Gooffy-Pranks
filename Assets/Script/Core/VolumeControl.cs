using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeControl : MonoBehaviour
{
    [Header("Optional: AudioMixer (if you use one)")]
    public AudioMixer mixer; // nếu dùng AudioMixer, set vào đây
    public string masterMixerParam = "Master";
    public string musicMixerParam = "Music";
    public string sfxMixerParam = "SFX";

    [Header("Direct AudioSource control (fallback)")]
    // Bạn có thể kéo các AudioSource vào inspector; nếu để trống, script sẽ tìm các GameObject có tag tương ứng
    public List<AudioSource> musicSources = new List<AudioSource>();
    public List<AudioSource> sfxSources = new List<AudioSource>();

    [Header("Volumes (0..1)")]
    [Range(0f, 1f)] public float masterVolume = 1f;
    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    [Header("Mute flags")]
    public bool masterMuted = false;
    public bool musicMuted = false;
    public bool sfxMuted = false;

    // Lưu volume gốc của các AudioSource để có thể nhân với musicVolume/sfxVolume
    Dictionary<AudioSource, float> originalSourceVolumes = new Dictionary<AudioSource, float>();

    void Awake()
    {
        CacheAssignedSources();
        ApplyAllVolumes();
    }

    void CacheAssignedSources()
    {
        // Nếu danh sách để trống, thử tìm theo tag "Music" và "SFX" (người dùng cần đặt Tag)
        if (musicSources.Count == 0)
        {
            GameObject[] musicObjs = GameObject.FindGameObjectsWithTag("Music");
            foreach (var go in musicObjs)
            {
                var src = go.GetComponent<AudioSource>();
                if (src != null) musicSources.Add(src);
            }
        }

        if (sfxSources.Count == 0)
        {
            GameObject[] sfxObjs = GameObject.FindGameObjectsWithTag("SFX");
            foreach (var go in sfxObjs)
            {
                var src = go.GetComponent<AudioSource>();
                if (src != null) sfxSources.Add(src);
            }
        }

        originalSourceVolumes.Clear();
        foreach (var src in musicSources)
        {
            if (src != null && !originalSourceVolumes.ContainsKey(src))
                originalSourceVolumes[src] = src.volume;
        }
        foreach (var src in sfxSources)
        {
            if (src != null && !originalSourceVolumes.ContainsKey(src))
                originalSourceVolumes[src] = src.volume;
        }
    }

    // Public API: set volumes (0..1)
    public void SetMasterVolume(float v)
    {
        masterVolume = Mathf.Clamp01(v);
        ApplyMasterVolume();
    }

    public void SetMusicVolume(float v)
    {
        musicVolume = Mathf.Clamp01(v);
        ApplyMusicVolume();
    }

    public void SetSFXVolume(float v)
    {
        sfxVolume = Mathf.Clamp01(v);
        ApplySFXVolume();
    }

    // Toggle mutes
    public void ToggleMasterMute()
    {
        masterMuted = !masterMuted;
        ApplyMasterVolume();
    }

    public void ToggleMusicMute()
    {
        musicMuted = !musicMuted;
        ApplyMusicVolume();
    }

    public void ToggleSFXMute()
    {
        sfxMuted = !sfxMuted;
        ApplySFXVolume();
    }

    // Áp dụng tất cả
    public void ApplyAllVolumes()
    {
        ApplyMasterVolume();
        ApplyMusicVolume();
        ApplySFXVolume();
    }

    void ApplyMasterVolume()
    {
        if (mixer != null)
        {
            // convert linear 0..1 to dB; when volume==0 set -80 dB (effectively silence)
            float dB = (masterMuted || masterVolume <= 0f) ? -80f : Mathf.Log10(Mathf.Max(0.0001f, masterVolume)) * 20f;
            mixer.SetFloat(masterMixerParam, dB);
        }
        else
        {
            // fallback global volume
            AudioListener.volume = (masterMuted ? 0f : masterVolume);
        }
    }

    void ApplyMusicVolume()
    {
        if (mixer != null)
        {
            float dB = (musicMuted || musicVolume <= 0f) ? -80f : Mathf.Log10(Mathf.Max(0.0001f, musicVolume)) * 20f;
            mixer.SetFloat(musicMixerParam, dB);
        }
        else
        {
            foreach (var src in musicSources)
            {
                if (src == null) continue;
                float baseVol = originalSourceVolumes.ContainsKey(src) ? originalSourceVolumes[src] : 1f;
                src.volume = (musicMuted ? 0f : baseVol * musicVolume);
            }
        }
    }

    void ApplySFXVolume()
    {
        if (mixer != null)
        {
            float dB = (sfxMuted || sfxVolume <= 0f) ? -80f : Mathf.Log10(Mathf.Max(0.0001f, sfxVolume)) * 20f;
            mixer.SetFloat(sfxMixerParam, dB);
        }
        else
        {
            foreach (var src in sfxSources)
            {
                if (src == null) continue;
                float baseVol = originalSourceVolumes.ContainsKey(src) ? originalSourceVolumes[src] : 1f;
                src.volume = (sfxMuted ? 0f : baseVol * sfxVolume);
            }
        }
    }

    // Helper: nếu muốn cập nhật runtime khi thay đổi từ Inspector
    void OnValidate()
    {
        // chỉ áp dụng khi chơi hoặc trong Editor để preview
        if (!Application.isPlaying)
        {
            CacheAssignedSources();
            ApplyAllVolumes();
        }
    }

    // ---------------------------
    // UI wiring helpers (OnClick / OnValueChanged)
    // Gọi những hàm này trực tiếp từ Button.OnClick hoặc Slider.OnValueChanged
    // ---------------------------

    // Slider callbacks (float parameter)
    public void OnMasterSliderChanged(float value)
    {
        // Nếu người dùng kéo slider lên > 0, tự động unmute master
        if (value > 0f) masterMuted = false;
        SetMasterVolume(value);
    }

    public void OnMusicSliderChanged(float value)
    {
        if (value > 0f) musicMuted = false;
        SetMusicVolume(value);
    }

    public void OnSFXSliderChanged(float value)
    {
        if (value > 0f) sfxMuted = false;
        SetSFXVolume(value);
    }

    // Button callbacks (no parameter)
    public void OnMasterMuteButton()
    {
        ToggleMasterMute();
    }

    public void OnMusicMuteButton()
    {
        ToggleMusicMute();
    }

    public void OnSFXMuteButton()
    {
        ToggleSFXMute();
    }

    // Quick preset buttons
    public void OnMuteAllButton()
    {
        masterMuted = true;
        musicMuted = true;
        sfxMuted = true;
        ApplyAllVolumes();
    }

    public void OnUnmuteAllButton()
    {
        masterMuted = false;
        musicMuted = false;
        sfxMuted = false;
        ApplyAllVolumes();
    }
}
