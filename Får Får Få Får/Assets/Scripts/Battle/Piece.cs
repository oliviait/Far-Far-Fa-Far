using UnityEngine;
using UnityEngine.UI;

public class Piece : MonoBehaviour
{
    public static int NumberOfEnemyPieces = 0;
    public static int NumberOfPlayerPieces = 0;
    public enum Team { Player, Opponent }
    public Team Owner;

    // Data
    public float powerConstant = 5; // Attack power constant used for damage calculations
    public int Range = 1;   // Movement range

    public GameObject Head;
    public GameObject Body;
    public GameObject Legs;


    public int speed;
    public int strength;
    public int maxHP;
    public int defence;
    public int hp;
    public Image HPBarGreen;

    public AudioClipGroup DamageSound;
    public AudioClipGroup DieSound;

    public Tile TilePlacedOn;   // Tile that the piece is placed on

    private float NextMoveTime; // Used to determine, when is this piece's turn

    public int GetRange() => Range;
    public int GetSpeed() => speed;
    public Team GetOwner() => Owner;
    public void SetOwner(Team owner) => Owner = owner;
    public Tile GetTilePlacedOn() => TilePlacedOn;
    public void SetTilePlacedOn(Tile tile) => TilePlacedOn = tile;


    private void Start()
    {
        hp = maxHP;
        HPBarGreen.fillAmount = (float) hp / maxHP;
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

        Head.GetComponent<SpriteRenderer>().sprite = data.headSprite;
        Body.GetComponent<SpriteRenderer>().sprite = data.bodySprite;
        Legs.GetComponent<SpriteRenderer>().sprite = data.legsSprite;
        Owner = Team.Player;
    }

    public void SetData(EnemyData data)
    {
        speed = data.SPD;
        strength = data.STR;
        maxHP = data.maxHP;
        defence = data.DEF;

        Head.GetComponent<SpriteRenderer>().sprite = data.HeadSprite;
        Body.GetComponent<SpriteRenderer>().sprite = data.BodySprite;
        Legs.GetComponent<SpriteRenderer>().sprite = data.LegsSprite;
        Owner = Team.Opponent;
    }

    public void TakeDamage(int dmg)
    {
        DamageSound.Play();
        hp -= Mathf.Max(0, dmg);
        if (hp < 0) hp = 0;
        HPBarGreen.fillAmount = (float) hp / maxHP;
        if (hp == 0) Die();
    }
    private void Die()
    {
        DieSound.Play();

        if (Owner == Team.Player) NumberOfPlayerPieces--;
        else NumberOfEnemyPieces--;

        TilePlacedOn.SetOccupant(null);

        // Check win/lose BEFORE destroying the piece
        if (NumberOfPlayerPieces == 0)
            Events.BattleLost();
        else if (NumberOfEnemyPieces == 0)
            Events.BattleWon();

        Destroy(gameObject);
    }


    public void Attack(Piece target)
    {
        int damage = (int) (powerConstant * strength / target.defence * 0.5);
        target.TakeDamage(Mathf.Max(damage, 1));
    }
}