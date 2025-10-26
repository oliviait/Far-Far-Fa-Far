using UnityEngine;

[CreateAssetMenu(fileName = "OpponentFarmData", menuName = "Scriptable Objects/OpponentFarmData")]
public class OpponentFarmData : ScriptableObject
{
    public EnemyData[] Animals;
    public string FarmerName;
    public string Species;
    public Vector3 Location;
    public bool Defeated;

    public static OpponentFarmData CreateInstance(EnemyData[] animals, string farmer, string species, Vector3 location)
    {
        var data = ScriptableObject.CreateInstance<OpponentFarmData>();
        data.Animals = animals;
        data.FarmerName = farmer;
        data.Species = species;
        data.Location = location;
        data.Defeated = false;
        return data;
    }
}
