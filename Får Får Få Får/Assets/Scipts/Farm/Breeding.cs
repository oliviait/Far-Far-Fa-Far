using System;
using TMPro.EditorUtilities;
using Unity.VisualScripting;
using UnityEngine;

public class Breeding : MonoBehaviour
{
    public static Breeding Instance;

    public GameObject SheepPrefab;

    public int StartingSheep;
    public int GenesNum = 4;

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
        for (int i = 0; i < StartingSheep; i++)
        {
            GameObject sheep = GameObject.Instantiate(SheepPrefab);
            sheep.transform.position = new Vector3(UnityEngine.Random.Range(-8f, 8f), UnityEngine.Random.Range(-4f, 4f), 0f);
            sheep.GetComponent<Genetics>().GenesA = RandomGenes();
            sheep.GetComponent<Genetics>().GenesB = RandomGenes();
            sheep.GetComponent<Stats>().SetStats(sheep.GetComponent<Genetics>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && NumSelected == 2)
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
            child.transform.position = new Vector3(UnityEngine.Random.Range(-8f, 8f), UnityEngine.Random.Range(-4f, 4f), 0f);
            FirstParent.GetComponent<Selectable>().Deselect();
            SecondParent.GetComponent<Selectable>().Deselect();
        }
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
                int add = UnityEngine.Random.Range(0, 4);
                if (add == 0)
                {
                    genes[i] += j; 
                }
            }
            
        }
        return genes;
    }
}
