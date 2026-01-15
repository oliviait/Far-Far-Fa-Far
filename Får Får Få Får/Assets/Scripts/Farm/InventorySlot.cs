using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public GameObject Icon;
    private SpriteRenderer SpriteRenderer;
    public InventoryItem currentItem;

    public int ID;

    public bool HasItem => currentItem != null;

    private void Awake()
    {
        SpriteRenderer = Icon.GetComponent<SpriteRenderer>();
    }

    public void SetItem(InventoryItem item)
    {
        currentItem = item;
        SpriteRenderer.sprite = item.icon;
        SpriteRenderer.enabled = true;
    }

    public void ClearSlot()
    {
        currentItem = null;
        SpriteRenderer.sprite = null;
        SpriteRenderer.enabled = false;
    }
}