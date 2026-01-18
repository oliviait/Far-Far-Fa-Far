using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool freshStart = true;  // TODO change to false at the end of tutorial
    
    public static Player Instance;
    private string sheepSave;

    // Player's sheep lists
    public List<SheepData> farmSheepList = new();
    public List<SheepData> inventorySheepList = new();
    
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
        string sheep = JsonUtility.ToJson(farmSheepList);
        System.IO.File.WriteAllText(sheepSave, sheep);
    }

    public void Load()
    {
        if (System.IO.File.Exists(sheepSave)) farmSheepList = JsonUtility.FromJson<List<SheepData>>(System.IO.File.ReadAllText(sheepSave));
    }

    public void NewGame()
    {
        farmSheepList = new List<SheepData>();
    }
}
