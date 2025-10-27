using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Region : MonoBehaviour
{
    public string RegionName;
    public RegionData Data;
    public GameObject OpponentFarmPrefab;
    public Canvas FarmInfoCanvas;
    public TextMeshProUGUI InfoText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        FarmInfoCanvas.gameObject.SetActive(false);
        List<OpponentFarmData> farms = Data.Farms;
        foreach (OpponentFarmData farm in farms)
        {
            GameObject opponentFarm = GameObject.Instantiate(OpponentFarmPrefab);
            opponentFarm.transform.position = farm.Location;
            OpponentFarm opf = opponentFarm.GetComponent<OpponentFarm>();
            opf.data = farm;
            opf.FarmInfoCanvas = FarmInfoCanvas;
            opf.InfoText = InfoText;
        }
    }

    public void onBattleButtonClicked()
    {
        SceneManager.LoadScene(2);
    }
}
