using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OpponentFarm : MonoBehaviour
{
    public OpponentFarmData data;
    public Canvas FarmInfoCanvas;
    public TextMeshProUGUI InfoText;

    private void Start()
    {
        transform.position = data.Location;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        { 
            FarmInfoCanvas.gameObject.SetActive(false);
        }
    }
    void OnMouseDown()
    {
        if (!data.Defeated)
        {
            FarmInfoCanvas.gameObject.SetActive(true);
            InfoText.text = ToString();
            Player.Instance.enteringLevel = data;
        }
    }

    public override string ToString()
    {
        return data.FarmerName + "\n" + data.Species + " farmer";
    }
}
