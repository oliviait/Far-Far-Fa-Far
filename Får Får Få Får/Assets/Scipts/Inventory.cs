using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    // Used for transfering inventory between scenes
    public static List<SheepData> Sheep;

    // Add or Insert a sheep to the inventory
    public static void AddSheep(SheepData data) => Sheep.Add(data);
    public static void InsertSheep(SheepData data, int i)
    {
        if (i >= Sheep.Count) Sheep.Add(data);
        else Sheep.Insert(i, data);
    }

    // Methods for removing sheep based on data or index, whichever is more convenient
    public static void RemoveSheep(SheepData data) => Sheep.Remove(data);
    public static void RemoveSheep(int i) => Sheep.RemoveAt(i);



    // Game progress
    private static int CurrentLevel;
    public static List<BattleData> Levels;
    public static BattleData GetCurrentLevelData() => Levels[CurrentLevel];

    public static int NextLevel() => CurrentLevel++;

}
