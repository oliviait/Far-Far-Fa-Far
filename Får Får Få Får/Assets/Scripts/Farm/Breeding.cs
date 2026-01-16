using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Breeding : MonoBehaviour
{
    public static Breeding Instance;

    public GameObject SheepPrefab;

    public int StartingSheep;
    public int GenesNum = 4;
    public int GenePercentage = 25;

    public List<string> RandomNames = new List<string>();

    public int NumSelected;
    public GameObject FirstParent;
    public GameObject SecondParent;
    
    private PolygonCollider2D boundsCollider;

    void Start()
    {
        Instance = this;
        NumSelected = 0;
        FirstParent = null;
        SecondParent = null;
        
        var fence = GameObject.Find("FarmFence");
        if (fence != null)
            boundsCollider = fence.GetComponentInChildren<PolygonCollider2D>();
        
        // If no sheep yet, start of the game
        int count = Player.Instance.InventorySheepList.Count(s => s != null);
        if (Player.Instance.SheepOnFarmList.Count + count == 0)
        {
            for (int i = 0; i < StartingSheep; i++)
            {
                GameObject sheep = Instantiate(SheepPrefab);
                sheep.transform.position = generateRandomPointInFence();
                sheep.GetComponent<Genetics>().GenesA = RandomGenes();
                sheep.GetComponent<Genetics>().GenesB = RandomGenes();
                sheep.GetComponent<Stats>().SetStats(sheep.GetComponent<Genetics>());
                sheep.GetComponent<Stats>().Name = RandomNames[Random.Range(0, RandomNames.Count)];
                sheep.GetComponent<Stats>().Data = CreateData(sheep);
                sheep.GetComponent<SheepDragger>().inSlot = false;
                Player.Instance.SheepOnFarmList.Add(sheep.GetComponent<Stats>().Data);  // Add to farm
            }
        }
        else  // If not start of game, player already has sheep
        {
            foreach (SheepData data in Player.Instance.SheepOnFarmList)
            {
                GameObject sheep = Instantiate(SheepPrefab);
                sheep.transform.position = generateRandomPointInFence();
                sheep.GetComponent<Genetics>().GenesA = data.GenesA;
                sheep.GetComponent<Genetics>().GenesB = data.GenesB;
                sheep.GetComponent<Stats>().SetStats(sheep.GetComponent<Genetics>());
                sheep.GetComponent<Stats>().Name = data.Name;
                sheep.GetComponent<Stats>().Data = data;
                sheep.GetComponent<SheepDragger>().inSlot = false;
            }
            int counter = 0;
            foreach (SheepData data in Player.Instance.InventorySheepList)
            {
                GameObject sheep = Instantiate(SheepPrefab);
                sheep.GetComponent<Genetics>().GenesA = data.GenesA;
                sheep.GetComponent<Genetics>().GenesB = data.GenesB;
                sheep.GetComponent<Stats>().SetStats(sheep.GetComponent<Genetics>());
                sheep.GetComponent<Stats>().Name = data.Name;
                sheep.GetComponent<Stats>().Data = data;
                InventorySlot slot = InventoryManager.Instance.slots[counter];
                slot.currentItem = sheep;
                sheep.GetComponent<SheepDragger>().inSlot = true;
                sheep.GetComponent<SheepDragger>().currentInventorySlot = slot;
                Vector3 pos = new(-3.75f, -3.5f, 0);
                sheep.transform.position = pos + new Vector3(counter * 1.875f, 0, 0);
                counter++;
            }
        }
    }

    private Vector3 generateRandomPointInFence()
    {
        Vector3 p = new Vector3(Random.Range(-8f, 8f), Random.Range(-2f, 4f), 0f);
        return boundsCollider.OverlapPoint(p) ? p : boundsCollider.ClosestPoint(p);
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
        data.HeadSprite = sheep.transform.GetChild(0).GetComponent<SpriteRenderer>().sprite;
        data.LegsSprite = sheep.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite;
        data.BodySprite = sheep.GetComponent<SpriteRenderer>().sprite;
        return data;
    }

    public void Increase(GameObject parent)
    {
        NumSelected++;
        if (FirstParent == null)
        {
            FirstParent = parent;
        }
        else
        {
            SecondParent = parent;
        }
    }

    public void Decrease(GameObject parent)
    {
        NumSelected--;
        if (FirstParent == parent)
        {
            FirstParent = null;
        }
        else
        {
            SecondParent = null;
        }
    }

    public void Breed()
    {
        if (NumSelected == 2)
        {
            GameObject child = Instantiate(SheepPrefab);
            Genetics firstParentGenes = FirstParent.GetComponent<Genetics>();
            Genetics secondParentGenes = SecondParent.GetComponent<Genetics>();
            Genetics childGenes = child.GetComponent<Genetics>();
            childGenes.GenesA = new int[GenesNum];
            childGenes.GenesB = new int[GenesNum];
            for (int i = 0; i < GenesNum; i++)
            {
                childGenes.GenesA[i] = SetGene(firstParentGenes.GenesA[i], firstParentGenes.GenesB[i]);
                childGenes.GenesB[i] = SetGene(secondParentGenes.GenesA[i], secondParentGenes.GenesB[i]);
            }
            child.GetComponent<Stats>().SetStats(childGenes);
            child.GetComponent<Stats>().Name = RandomNames[Random.Range(0, RandomNames.Count)];
            child.transform.position = generateRandomPointInFence();
            child.GetComponent<Stats>().Data = CreateData(child);
            Player.Instance.SheepOnFarmList.Add(child.GetComponent<Stats>().Data);
            FirstParent.GetComponent<Selectable>().Deselect();
            SecondParent.GetComponent<Selectable>().Deselect();
        }
    }

    public int SetGene(int GeneA, int GeneB)
    {
        int childGene = 0;
        for (int i = 1; i <= 1<<15; i *= 2)
        {
            int r = Random.Range(-15, 16);
            if (r < 0)
            {
                childGene += i & GeneA;
            }
            else if (r > 0)
            {
                childGene += i & GeneB;
            }
            else
            {
                childGene += i;
            }
        }
        return childGene;
    }

    public int[] RandomGenes()
    {
        int[] genes = new int[GenesNum];
        for (int i = 0; i < GenesNum; i++)
        {
            for (int j = 1; j < 1<<15; j *= 2)
            {
                int add = Random.Range(0, 100);
                if (add < GenePercentage)
                {
                    genes[i] += j; 
                }
            }
            
        }
        return genes;
    }
}
