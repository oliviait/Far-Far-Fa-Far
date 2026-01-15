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
        SceneManager.LoadScene(1);
    }

    public void OptionsButtonPressed()
    {
        ButtonClick.Play();
    }

    public void ExitGameButtonPressed()
    {
        ButtonClick.Play();
        Application.Quit();
    }
}
