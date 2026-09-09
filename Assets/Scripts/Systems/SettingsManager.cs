using UnityEngine;
using System;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    [System.Serializable]
    public class GameSettings
    {
        public bool musicEnabled = true;
        public bool sfxEnabled = true;
        public bool vibrationEnabled = true;
        public int graphicsQuality = 1; // 0=LOW, 1=MEDIUM, 2=HIGH
        public float controlSensitivity = 1f;
        public float musicVolume = 0.7f;
        public float sfxVolume = 0.8f;
    }

    public GameSettings settings = new GameSettings();
    public Action OnSettingsChanged;

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

    public void ApplySettings()
    {
        AudioManager.Instance?.SetMusicEnabled(settings.musicEnabled);
        AudioManager.Instance?.SetSFXEnabled(settings.sfxEnabled);
        AudioManager.Instance?.SetMusicVolume(settings.musicVolume);
        AudioManager.Instance?.SetSFXVolume(settings.sfxVolume);

        QualitySettings.SetQualityLevel(settings.graphicsQuality);
        OnSettingsChanged?.Invoke();
    }

    public void SaveSettings()
    {
        string json = JsonUtility.ToJson(settings, true);
        PlayerPrefs.SetString("GameSettings", json);
        PlayerPrefs.Save();
    }

    public void LoadSettings()
    {
        if (PlayerPrefs.HasKey("GameSettings"))
        {
            string json = PlayerPrefs.GetString("GameSettings");
            settings = JsonUtility.FromJson<GameSettings>(json);
        }
        ApplySettings();
    }
}
