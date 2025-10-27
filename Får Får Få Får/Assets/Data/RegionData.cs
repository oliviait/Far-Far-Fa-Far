using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RegionData", menuName = "Game/RegionData")]
public class RegionData : ScriptableObject
{
    [SerializeField] public List<OpponentFarmData> Farms = new List<OpponentFarmData>();
}
