using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    public List<InventorySlot> slots = new();

    private void Awake()
    {
        if (Instance && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    private void Start()
    {
        int counter = 0;
        foreach (var slot in slots)
        {
            slot.ClearSlot();
            slot.ID = counter++;
        }
    }
    public bool AddItem(InventoryItem item)
    {
        foreach (var slot in slots)
        {
            if (!slot.HasItem)
            {
                slot.SetItem(item);
                return true;
            }
        }

        return false;
    }
}
