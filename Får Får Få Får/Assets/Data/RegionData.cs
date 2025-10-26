using UnityEngine;
using static UnityEditor.FilePathAttribute;

[CreateAssetMenu(fileName = "RegionData", menuName = "Scriptable Objects/RegionData")]
public class RegionData : ScriptableObject
{
    public OpponentFarmData[] Farms;

    public static RegionData Orebro()
    {
        var data = ScriptableObject.CreateInstance<RegionData>();
        data.Farms = new OpponentFarmData[5];
        EnemyData[] animals = new EnemyData[5];
        data.Farms[0] = OpponentFarmData.CreateInstance(animals, "Peter", "Sheep", new Vector3(-0.12f, 3.16f, 0));
        data.Farms[1] = OpponentFarmData.CreateInstance(animals, "Annika", "Cow", new Vector3(-1.11f, -0.14f, 0));
        data.Farms[2] = OpponentFarmData.CreateInstance(animals, "Harry", "Pig", new Vector3(0.82f, 1.35f, 0));
        data.Farms[3] = OpponentFarmData.CreateInstance(animals, "Mats", "Cow", new Vector3(-0.07f, -3.42f, 0));
        data.Farms[4] = OpponentFarmData.CreateInstance(animals, "Leila", "Sheep", new Vector3(1.18f, -0.62f, 0));
        return data;
    }
}
