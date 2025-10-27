using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Button MapButton;

    public void onMapButtonClicked()
    {
        SceneManager.LoadScene(1);
    }
}
