using UnityEngine;

[CreateAssetMenu(menuName ="Game/Sheep")]
public class SheepData : ScriptableObject
{
    public int[] genesA;
    public int[] genesB;

    public int maxHP;
    public int DEF;
    public int STR;
    public int SPD;

    public Sprite headSprite;
    public Sprite bodySprite;
    public Sprite legsSprite;
    public SheepSpriteGroup spriteGroup;
    public string Name;
}
