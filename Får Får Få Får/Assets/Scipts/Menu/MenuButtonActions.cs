using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuButtonActions : MonoBehaviour
{
    public void ContinueGameButtonPressed()
    {
        SceneManager.LoadScene(0);
    }

    public void NewGameButtonPressed()
    {
        SceneManager.LoadScene(0);
    }

    public void OptionsButtonPressed()
    {
        return;
    }

    public void ExitGameButtonPressed()
    {
        Application.Quit();
    }
}
