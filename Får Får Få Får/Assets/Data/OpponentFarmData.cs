using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OpponentFarmData", menuName = "Game/OpponentFarmData")]
public class OpponentFarmData : ScriptableObject
{
    // Farm
    public string FarmerName;
    public string Species;
    public Vector3 Location;
    public bool Defeated;

    // Battle
    public int BoardSizeX;
    public int BoardSizeY;
    [SerializeField] public List<EnemyData> Animals;
    [SerializeField] public List<Vector2Int> EnemySpawnLocations;
    [SerializeField] public List<Vector2Int> PlayerSpawnLocations;

    // We make data in Assets/Data dir :)
    /*
    public static OpponentFarmData CreateInstance(
        List<EnemyData> animals, 
        string farmer, 
        string species, 
        Vector3 location, 
        int BoardSizeX, 
        int BoardSizeY, 
        List<Vector2Int> EnemySpawnLocations, 
        List<Vector2Int> PlayerSpawnLocations
    ){
        var data = ScriptableObject.CreateInstance<OpponentFarmData>();
        data.Animals = animals;
        data.FarmerName = farmer;
        data.Species = species;
        data.Location = location;
        data.BoardSizeX = BoardSizeX;
        data.BoardSizeY = BoardSizeY;
        data.EnemySpawnLocations = EnemySpawnLocations;
        data.PlayerSpawnLocations = PlayerSpawnLocations;
        data.Defeated = false;
        return data;
    }
    */
}
