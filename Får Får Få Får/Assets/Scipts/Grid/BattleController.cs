using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleController : MonoBehaviour
{
    public void onBackToFarmButtonClicked()
    {
        SceneManager.LoadScene(0);
    }
}