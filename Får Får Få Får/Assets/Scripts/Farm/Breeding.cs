using System.Collections.Generic;
using UnityEngine;

public class Breeding : MonoBehaviour
{
    public static Breeding Instance;
    public GameObject sheepPrefab;
    
    public int genesNum = 4;
    public int genePercentage = 25;

    public List<string> randomNames = new();

    public int numSelected;
    public GameObject firstParent;
    public GameObject secondParent;
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    
    void Start()
    {
        Instance = this;
        numSelected = 0;
        firstParent = null;
        secondParent = null;
    }
    
    public void Increase(GameObject parent)
    {
        numSelected++;
        if (firstParent == null)
        {
            firstParent = parent;
        }
        else
        {
            secondParent = parent;
        }
    }

    public void Decrease(GameObject parent)
    {
        numSelected--;
        if (firstParent == parent)
        {
            firstParent = null;
        }
        else
        {
            secondParent = null;
        }
    }

    public void Breed()
    {
        if (numSelected == 2)
        {
            GameObject child = Instantiate(sheepPrefab);
            child.transform.position = SheepSpawner.Instance.GenerateRandomPointInFence();
            
            SpriteSwapper swapper = child.GetComponent<SpriteSwapper>();
            if (Random.value < 0.5f)
                swapper.sheepSpriteGroup = firstParent.GetComponent<SpriteSwapper>().sheepSpriteGroup;
            else swapper.sheepSpriteGroup = secondParent.GetComponent<SpriteSwapper>().sheepSpriteGroup;
            
            Genetics firstParentGenes = firstParent.GetComponent<Genetics>();
            Genetics secondParentGenes = secondParent.GetComponent<Genetics>();
            Genetics childGenes = child.GetComponent<Genetics>();
            childGenes.GenesA = new int[genesNum];
            childGenes.GenesB = new int[genesNum];
            for (int i = 0; i < genesNum; i++)
            {
                childGenes.GenesA[i] = SetGene(firstParentGenes.GenesA[i], firstParentGenes.GenesB[i]);
                childGenes.GenesB[i] = SetGene(secondParentGenes.GenesA[i], secondParentGenes.GenesB[i]);
            }

            Stats stats = child.GetComponent<Stats>();
            stats.SetStats(childGenes);
            stats.Name = randomNames[Random.Range(0, randomNames.Count)];
            stats.Data = SheepSpawner.Instance.CreateData(child);

            Player.Instance.farmSheepList.Add(child.GetComponent<Stats>().Data);
            
            Selectable firstParentSelectable = firstParent.GetComponent<Selectable>();
            firstParentSelectable.Deselect();
            firstParentSelectable.isSelectable = false;
            
            Selectable secondParentSelectable = secondParent.GetComponent<Selectable>();
            secondParentSelectable.Deselect();
            secondParentSelectable.isSelectable = false;
            
            SheepSpawner.Instance.SpawnSheep(child);
        }
    }

    public int SetGene(int GeneA, int GeneB)
    {
        int childGene = 0;
        for (int i = 1; i <= 1<<15; i *= 2)
        {
            int r = Random.Range(-3, 4);
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
        int[] genes = new int[genesNum];
        for (int i = 0; i < genesNum; i++)
        {
            for (int j = 1; j < 1<<15; j *= 2)
            {
                int add = Random.Range(0, 100);
                if (add < genePercentage)
                {
                    genes[i] += j; 
                }
            }
            
        }
        return genes;
    }
}
