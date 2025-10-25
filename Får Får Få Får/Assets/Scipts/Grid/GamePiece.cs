using UnityEngine;

public class Piece : MonoBehaviour
{
    // Data
    private int speed;
    private int strength;
    private int maxHP;
    private int defence;
    private Sprite sprite;

    private int hp;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        hp = maxHP;
    }

    public void SetData(SheepData data)
    {
        speed = data.SPD;
        strength = data.STR;
        maxHP = data.maxHP;
        defence = data.DEF;

        sr.sprite = data.Sprite;
        sprite = data.Sprite;
    }

    public void SetData(EnemyData data)
    {
        speed = data.SPD;
        strength = data.STR;
        maxHP = data.maxHP;
        defence = data.DEF;

        sr.sprite = data.Sprite;
        sprite = data.Sprite;
    }
}
