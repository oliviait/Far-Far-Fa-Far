using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    public int i;   // Slot's index in inventory
    public GameObject currentItem;

    public bool HasItem => currentItem != null;
}