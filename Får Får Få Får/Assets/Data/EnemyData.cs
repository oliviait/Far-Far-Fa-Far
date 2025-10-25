using UnityEngine;

[CreateAssetMenu(menuName ="Game/Enemy")]
public class EnemyData : ScriptableObject
{
    public int maxHP;
    public int DEF;
    public int STR;
    public int SPD;

    public Sprite Sprite;
    public string Name;
}
