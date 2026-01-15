using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingsButton : MonoBehaviour
{
    public static SettingsButton Instance;
    public GameObject settingsPanel;


    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        gameObject.SetActive(scene.buildIndex != 0);
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
}
