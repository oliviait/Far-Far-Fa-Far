using UnityEngine;

[CreateAssetMenu(menuName ="Game/Sheep")]
public class SheepData : ScriptableObject
{
    public int maxHP;
    public int DEF;
    public int STR;
    public int SPD;

    public Sprite Sprite;
    public string Name;
}
