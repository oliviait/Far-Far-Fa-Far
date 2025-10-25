using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Game/Battle")]
public class BattleData : ScriptableObject
{
    public int BoardSizeX;
    public int BoardSizeY;

    public List<EnemyData> Enemies;
    public List<Vector2Int> EnemySpawnLocations;

    public List<Vector2Int> PlayerSpawnLocations;
}
