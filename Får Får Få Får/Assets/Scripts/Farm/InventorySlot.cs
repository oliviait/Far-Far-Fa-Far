using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    public InventoryItem currentItem;

    public int ID;

    public bool HasItem => currentItem != null;

    public void SetItem(InventoryItem item)
    {
        currentItem = item;
        iconImage.sprite = item.icon;
        iconImage.enabled = true;
    }

    public void ClearSlot()
    {
        currentItem = null;
        iconImage.sprite = null;
        iconImage.enabled = false;
    }

    private void Reset()
    {
        if (iconImage == null)
            iconImage = transform.Find("Icon")?.GetComponent<Image>();
    }
}