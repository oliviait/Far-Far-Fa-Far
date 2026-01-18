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
        if (Player.Instance.freshStart)
        {
            Debug.Log("FRESH START");
            for (int i = 0; i < startingSheep; i++)
                SpawnNewSheep(i);
        }
        else
        {
            // Spawn all sheep onto farm
            foreach (SheepData data in Player.Instance.farmSheepList)
                SpawnSheep(data);
            foreach (SheepData data in Player.Instance.inventorySheepList)
                SpawnSheep(data);

            // Move sheep that are supposed to be in inventory to slots
            foreach (var stats in FindObjectsByType<Stats>(FindObjectsSortMode.None))
            {
                GameObject sheep = stats.gameObject;
                if (Player.Instance.inventorySheepList.Contains(stats.Data)) // If sheep should be in inv slot
                    InventoryManager.Instance.PlaceSheepInSlot(sheep,
                        Player.Instance.inventorySheepList.IndexOf(stats.Data)); // move it there
            }
        }
    }

    public GameObject SpawnSheep(SheepData data)
    {
        GameObject sheep = Instantiate(sheepPrefab);

        Genetics genes = sheep.GetComponent<Genetics>();
        genes.GenesA = data.genesA;
        genes.GenesB = data.genesB;

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

        // Choose spritegroup 
        SpriteSwapper swapper = sheepObj.GetComponent<SpriteSwapper>();
        if (data.spriteGroup != null) swapper.sheepSpriteGroup = data.spriteGroup;
        else if (swapper.sheepSpriteGroup == null)
        {
            swapper.sheepSpriteGroup = sheepSpriteGroups[Random.Range(0, sheepSpriteGroups.Count)];
            data.spriteGroup = swapper.sheepSpriteGroup;
        }
        swapper.ChooseSprites();

        // Random position on farm
        sheepObj.transform.position = GenerateRandomPointInFence();

        // Add to farm list if not there already
        if (!Player.Instance.farmSheepList.Contains(data)) Player.Instance.farmSheepList.Add(data);
    }

    public GameObject SpawnNewSheep()
    {
        GameObject sheep = Instantiate(sheepPrefab);
        sheep.transform.position = GenerateRandomPointInFence(); // Place it randomly into fence

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
    
    public GameObject SpawnNewSheep(int i)
    {
        GameObject sheep = Instantiate(sheepPrefab);
        sheep.transform.position = GenerateRandomPointInFence(); // Place it randomly into fence

        Genetics genes = sheep.GetComponent<Genetics>();
        genes.GenesA = Breeding.Instance.RandomGenes();
        genes.GenesB = Breeding.Instance.RandomGenes();

        Stats stats = sheep.GetComponent<Stats>();
        stats.SetStats(genes);
        stats.Name = Breeding.Instance.randomNames[Random.Range(0, Breeding.Instance.randomNames.Count)];
        stats.Data = CreateData(sheep);

        SpriteSwapper swapper = sheep.GetComponent<SpriteSwapper>();
        swapper.sheepSpriteGroup = sheepSpriteGroups[i];
        
        SpawnSheep(sheep);
        return sheep;
    }

    public SheepData CreateData(GameObject sheep)
    {
        Genetics genes = sheep.GetComponent<Genetics>();
        Stats stats = sheep.GetComponent<Stats>();
        SheepData data = ScriptableObject.CreateInstance<SheepData>();
        data.genesA = genes.GenesA;
        data.genesB = genes.GenesB;
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
        return boundsCollider.OverlapPoint(p) ? p : GenerateRandomPointInFence();
    }
}