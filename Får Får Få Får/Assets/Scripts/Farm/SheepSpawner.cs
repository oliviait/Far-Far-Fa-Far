using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class SheepSpawner : MonoBehaviour
{
    public static SheepSpawner Instance;
    
    public PolygonCollider2D boundsCollider;
    
    public GameObject sheepPrefab;
    public List<SheepSpriteGroup> sheepSpriteGroups;
    public int startingSheep;
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        int count = Player.Instance.InventorySheepList.Count(s => s != null);
        if (Player.Instance.SheepOnFarmList.Count + count == 0)
        {
            // Fresh start
            for (int i = 0; i < startingSheep; i++)
            {
                SpawnNewSheep();
            }
        }
        else
        {
            Player.Instance.SheepOnFarmList.AddRange(Player.Instance.InventorySheepList);
            Player.Instance.InventorySheepList.Clear();
            // Restore farm sheep
            foreach (SheepData data in Player.Instance.SheepOnFarmList)
            {
                SpawnSheep(data);
            }
        }
    }

    public GameObject SpawnSheep(SheepData data)
    {
        GameObject sheep = Instantiate(sheepPrefab);
        
        Genetics genes = sheep.GetComponent<Genetics>();
        genes.GenesA = data.GenesA;
        genes.GenesB = data.GenesB;
        
        Stats stats = sheep.GetComponent<Stats>();
        stats.SetStats(genes);
        stats.Name = data.Name;
        stats.Data = data;

        SpawnSheep(sheep);
        return sheep;
    }

    public void SpawnSheep(GameObject sheepObj)
    {
        if (sheepObj == null) return;
        SheepData data = sheepObj.GetComponent<Stats>().Data;

        // Choose random spritegroup 
        SpriteSwapper swapper = sheepObj.GetComponent<SpriteSwapper>();
        if (swapper.sheepSpriteGroup == null) 
            swapper.sheepSpriteGroup = sheepSpriteGroups[Random.Range(0, sheepSpriteGroups.Count)]; 
        swapper.ChooseSprites();
        
        // Random position on farm
        sheepObj.transform.position = GenerateRandomPointInFence();
        if (!Player.Instance.SheepOnFarmList.Contains(data)) Player.Instance.SheepOnFarmList.Add(data);
    }

    public GameObject SpawnNewSheep()
    {
        GameObject sheep = Instantiate(sheepPrefab);
        sheep.transform.position = GenerateRandomPointInFence();    // Place it randomly into fence
        
        Genetics genes = sheep.GetComponent<Genetics>();
        genes.GenesA = Breeding.Instance.RandomGenes();
        genes.GenesB = Breeding.Instance.RandomGenes();

        Stats stats = sheep.GetComponent<Stats>();
        stats.SetStats(genes);
        stats.Name = Breeding.Instance.randomNames[Random.Range(0, Breeding.Instance.randomNames.Count)];
        stats.Data = CreateData(sheep);

        SpawnSheep(sheep);
        return sheep;
    }

    public SheepData CreateData(GameObject sheep)
    {
        Genetics genes = sheep.GetComponent<Genetics>();
        Stats stats = sheep.GetComponent<Stats>();
        SheepData data = ScriptableObject.CreateInstance<SheepData>();
        data.GenesA = genes.GenesA;
        data.GenesB = genes.GenesB;
        data.maxHP = stats.MaxHp;
        data.STR = stats.Str;
        data.DEF = stats.Def;
        data.SPD = stats.Spd;
        data.Name = stats.Name;
        
        return data;
    }

    public Vector3 GenerateRandomPointInFence()
    {
        Vector3 p = new Vector3(Random.Range(-8f, 8f), Random.Range(-2f, 4f), 0f);
        return boundsCollider.OverlapPoint(p) ? p : boundsCollider.ClosestPoint(p);
    }
}