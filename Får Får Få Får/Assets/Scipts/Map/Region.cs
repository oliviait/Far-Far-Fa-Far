using UnityEngine;

public class Region : MonoBehaviour
{
    public string RegionName;
    public RegionData Data;
    public GameObject OpponentFarmPrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (RegionName == "Örebro")
        {
            Data = RegionData.Orebro();
        }
        OpponentFarmData[] farms = Data.Farms;
        foreach (OpponentFarmData farm in farms)
        {
            GameObject opponentFarm = GameObject.Instantiate(OpponentFarmPrefab);
            opponentFarm.transform.position = farm.Location;
            opponentFarm.GetComponent<OpponentFarm>().Animals = farm.Animals;
            opponentFarm.GetComponent<OpponentFarm>().FarmerName = farm.FarmerName;
            opponentFarm.GetComponent<OpponentFarm>().Species = farm.Species;
            opponentFarm.GetComponent<OpponentFarm>().Defeated = farm.Defeated;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
