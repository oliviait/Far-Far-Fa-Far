using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2Int GridPos;

    public GameObject Highlight;
    public enum HighlightType { None, Free, Attack }
    public enum TileType { Free, Attackable, MovingFrom, Default}

    public Color BaseColor;
    public Color MovingFromColor;
    public Color HighlightColor;
    public Color AttackHighlightColor;

    private bool isHovered = false;
    private TileType tileType;

    private SpriteRenderer sr;

    private Piece occupant;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        ApplyColor();
        Highlight.gameObject.SetActive(false);
    }

    public Piece GetOccupant() => occupant;
    public void SetOccupant(Piece piece) => occupant = piece;
    public bool IsOccupied() => occupant != null;
    public void SetTileType(TileType tileType)
    {
        this.tileType = tileType;
        ApplyColor();
    }
    public Vector2Int getGridPos() => GridPos;

    private void ApplyColor()
    {
        if (sr == null) return;

        if (isHovered)
        {
            if (tileType == TileType.Free) sr.color = HighlightColor;
            else if (tileType == TileType.Attackable) sr.color = AttackHighlightColor;
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
        TurnController.Instance.OnTileClicked(this);
    }

    public void SetHighlight(bool hasHighlight, HighlightType type)
    {
        if (Highlight == null) return;
        Highlight.SetActive(hasHighlight);
        if (!hasHighlight) return;  // If highlight was set as unactive then thats it

        SpriteRenderer highlightSR = Highlight.GetComponent<SpriteRenderer>();
        if (highlightSR == null) return;
        if (type == HighlightType.Free) highlightSR.color = HighlightColor;
        else if (type == HighlightType.Attack) highlightSR.color = AttackHighlightColor;
        else Highlight.SetActive(false);    // Should be unreachable
    }
}