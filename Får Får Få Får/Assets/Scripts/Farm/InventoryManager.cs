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
    }

    private void Start()
    {
        // Clear slots jic
        int counter = 0;
        foreach (var slot in slots)
        {
            slot.currentItem = null;
            slot.i = counter++;
        }

        
        // TODO Put sheep into slots
        // Just putting sheep onto farm for now
        Player.Instance.SheepOnFarmList.AddRange(Player.Instance.InventorySheepList);
        for (int i = 0; i < Player.Instance.InventorySheepList.Count(); i++)
        {
            Player.Instance.InventorySheepList.RemoveAt(0);
        }
    }

    public void ReturnSheepToFarm(GameObject sheepObj, InventorySlot slot)
    {
        slot.currentItem = null;    // Remove sheep from slot
        SheepData data = sheepObj.GetComponent<Stats>().Data;
        
        Player.Instance.SheepOnFarmList.Add(data);
        Player.Instance.InventorySheepList.Remove(data);
    }

    public void MoveSheepToInventory(GameObject sheepObj, InventorySlot slot)
    {
        slot.currentItem = sheepObj;
        SheepData data = sheepObj.GetComponent<Stats>().Data;
        
        Player.Instance.InventorySheepList.Add(data);    // Set sheep as i-th in inventory
        Player.Instance.SheepOnFarmList.Remove(data);
    }
}
