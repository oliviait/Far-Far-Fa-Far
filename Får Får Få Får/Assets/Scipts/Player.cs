using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public SheepData SheepData;
    private string sheepSave;

    // Player's sheep and corresponding methods
    public List<SheepData> Sheep = new List<SheepData>();

    public void AddSheep(SheepData data) => Sheep.Add(data);
    public void InsertSheep(SheepData data, int i)
    {
        if (i >= Sheep.Count) Sheep.Add(data);
        else Sheep.Insert(i, data);
    }
    public void RemoveSheep(SheepData data) => Sheep.Remove(data);
    public void RemoveSheep(int i) => Sheep.RemoveAt(i);



    // Game progress
    public OpponentFarmData enteringLevel;

    private void Awake()
    {
        sheepSave = Application.persistentDataPath + "/Sheep.json";
        DontDestroyOnLoad(gameObject);
        if (Instance == null) {
		    Instance = this;
	    }
        else {
            DestroyObject(gameObject);
	    }
    }

    public void Save()
    {
        string sheep = JsonUtility.ToJson(Sheep);
        System.IO.File.WriteAllText(sheepSave, sheep);
    }

    public void Load()
    {
        if (System.IO.File.Exists(sheepSave)) Sheep = JsonUtility.FromJson<List<SheepData>>(System.IO.File.ReadAllText(sheepSave));
    }

    public void NewGame()
    {
        Sheep = new List<SheepData>();
    }
}
