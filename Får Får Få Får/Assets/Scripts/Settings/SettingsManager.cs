using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

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

        LoadSettings();
        ApplyVolumes();
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
        SettingsButton.Instance.Close();
    }

    void LoadSettings()
    {
        musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        gameSoundsVolume = PlayerPrefs.GetFloat("GameSoundsVolume", 1f);
    }

    public void BackToMenu()
    {
        SettingsButton.Instance.Close();
        
        // TODO save game state
        SceneManager.LoadScene(0);
    }
}