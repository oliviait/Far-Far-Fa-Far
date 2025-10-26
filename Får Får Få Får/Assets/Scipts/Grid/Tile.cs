using UnityEngine;

public class Tile : MonoBehaviour
{
    public Vector2 GridPos;

    public Color UnactiveColor;
    public Color ActiveColor;
    public Color HighlightColor;

    private bool active = false;
    private bool isHovered = false;

    private SpriteRenderer sr;

    private GameObject occupant;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        ApplyColor();
    }

    public void SetOccupant(GameObject piece) => occupant = piece;
    public bool IsOccupied() => occupant != null;

    private void ApplyColor()
    {
        if (sr == null) return;
        if (isHovered) sr.color = HighlightColor;
        else sr.color = active ? ActiveColor : UnactiveColor;
    }

    
    private void OnMouseEnter()
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
        active = !active;
        ApplyColor();
    }

    public void SetActive(bool value)
    {
        active = value;
        ApplyColor();
    }
}
