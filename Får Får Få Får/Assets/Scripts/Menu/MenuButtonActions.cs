using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonActions : MonoBehaviour
{
    public AudioClipGroup ButtonClick;

    public void ContinueGameButtonPressed()
    {
        ButtonClick.Play();
        Player.Instance.Load();
        SceneManager.LoadScene(0);

    }

    public void NewGameButtonPressed()
    {
        Player.Instance.NewGame();
        ButtonClick.Play();
        SceneManager.LoadScene(0);
    }

    public void OptionsButtonPressed()
    {
        ButtonClick.Play();
        return;
    }

    public void ExitGameButtonPressed()
    {
        ButtonClick.Play();
        Application.Quit();
    }
}
