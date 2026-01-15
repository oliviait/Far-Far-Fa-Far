using UnityEngine;

public class Draggable : MonoBehaviour
{
    private bool isDragging;
    public bool inInventory;
    public InventorySlot inventorySlot;
    private Vector3 pos;
    private Camera mainCam;
    private PolygonCollider2D boundsCollider;
    

    [Header("Drag Bounds (world units)")]
    public Vector2 dragMin = new Vector2(-8f, -2f);
    public Vector2 dragMax = new Vector2(8f, 4f);

    private void Awake()
    {
        mainCam = Camera.main;
        // Get bounds to snap sheep back to
        var fence = GameObject.Find("FarmFence");
        if (fence != null)
            boundsCollider = fence.GetComponentInChildren<PolygonCollider2D>();
    }
    public void OnMouseDown()
    {
        if (inInventory)
        {
            pos = new Vector3(Random.Range(dragMin.x, dragMax.x), Random.Range(dragMin.y, dragMax.y), 0f);
            transform.position = pos;
            inventorySlot.ClearSlot();
            inInventory = false;
            inventorySlot = null;
            Player.Instance.RemoveFromInventory(gameObject.GetComponent<Stats>().Data);
            if (gameObject.GetComponent<Selectable>().Selected)
                gameObject.GetComponent<Selectable>().Deselect();
        }
        else
        {
            isDragging = true;
            pos = transform.position;
        }
    }

    public void OnMouseUp()
    {
        isDragging = false;

        if (boundsCollider != null)
        {
            // Snap back if outside trapezoid
            if (!boundsCollider.OverlapPoint(transform.position))
            {
                transform.position = boundsCollider.ClosestPoint(transform.position);
            }
        }
    }

    public void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector3 mousePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0f;
            transform.position = mousePosition;
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (inventorySlot) return;
        
        var slot = other.GetComponent<InventorySlot>();
        if (slot != null && !slot.HasItem)
        {   
            InventoryItem newItem = new InventoryItem
            {
                itemName = gameObject.name,
                icon = gameObject.GetComponent<SpriteRenderer>().sprite,
                originalObject = gameObject
            };
            slot.SetItem(newItem);
            pos = other.transform.position;
            transform.position = pos;
            isDragging = false;
            inInventory = true;
            inventorySlot = slot;
            if (gameObject.GetComponent<Selectable>().Selected)
                gameObject.GetComponent<Selectable>().Deselect();
            Player.Instance.AddToInventory(gameObject.GetComponent<Stats>().Data, slot.ID);
        }
    }
}
