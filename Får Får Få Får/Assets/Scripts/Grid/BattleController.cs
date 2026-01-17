using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleController : MonoBehaviour
{
    
    public bool playerWon = false;

    public void onBackToFarmButtonClicked()
    {
        SceneManager.LoadScene(1);
    }


    public void SetWin()
    {
        playerWon = true;
    }
}