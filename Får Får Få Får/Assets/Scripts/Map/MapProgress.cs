using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Persists map progression across scene loads:
/// - Which stage (map sprite) you're on
/// - Which farm was selected for battle
/// - Which farms have been defeated
/// </summary>
public class MapProgress : MonoBehaviour
{
    public static MapProgress Instance;

    [Header("Progress")]
    [Tooltip("Which map stage you're currently on (0,1,2...).")]
    public int stageIndex = 0;

    [Tooltip("The farm ID currently selected for battle. -1 means none selected.")]
    public int currentFarmId = -1;

    // Tracks defeated farms by their unique int Id
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

    /// <summary>Returns true if this farm has been defeated.</summary>
    public bool IsDefeated(int farmId)
    {
        return defeatedFarmIds.Contains(farmId);
    }

    /// <summary>Marks a farm as defeated.</summary>
    public void MarkDefeated(int farmId)
    {
        if (farmId < 0) return;
        defeatedFarmIds.Add(farmId);
    }

    /// <summary>Clears the selected farm (use after battle ends if you want).</summary>
    public void ClearCurrentFarm()
    {
        currentFarmId = -1;
    }

    /// <summary>Resets all progress (useful for "New Game").</summary>
    public void ResetAll()
    {
        stageIndex = 0;
        currentFarmId = -1;
        defeatedFarmIds.Clear();
    }

    /// <summary>Optional: returns a copy if you ever want to debug / display defeated IDs.</summary>
    public List<int> GetDefeatedFarmIds()
    {
        return new List<int>(defeatedFarmIds);
    }
}