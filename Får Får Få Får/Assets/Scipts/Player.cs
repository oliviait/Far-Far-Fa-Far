using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    // HARD CODED VALUES
    public SheepData Sheep1;
    public SheepData Sheep2;
    public SheepData Sheep3;

    public static Player Instance;

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
    private int CurrentLevel;
    public List<OpponentFarmData> Levels = new List<OpponentFarmData>();
    public OpponentFarmData GetCurrentLevelData() => Levels[CurrentLevel];

    public void NextLevel() => CurrentLevel++;
    public void SetLevel(int NewLevel) => CurrentLevel = NewLevel;


    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Instance = this;

        // HARD CODED VALUES;
        AddSheep(Sheep1);
        AddSheep(Sheep2);
        AddSheep(Sheep3);
    }
}
