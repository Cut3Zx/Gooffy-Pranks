using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SoundEffect
{
    public string name;
    public AudioClip clip;
}

public class SFXManager : MonoBehaviour
{
    public static SFXManager Instance;

    public AudioSource sfxAudioSource;
    public SoundEffect[] soundEffects;

    void Awake()
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

    public void PlaySFX(string soundName)
    {
        if (sfxAudioSource == null)
        {
            Debug.LogError("SFXManager: Chưa gán AudioSource!");
            return;
        }

        SoundEffect sfx = System.Array.Find(soundEffects, s => s.name == soundName);

        if (sfx.clip != null)
        {
            sfxAudioSource.PlayOneShot(sfx.clip);
        }
        else
        {
            Debug.LogWarning("SFXManager: Không tìm thấy âm thanh tên: " + soundName);
        }
    }
}