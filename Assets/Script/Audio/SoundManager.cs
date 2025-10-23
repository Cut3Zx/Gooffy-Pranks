using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    [Header("Music Clips")]
    public AudioClip mainMenuMusic;
    public AudioClip gameplayMusic;

    [Header("SFX Clips")]
    public AudioClip clickSound;
    public AudioClip winSound;
    public AudioClip loseSound;

    private bool musicMuted;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        // Lấy trạng thái lưu từ PlayerPrefs
        musicMuted = PlayerPrefs.GetInt("MusicMuted", 0) == 1;
        musicSource.mute = musicMuted;

        PlayMusicForScene(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    private void PlayMusicForScene(string sceneName)
    {
        AudioClip clip = null;

        if (sceneName.Contains("MainMenu"))
            clip = mainMenuMusic;
        else if (sceneName.Contains("Level"))
            clip = gameplayMusic;

        if (clip == null) return;

        musicSource.clip = clip;
        musicSource.loop = true;
        if (!musicMuted)
            musicSource.Play();
    }

    // ======================
    // 🔊 Chỉ tắt/bật nhạc nền
    // ======================
    public void ToggleMusic()
    {
        musicMuted = !musicMuted;
        musicSource.mute = musicMuted;

        PlayerPrefs.SetInt("MusicMuted", musicMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    // ======================
    // 🔔 Luôn cho phép phát SFX
    // ======================
    public void PlayClick() { sfxSource.PlayOneShot(clickSound); }
    public void PlayWin() { sfxSource.PlayOneShot(winSound); }
    public void PlayLose() { sfxSource.PlayOneShot(loseSound); }
}
