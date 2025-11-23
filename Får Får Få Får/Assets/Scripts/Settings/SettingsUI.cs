using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    public Slider musicSlider;
    public Slider gameSoundsSlider;

    void OnEnable()
    {
        if (SettingsManager.Instance != null)
        {
            musicSlider.value = SettingsManager.Instance.musicVolume;
            gameSoundsSlider.value = SettingsManager.Instance.gameSoundsVolume;
        }
    }


    public void OnMusicSliderChanged(float value)
    {
        SettingsManager.Instance.SetMusicVolume(value);
    }

    public void OnGameSoundsSliderChanged(float value)
    {
        SettingsManager.Instance.SetGameSoundsVolume(value);
    }


    public void OnSaveButton()
    {
        SettingsManager.Instance.SaveSettings();
    }

    public void OnBackButton()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OnQuitButton()
    {
        SettingsManager.Instance.SaveSettings();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}