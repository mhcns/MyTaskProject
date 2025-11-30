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
                Item newItem = new(item.nameId, item.itemDescription);
                Debug.Log($"adding {newItem.nameId} to slot {i}");
                _items.Add(i, newItem);
                InventoryUIController.Instance.AddItem(newItem, i);
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
        if (_items.ContainsKey(destinySlot))
            return false;

        Item itemMoved = _items[fromSlot];
        _items.Remove(fromSlot);
        _items.Add(destinySlot, itemMoved);
        return true;
    }
}
