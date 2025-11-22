using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public static Settings Instance;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        gameObject.SetActive(scene.buildIndex != 3);    // Disable, if in Menu scene
    }

    public void SettingsButtonClicked() // Also connect in editor
    {
        // Show settings panel
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;  
    }
}
