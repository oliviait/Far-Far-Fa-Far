using UnityEngine;
using UnityEngine.Analytics;

public class Stats : MonoBehaviour
{
    public int BaseHp = 8;
    public int BaseStr = 4;
    public int BaseDef = 2;
    public int BaseSpd = 2;

    public float MultHp = 1.5f;
    public float MultStr = 1f;
    public float MultDef = 1f;
    public float MultSpd = 0.5f;

    public int MaxHp;
    public int Hp;
    public int Str;
    public int Def;
    public int Spd;

    public void SetStats(Genetics genes)
    {
        MaxHp = BaseHp + (int)(GetStat(genes.GenesA[0], genes.GenesB[0]) * MultHp);
        Str = BaseStr + (int)(GetStat(genes.GenesA[1], genes.GenesB[1]) * MultStr);
        Def = BaseDef + (int)(GetStat(genes.GenesA[2], genes.GenesB[2]) * MultDef);
        Spd = BaseSpd + (int)(GetStat(genes.GenesA[3], genes.GenesB[3]) * MultSpd);
        Hp = MaxHp;
    }

    private int GetStat(int geneA, int geneB)
    {
        int stat = 0;
        for (int i = 1; i <= 1 << 15; i *= 2)
        {
            if (((geneA | geneB) & i) > 0)
            {
                stat++;
            }
        }
        return stat;
    }

    public override string ToString()
    {
        return "HP: " + MaxHp + "\nSTR: " + Str + "\nDEF: " + Def + "\nSPD: " + Spd;
    }
}
