using UnityEngine;

public class Tile : MonoBehaviour
{

    public Vector2Int GridPos;

    public GameObject Highlight;
    public enum HighlightType { None, Move, Attack }

    public Color UnactiveColor;
    public Color ActiveColor;
    public Color HighlightColor;
    public Color AttackHighlightColor;


    private bool active = false;
    private bool isHovered = false;
    private bool movableSpace = false;

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

    private void ApplyColor()
    {
        if (sr == null) return;
        if (isHovered) sr.color = HighlightColor;
        else sr.color = active ? ActiveColor : UnactiveColor;
    }

    
    private void OnMouseEnter()
    {
        if (movableSpace)
        {
            isHovered = true;
            ApplyColor();
        }
        else
        {
            isHovered = false;
            ApplyColor();
        }
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

    public void SetAsFromTile(bool value)
    {
        active = value;
        ApplyColor();
    }

    public void SetMovable(bool isMovableTo) => movableSpace = isMovableTo;

    public void SetHighlight(bool hasHighlight, HighlightType type)
    {
        if (Highlight == null) return;
        Highlight.SetActive(hasHighlight);
        if (!hasHighlight) return;

        SpriteRenderer highlightSR = Highlight.GetComponent<SpriteRenderer>();
        if (highlightSR == null) return;
        if (type == HighlightType.Move) highlightSR.color = HighlightColor;
        else if (type == HighlightType.Attack) highlightSR.color = AttackHighlightColor;
        else Highlight.SetActive(false);    // Should be unreachable
    }
}
