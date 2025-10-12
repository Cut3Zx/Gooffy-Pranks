using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager I;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSourcePrefab;

    [Header("Music Clips")]
    public AudioClip mainMenuMusic;
    public AudioClip gameplayMusic;

    [Header("SFX Clips")]
    public AudioClip clickSound;
    public AudioClip errorSound;
    public AudioClip gameOverSound;

    private void Awake()
    {
        if (I == null)
        {
            I = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic(mainMenuMusic);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Contains("MainMenu"))
            PlayMusic(mainMenuMusic);
        else if (scene.name.Contains("Play") || scene.name.Contains("Game"))
            PlayMusic(gameplayMusic);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (clip == null) return;
        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null) return;
        AudioSource sfx = Instantiate(sfxSourcePrefab, transform);
        sfx.clip = clip;
        sfx.volume = volume;
        sfx.Play();
        Destroy(sfx.gameObject, clip.length + 0.1f);
    }
}
