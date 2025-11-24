using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioClipGroup MainMenuMusic;
    public AudioClipGroup FarmMusic;
    public AudioClipGroup MapMusic;
    public AudioClipGroup BattleMusic;
    public AudioSource MusicAudioSource;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += PlaySceneMusic;

        if (SettingsManager.Instance != null)
        {
            MusicAudioSource.volume = SettingsManager.Instance.musicVolume;
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
            SceneManager.sceneLoaded -= PlaySceneMusic;
    }

    void PlaySceneMusic(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 1) FarmMusic.Play(MusicAudioSource);
        else if (scene.buildIndex == 2) MapMusic.Play(MusicAudioSource);
        else if (scene.buildIndex == 3) BattleMusic.Play(MusicAudioSource);
        else if (scene.buildIndex == 0) MainMenuMusic.Play(MusicAudioSource);

        if (SettingsManager.Instance != null)
        {
            MusicAudioSource.volume = SettingsManager.Instance.musicVolume;
        }
    }

    public void SetMusicVolume(float volume)
    {
        if (MusicAudioSource != null)
            MusicAudioSource.volume = volume;
    }
}