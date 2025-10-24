using UnityEngine;

public class GridTile : MonoBehaviour
{
    public Vector2 GridPos;

    public Color UnactiveColor;
    public Color ActiveColor;
    public Color HoverColor;

    private bool active = false;
    private bool isHovered = false;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        ApplyColor();
    }

    private void ApplyColor()
    {
        if (sr == null) return;
        if (isHovered) sr.color = HoverColor;
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

    // optional helper if you want to set from other scripts
    public void SetActive(bool value)
    {
        active = value;
        ApplyColor();
    }
}
