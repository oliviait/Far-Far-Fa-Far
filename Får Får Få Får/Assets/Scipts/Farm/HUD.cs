using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Button MapButton;
    public Button BreedButton;

    public AudioClipGroup MapCrunch;

    public void onMapButtonClicked()
    {
        MapCrunch.Play();
        SceneManager.LoadScene(1);
    }

    public void onBreedButtonClicked()
    {
        Breeding.Instance.Breed();
    }
}
