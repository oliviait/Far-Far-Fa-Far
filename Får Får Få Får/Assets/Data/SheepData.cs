using UnityEngine;

[CreateAssetMenu(menuName ="Game/Sheep")]
public class SheepData : ScriptableObject
{
    public int[] GenesA;
    public int[] GenesB;

    public int maxHP;
    public int DEF;
    public int STR;
    public int SPD;

    public Sprite HeadSprite;
    public Sprite BodySprite;
    public Sprite LegsSprite;
    public string Name;
}
