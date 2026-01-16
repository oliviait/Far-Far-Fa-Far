using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public SheepData SheepData;
    private string sheepSave;

    // Player's sheep and corresponding methods
    public List<SheepData> SheepOnFarmList = new();
    public List<SheepData> InventorySheepList = new();
    // Game progress
    public OpponentFarmData enteringLevel;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        sheepSave = Application.persistentDataPath + "/Sheep.json";
        DontDestroyOnLoad(gameObject);
    }

    public void Save()
    {
        string sheep = JsonUtility.ToJson(SheepOnFarmList);
        System.IO.File.WriteAllText(sheepSave, sheep);
    }

    public void Load()
    {
        if (System.IO.File.Exists(sheepSave)) SheepOnFarmList = JsonUtility.FromJson<List<SheepData>>(System.IO.File.ReadAllText(sheepSave));
    }

    public void NewGame()
    {
        SheepOnFarmList = new List<SheepData>();
    }
}
