using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    [SerializeField] private List<InventorySlot> slots = new List<InventorySlot>();

    private void Awake()
    {
        Instance = this;
    }

private void Start()
{

    foreach (var slot in slots)
    {
        slot.ClearSlot();
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
