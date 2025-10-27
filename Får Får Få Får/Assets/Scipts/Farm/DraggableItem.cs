using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public string itemName;
    public Sprite icon;

    private GameObject dragIcon;
    private RectTransform dragRect;

    public void OnBeginDrag(PointerEventData eventData)
    {
        var canvas = GameObject.Find("Canvas").transform;
        dragIcon = new GameObject("DragIcon");
        dragIcon.transform.SetParent(canvas, false);

        var image = dragIcon.AddComponent<Image>();
        image.sprite = Resources.Load<Sprite>("Sprites/Circle");
        image.raycastTarget = false;

        dragRect = dragIcon.GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (dragRect != null)
            dragRect.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

        if (eventData.pointerEnter != null)
        {
            var slot = eventData.pointerEnter.GetComponentInParent<InventorySlot>();
            if (slot != null)
            {

                InventoryItem newItem = new InventoryItem
                {
                    itemName = itemName,
                    icon = icon,
                    originalObject = gameObject
                };

                if (InventoryManager.Instance.AddItem(newItem))
                {
                    gameObject.SetActive(false);
                }
            }
    
        }

        if (dragIcon != null)
            Destroy(dragIcon);
    }
}