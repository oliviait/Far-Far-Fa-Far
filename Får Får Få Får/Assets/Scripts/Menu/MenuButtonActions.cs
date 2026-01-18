using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonActions : MonoBehaviour
{
    public AudioClipGroup ButtonClick;
    private object settingsCanvas;

    public void ContinueGameButtonPressed()
    {
        ButtonClick.Play();
        Player.Instance.Load();
        SceneManager.LoadScene(1);
    }

    public void NewGameButtonPressed()
    {
        Player.Instance.NewGame();
        ButtonClick.Play();
        SettingsManager.Instance.SetTutorialText("" +
            "Congratulations! Your grandfather gave you his farm and sheep, " +
            "so you could become a great sheep battler.\n" +
            "Your first task is to breed 2 sheep. Don't forget your training and select them first.");
        SceneManager.LoadScene(1);
    }

    public void OptionsButtonPressed()
    {
        SettingsManager.Instance.Open();
        ButtonClick.Play();
    }

    public void ExitGameButtonPressed()
    {
        ButtonClick.Play();
        Application.Quit();
    }
}
