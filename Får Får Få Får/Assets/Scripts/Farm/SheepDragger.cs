using UnityEngine;

public class SheepDragger : MonoBehaviour
{
    public bool inSlot;
    public InventorySlot currentInventorySlot;
    
    private bool isDraggingLocked;  // While dragging, is object's position locked
    private bool isDragging;    // Is this object currently being dragged

    private Camera mainCam;
    private PolygonCollider2D FarmBounds;
    private PolygonCollider2D DisposerBounds;


    private void Awake()
    {
        mainCam = Camera.main;
        // Get bounds to snap sheep back to
        var fence = GameObject.Find("FarmFence");
        if (fence != null)
            FarmBounds = fence.GetComponentInChildren<PolygonCollider2D>();
        var disposer = GameObject.Find("SheepDisposer");
        if (disposer != null) 
            DisposerBounds = disposer.GetComponentInChildren<PolygonCollider2D>();
    }

    public void OnMouseDown()
    {
        if (inSlot) // Remove sheep from inventory if clicked on
        {
            InventoryManager.Instance.ReturnSheepToFarm(gameObject, currentInventorySlot);
            inSlot = false;
            currentInventorySlot = null;
        }

        isDragging = true;
    }

    public void OnMouseUp()
    {
        isDragging = false;

        // If the sheep isn’t in an inventory slot and is outside the fence
        if (!inSlot && FarmBounds != null && !FarmBounds.OverlapPoint(transform.position))
        {
            if (!DisposerBounds.OverlapPoint(transform.position))
                transform.position = FarmBounds.ClosestPoint(transform.position); // Snap it back inside
        }

        // If sheep is in slot, add it to inventory
        if (inSlot) InventoryManager.Instance.MoveSheepToInventory(gameObject, currentInventorySlot);
    }

    public void OnMouseDrag()
    {
        if (!isDragging) return; // If not dragging

        Vector3 mousePosition = mainCam.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;

        // Is mouse over inv slot
        Collider2D[] hits = Physics2D.OverlapPointAll(mousePosition);
        bool hoveringSlot = false;
        foreach (Collider2D hit in hits)    // Go through everything the mouse is over
        {
            if (hit.gameObject == gameObject) continue; // skip self
            InventorySlot slot = hit.GetComponent<InventorySlot>();
            if (slot != null) // If over slot
            {
                hoveringSlot = true;
                if (slot.HasItem)
                {
                    if (slot == currentInventorySlot)
                    {
                        isDraggingLocked = true;
                        inSlot = true;
                        transform.position = slot.transform.position;
                        currentInventorySlot = slot;
                    }
                    else
                    {
                        isDraggingLocked = false; // Hovering already taken slot
                        inSlot = false;
                    }
                }
                else // Empty slot, snap sheep into it
                {
                    isDraggingLocked = true;
                    inSlot = true;
                    transform.position = slot.transform.position;
                    currentInventorySlot = slot;
                }
            }
        }
        

        if (!hoveringSlot)
        {
            isDraggingLocked = false;
            inSlot = false;
            
            if (DisposerBounds.OverlapPoint(mousePosition))   // If over disposer
            {
                isDraggingLocked = true;

                GameObject sheepDisposer = GameObject.Find("SheepDisposer"); 
                transform.position = sheepDisposer.transform.position;
                sheepDisposer.GetComponent<SheepDisposer>().sheep = gameObject;
                
                gameObject.GetComponent<SheepPartsZIndexChooser>().PutInDisposer();
                gameObject.transform.SetParent(DisposerBounds.gameObject.transform);
            }
            else
            {
                GameObject.Find("SheepDisposer").GetComponent<SheepDisposer>().sheep = null;
                gameObject.GetComponent<SheepPartsZIndexChooser>().RemoveFromDisposer();
                gameObject.transform.SetParent(null);
            }
        }
        
        // Move sheep if not locked to inventory slot
        if (!isDraggingLocked) transform.position = mousePosition;
    }
}