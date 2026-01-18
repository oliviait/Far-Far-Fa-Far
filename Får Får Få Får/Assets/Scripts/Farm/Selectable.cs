using UnityEngine;

public class Selectable : MonoBehaviour
{
    public bool Selected;

    [Header("Selection Settings")]
    public float maxHoldTime = 1f;
    public float maxMoveDistance = 0.2f;

    public bool isSelectable;

    private float mouseDownTime;
    private bool isHolding;
    private Vector3 mouseDownPosition;

    void Start()
    {
        Selected = false;
        isSelectable = true;
    }

    void OnMouseDown()
    {
        if (!isSelectable) return;
        mouseDownTime = Time.time;
        mouseDownPosition = transform.position;
        isHolding = true;
    }

    void OnMouseUp()
    {
        if (!isHolding) return;

        isHolding = false;

        float heldTime = Time.time - mouseDownTime;
        float movedDistance = Vector3.Distance(transform.position, mouseDownPosition);

        if (heldTime > maxHoldTime) return;
        if (movedDistance > maxMoveDistance) return;

        if (Selected)
        {
            Deselect();
        }
        else if (Breeding.Instance.numSelected < 2)
        {
            Select();
        }
    }

    public void Select()
    {
        if (!isSelectable) return;
        Breeding.Instance.Increase(gameObject);
        Selected = true;
        SetColor(new Color(1f, 0.7f, 0.7f, 1f));
    }

    public void Deselect()
    {
        Breeding.Instance.Decrease(gameObject);
        Selected = false;
        SetColor(Color.white);
    }

    private void SetColor(Color color)
    {
        foreach (var sr in GetComponentsInChildren<SpriteRenderer>())
        {
            sr.color = color;
        }
    }
}