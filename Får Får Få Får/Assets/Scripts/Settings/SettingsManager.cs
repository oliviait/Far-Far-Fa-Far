using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }
    public GameObject settingsPanel;
    public GameObject settingsButton;

    [Range(0f, 1f)] public float musicVolume = 1f;
    [Range(0f, 1f)] public float gameSoundsVolume = 1f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        
        SceneManager.sceneLoaded += OnSceneLoaded;

        LoadSettings();
        ApplyVolumes();
    }
    
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        settingsButton.SetActive(scene.buildIndex != 0);
    }

    public void Open()
    {
        settingsPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Close()
    {
        settingsPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void SetMusicVolume(float value)
    {
        musicVolume = value;
        ApplyMusicVolume();
    }

    public void SetGameSoundsVolume(float value)
    {
        gameSoundsVolume = value;
        ApplyGameSoundsVolume();
    }

    public void ApplyVolumes()
    {
        ApplyMusicVolume();
        ApplyGameSoundsVolume();
    }

    void ApplyMusicVolume()
    {
        if (AudioManager.Instance != null)
            AudioManager.Instance.SetMusicVolume(musicVolume);
    }


    void ApplyGameSoundsVolume()
    {
        if (AudioSourcePool.Instance != null)
        {
            AudioSourcePool.Instance.SetSfxVolume(gameSoundsVolume);
        }
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
        PlayerPrefs.SetFloat("GameSoundsVolume", gameSoundsVolume);
        PlayerPrefs.Save();
        Close();
    }

    void LoadSettings()
    {
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        gameSoundsVolume = PlayerPrefs.GetFloat("GameSoundsVolume", 1f);
    }

    public void BackToMenu()
    {
        Close();
        
        // TODO save game state
        SceneManager.LoadScene(0);
    }
}