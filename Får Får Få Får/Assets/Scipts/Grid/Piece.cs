using UnityEngine;

public class Piece : MonoBehaviour
{
    public enum Team { Player, Opponent }
    public Team Owner;

    public GameObject Parent;

    // Data
    public float powerConstant = 1;
    public SpriteRenderer sr;

    // Movement range
    public int Range = 1;
    public int GetRange() => Range;

    private int speed;
    private int strength;
    private int maxHP;
    private int defence;
    private Sprite sprite;

    private float NextMoveTime;

    private int hp;

    public int GetSpeed() => speed;
    public Team GetOwner() => Owner;
    public void SetOwner(Team owner) => Owner = owner;

    private void Start()
    {
        hp = maxHP;
        NextMoveTime = 1f / speed;
    }

    public float GetNextMoveTime()
    {
        return NextMoveTime;
    }

    public void IncrementNextMoveTime()
    {
        NextMoveTime += 1f / speed;
    }

    public void SetData(SheepData data)
    {
        speed = data.SPD;
        strength = data.STR;
        maxHP = data.maxHP;
        defence = data.DEF;

        sr.sprite = data.Sprite;
        sprite = data.Sprite;
        Owner = Team.Player;
    }

    public void SetData(EnemyData data)
    {
        speed = data.SPD;
        strength = data.STR;
        maxHP = data.maxHP;
        defence = data.DEF;

        sr.sprite = data.Sprite;
        sprite = data.Sprite;
        Owner = Team.Opponent;
    }

    public void TakeDamage(int dmg)
    {
        hp -= Mathf.Max(0, dmg);
        Debug.Log("Remaining HP: " + hp);
        if (hp <= 0) Die();
    }
    private void Die()
    {
        // caller should clear tile occupant.
        Destroy(gameObject);
    }

    public void Attack(Piece target)
    {
        Debug.Log("HERE");
        int damage = (int) (powerConstant * (float) strength / (float) defence);
        Debug.Log("DAMAGE: " + damage);
        target.TakeDamage(damage);
    }

    private void OnDestroy()
    {
        sr.sprite = null;
    }
}
