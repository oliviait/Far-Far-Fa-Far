using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Unity.VisualScripting;
using UnityEngine;

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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Instance = this;
        NumSelected = 0;
        FirstParent = null;
        SecondParent = null;
        if (Player.Instance.Sheep.Count == 0)
        {
            for (int i = 0; i < StartingSheep; i++)
            {
                GameObject sheep = GameObject.Instantiate(SheepPrefab);
                sheep.transform.position = new Vector3(UnityEngine.Random.Range(-8f, 8f), UnityEngine.Random.Range(-2f, 4f), 0f);
                sheep.GetComponent<Genetics>().GenesA = RandomGenes();
                sheep.GetComponent<Genetics>().GenesB = RandomGenes();
                sheep.GetComponent<Stats>().SetStats(sheep.GetComponent<Genetics>());
                sheep.GetComponent<Stats>().Name = RandomNames[UnityEngine.Random.Range(0, RandomNames.Count)];
                Player.Instance.AddSheep(CreateData(sheep));
            }
        }
        else
        {
            foreach (SheepData data in Player.Instance.Sheep)
            {
                GameObject sheep = GameObject.Instantiate(SheepPrefab);
                sheep.transform.position = new Vector3(UnityEngine.Random.Range(-8f, 8f), UnityEngine.Random.Range(-2f, 4f), 0f);
                sheep.GetComponent<Genetics>().GenesA = data.GenesA;
                sheep.GetComponent<Genetics>().GenesB = data.GenesB;
                sheep.GetComponent<Stats>().SetStats(sheep.GetComponent<Genetics>());
                sheep.GetComponent<Stats>().Name = data.Name;
            }
        }
    }

    private SheepData CreateData(GameObject sheep)
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
            GameObject child = GameObject.Instantiate(SheepPrefab);
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
            child.GetComponent<Stats>().Name = RandomNames[UnityEngine.Random.Range(0, RandomNames.Count)];
            child.transform.position = new Vector3(UnityEngine.Random.Range(-8f, 8f), UnityEngine.Random.Range(-2f, 4f), 0f);
            Player.Instance.AddSheep(CreateData(child));
            FirstParent.GetComponent<Selectable>().Deselect();
            SecondParent.GetComponent<Selectable>().Deselect();
        }
    }

    public int SetGene(int GeneA, int GeneB)
    {
        int childGene = 0;
        for (int i = 1; i <= 1<<15; i *= 2)
        {
            int r = UnityEngine.Random.Range(-15, 16);
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
                int add = UnityEngine.Random.Range(0, 100);
                if (add < GenePercentage)
                {
                    genes[i] += j; 
                }
            }
            
        }
        return genes;
    }
}
