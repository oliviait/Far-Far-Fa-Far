using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "OpponentFarmData", menuName = "Game/OpponentFarmData")]
public class OpponentFarmData : ScriptableObject
{
    // Farm
    public int FarmID;
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
}
