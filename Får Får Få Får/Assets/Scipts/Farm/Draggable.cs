using UnityEngine;

public class Draggable : MonoBehaviour
{
    private bool isDragging;
    private bool inInventory;
    private InventorySlot inventorySlot;
    private Vector3 pos;

    private void Start()
    {
        inInventory = false;
    }

    public void OnMouseDown()
    {
        if (inInventory)
        {
            pos = new Vector3(UnityEngine.Random.Range(-8f, 8f), UnityEngine.Random.Range(-2.5f, 4f), 0f);
            transform.position = pos;
            inventorySlot.ClearSlot();
            inInventory = false;
            if (gameObject.GetComponent<Selectable>().Selected)
            {
                gameObject.GetComponent<Selectable>().Deselect();
            }
        }
        else
        {
            isDragging = true;
            pos = transform.position;
        }
    }
    public void OnMouseUp()
    {
        transform.position = pos;
        isDragging = false;
    }
    public void OnMouseDrag()
    {
        if (isDragging)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            transform.Translate(mousePosition);
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        var slot = other.GetComponent<InventorySlot>();
        if (slot != null)
        {   
            if (!slot.HasItem)
            {

                InventoryItem newItem = new InventoryItem
                {
                    itemName = gameObject.name,
                    icon = gameObject.GetComponent<Sprite>(),
                    originalObject = gameObject
                };
                slot.SetItem(newItem);
                pos = other.transform.position;
                transform.position = pos;
                isDragging = false;
                inInventory = true;
                inventorySlot = slot;
                if (gameObject.GetComponent<Selectable>().Selected)
                {
                    gameObject.GetComponent<Selectable>().Deselect();
                }
            }
        }
    }
}
