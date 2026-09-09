using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private SaveSystem saveSystem;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private SettingsManager settingsManager;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // Initialize core systems
        if (saveSystem == null)
            saveSystem = GetComponent<SaveSystem>();
        if (audioManager == null)
            audioManager = GetComponent<AudioManager>();
        if (settingsManager == null)
            settingsManager = GetComponent<SettingsManager>();

        saveSystem?.LoadGame();
        settingsManager?.ApplySettings();
    }

    public void LoadScene(string sceneName)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneName);
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
    }

    public SaveSystem GetSaveSystem() => saveSystem;
    public AudioManager GetAudioManager() => audioManager;
    public SettingsManager GetSettingsManager() => settingsManager;
}
