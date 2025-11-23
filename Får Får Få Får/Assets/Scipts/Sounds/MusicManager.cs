using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public AudioClipGroup MainMenuMusic;
    public AudioClipGroup FarmMusic;
    public AudioClipGroup MapMusic;
    public AudioClipGroup BattleMusic;
    public AudioSource MusicAudioSource;

    private void Awake()
    {
        SceneManager.sceneLoaded += PlaySceneMusic;
        DontDestroyOnLoad(gameObject);
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= PlaySceneMusic;
    }

    void PlaySceneMusic(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0) FarmMusic.Play(MusicAudioSource);
        else if (scene.buildIndex == 1) MapMusic.Play(MusicAudioSource);
        else if (scene.buildIndex == 2) BattleMusic.Play(MusicAudioSource);
        else if (scene.buildIndex == 3) MainMenuMusic.Play(MusicAudioSource);
    }
}
