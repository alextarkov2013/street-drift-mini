using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    private float musicVolume = 0.7f;
    private float sfxVolume = 0.8f;
    private bool musicEnabled = true;
    private bool sfxEnabled = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void PlaySFX(AudioClip clip)
    {
        if (!sfxEnabled) return;
        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    public void PlayMusic(AudioClip clip)
    {
        if (!musicEnabled) return;
        musicSource.clip = clip;
        musicSource.volume = musicVolume;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        musicSource.volume = musicVolume;
    }

    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
    }

    public void SetMusicEnabled(bool enabled)
    {
        musicEnabled = enabled;
        if (!enabled) musicSource.Stop();
    }

    public void SetSFXEnabled(bool enabled)
    {
        sfxEnabled = enabled;
    }
}
