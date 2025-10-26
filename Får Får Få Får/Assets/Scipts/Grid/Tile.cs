using UnityEngine;

public class Tile : MonoBehaviour
{

    public Vector2Int GridPos;

    public GameObject Highlight;
    public enum HighlightType { None, Move, Attack }
    public enum TileType { Free, Attackable, MovingFrom, Base}

    public Color BaseColor;
    public Color MovingFromColor;
    public Color HighlightColor;
    public Color AttackHighlightColor;

    private bool isHovered = false;
    private TileType tileType;

    private SpriteRenderer sr;

    private Piece occupant;

    public void SetTileType(TileType tileType)
    {
        this.tileType = tileType;
        ApplyColor();
    }


    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        ApplyColor();
        Highlight.gameObject.SetActive(false);
    }

    public Piece GetOccupant() => occupant;
    public void SetOccupant(Piece piece) => occupant = piece;
    public bool IsOccupied() => occupant != null;

    private void ApplyColor()
    {
        if (sr == null) return;
        /*
        if (isHovered && freeSpace) sr.color = HighlightColor;
        else if (isHovered && attackableSpace) sr.color = AttackHighlightColor;
        else sr.color = attackableSpace ? MovingFromColor : BaseColor;*/

        if (isHovered)
        {
            if (tileType == TileType.Free)
            {
                sr.color = HighlightColor;
            }
            else if (tileType == TileType.Attackable)
            {
                sr.color = AttackHighlightColor;
            }
        }
        else {
            if (tileType == TileType.MovingFrom) sr.color = MovingFromColor;
            else sr.color = BaseColor;

        }

    }

    private void OnMouseOver()
    {
        isHovered = true;
        ApplyColor();
    }

    private void OnMouseExit()
    {
        isHovered = false;
        ApplyColor();
    }

    private void OnMouseDown()
    {
        // forward click to controller only if this tile is part of current movable set or attackable
        BattleController.Instance.OnTileClicked(this);
    }

    public void SetHighlight(bool hasHighlight, HighlightType type)
    {
        if (Highlight == null) return;
        Highlight.SetActive(hasHighlight);
        if (!hasHighlight) return;  // If highlight was set as unactive then thats it

        SpriteRenderer highlightSR = Highlight.GetComponent<SpriteRenderer>();
        if (highlightSR == null) return;
        if (type == HighlightType.Move) highlightSR.color = HighlightColor;
        else if (type == HighlightType.Attack) highlightSR.color = AttackHighlightColor;
        else Highlight.SetActive(false);    // Should be unreachable
    }
}