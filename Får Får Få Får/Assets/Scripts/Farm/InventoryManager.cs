using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    public List<InventorySlot> slots = new();


    private void Awake()
    {
        if (Instance && Instance != this) Destroy(gameObject);
        else Instance = this;
        
        // Clear slots jic
        int counter = 0;
        foreach (var slot in slots)
        {
            slot.currentItem = null;
            slot.i = counter++;
        }
    }

    public void ReturnSheepToFarm(GameObject sheepObj, InventorySlot slot)
    {
        slot.currentItem = null; // Remove sheep from slot
        SheepData data = sheepObj.GetComponent<Stats>().Data;

        Player.Instance.farmSheepList.Add(data);
        Player.Instance.inventorySheepList.Remove(data);
    }

    public void MoveSheepToInventory(GameObject sheepObj, InventorySlot slot)
    {
        slot.currentItem = sheepObj;
        SheepData data = sheepObj.GetComponent<Stats>().Data;

        Player.Instance.inventorySheepList.Add(data);
        if (Player.Instance.farmSheepList.Contains(data)) // If moved from farm 
            Player.Instance.farmSheepList.Remove(data);
    }

    public void ReloadSheepFromSlotsToInventory()
    {
        // Load with fixed order
        Player.Instance.inventorySheepList.Clear();
        foreach (var slot in slots)
        {
            if (slot.currentItem != null) MoveSheepToInventory(slot.currentItem, slot);
        }
    }

    public void PlaceSheepInSlot(GameObject sheep, int i)
    {
        sheep.transform.position = slots[i].transform.position;

        slots[i].currentItem = sheep;

        SheepDragger dragger = sheep.GetComponent<SheepDragger>();
        dragger.currentInventorySlot = slots[i];
        dragger.inSlot = true;

        SheepData data = sheep.GetComponent<Stats>().Data;
        Player.Instance.farmSheepList.Remove(data);
    }


}