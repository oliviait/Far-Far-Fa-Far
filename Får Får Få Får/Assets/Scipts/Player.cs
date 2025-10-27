using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player Instance;
    public SheepData SheepData;

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
        DontDestroyOnLoad(gameObject);
        Instance = this;

        AddSheep(SheepData);
        AddSheep(SheepData);
        AddSheep(SheepData);
    }
}
