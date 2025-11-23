using UnityEngine;

public class OpenSettings : MonoBehaviour
{
    public GameObject settingsCanvas;   

    public void Open()
    {
        settingsCanvas.SetActive(true);
        Time.timeScale = 0f;

}

public void Close()
    {
        settingsCanvas.SetActive(false);
        Time.timeScale = 1f;
    }
}