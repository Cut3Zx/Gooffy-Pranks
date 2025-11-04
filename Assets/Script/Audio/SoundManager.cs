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
    public AudioClip cameraSound;
    public AudioClip errorClickSound;

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
            return;
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;

        // 🔸 Lấy trạng thái lưu từ PlayerPrefs
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

    // 🎵 Toggle nhạc nền
    public void ToggleMusic()
    {
        musicMuted = !musicMuted;
        musicSource.mute = musicMuted;

        PlayerPrefs.SetInt("MusicMuted", musicMuted ? 1 : 0);
        PlayerPrefs.Save();
    }

    // 🪄 Các hàm phát âm thanh
    public void PlayClick()
    {
        if (clickSound != null)
            sfxSource.PlayOneShot(clickSound);
    }

    public void PlayWin()
    {
        if (winSound != null)
            sfxSource.PlayOneShot(winSound);
    }

    public void PlayLose()
    {
        if (loseSound != null)
            sfxSource.PlayOneShot(loseSound);
    }

    public void PlayCamera()
    {
        if (cameraSound != null)
            sfxSource.PlayOneShot(cameraSound);
    }

    public void PlayErrorClick()
    {
        if (errorClickSound != null)
            sfxSource.PlayOneShot(errorClickSound);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}