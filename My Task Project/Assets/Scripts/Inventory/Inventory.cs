using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private int _inventorySize;
    private Dictionary<int, Item> _items = new();
    public Dictionary<int, Item> items => _items;

    void Start()
    {
        _inventorySize = 8;
        // Implement load inventory later
    }

    // Adds 'item' to the first empty slot in the inventory;
    public void AddItem(Item item)
    {
        if (_items.Count >= _inventorySize)
        {
            // Inventory is full message;
            return;
        }

        for (int i = 0; i < _inventorySize; i++)
        {
            if (!_items.ContainsKey(i))
            {
                if (item.nameId == "Watering Can")
                {
                    WateringCan wateringCan = new(item.nameId, item.itemDescription);
                    _items.Add(i, wateringCan);
                    InventoryUIController.Instance.AddItem(wateringCan, i);
                }
                else
                {
                    Item newItem = new(item.nameId, item.itemDescription);
                    _items.Add(i, newItem);
                    InventoryUIController.Instance.AddItem(newItem, i);
                }
                if (item.gameObject != null)
                    Destroy(item.gameObject);
                return;
            }
        }
    }

    public void RemoveItem(int slot, bool usedItem = false)
    {
        if (!_items.ContainsKey(slot))
        {
            Debug.LogError($"There is no item on the slot {slot}");
            return;
        }

        if (!usedItem)
        {
            Item removedItem = new(_items[slot].nameId, _items[slot].itemDescription);
            DropController.Instance.DropItem(removedItem, transform.position);
        }
        InventoryUIController.Instance.RemoveItem(slot);
        _items.Remove(slot);
    }

    public bool MoveItem(int fromSlot, int destinySlot)
    {
        // if there is already an item on destiny slot, swap them
        if (_items.ContainsKey(destinySlot))
        {
            Item fromItem = _items[fromSlot];
            Item destinyItem = _items[destinySlot];

            _items.Remove(fromSlot);
            _items.Remove(destinySlot);

            _items.Add(destinySlot, fromItem);
            _items.Add(fromSlot, destinyItem);
            return true;
        }
        else
        {
            Item itemMoved = _items[fromSlot];
            _items.Remove(fromSlot);
            _items.Add(destinySlot, itemMoved);
            return false;
        }
    }

    public void TreantInteraction()
    {
        for (int i = 0; i < _inventorySize; i++)
        {
            if (_items.ContainsKey(i))
            {
                RemoveItem(i, true);
            }
        }

        foreach (GameObject itemPrefab in DropController.Instance.itemPrefabs)
        {
            if (itemPrefab.name == "Watering Can")
            {
                Item item = itemPrefab.GetComponent<Item>();
                AddItem(new WateringCan(item.nameId, item.itemDescription));
                return;
            }
        }
    }

    public void UnlockWateringCan(bool value)
    {
        foreach (Item item in _items.Values)
        {
            if (item.nameId == "Watering Can")
            {
                item.usable = value;
            }
        }
    }
}
