using System.Collections.Generic;
using UnityEngine;

public class DefeatProgress : MonoBehaviour
{
    public static DefeatProgress Instance;

    private readonly HashSet<int> defeatedFarmIds = new HashSet<int>();

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool IsDefeated(int farmId)
    {
        return defeatedFarmIds.Contains(farmId);
    }

    public void MarkDefeated(int farmId)
    {
        if (farmId < 0) return;
        defeatedFarmIds.Add(farmId);
    }

    public void ResetAll()
    {
        defeatedFarmIds.Clear();
    }
}
