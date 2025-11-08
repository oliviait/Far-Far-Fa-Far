using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleController : MonoBehaviour
{
    public void onBackToFarmButtonClicked()
    {
        SceneManager.LoadScene(0);
    }
}