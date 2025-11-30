using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private int _inventorySize;
    private Dictionary<int, Item> _items = new();
    public Dictionary<int, Item> items => _items;
    public Action OnInventoryChange;
    private Coroutine _saveCoroutine;
    private static WaitForSeconds _waitForSeconds0_3 = new WaitForSeconds(0.3f);
    private bool _loadedInventory = false;

    void Start()
    {
        _inventorySize = 8;
        OnInventoryChange += SaveData;
    }

    void Update()
    {
        if (!_loadedInventory && InventoryUIController.Instance != null)
        {
            LoadInventory();
            _loadedInventory = true;
        }
    }

    private void LoadInventory()
    {
        SaveData savedData = SaveSystem.Load();
        if (savedData.inventory == null || savedData.inventory.Count == 0)
        {
            return;
        }

        foreach (InventorySlot slotItem in savedData.inventory)
        {
            Item savedItem = new(slotItem.item[0], slotItem.item[1]);
            CheckAddItem(savedItem, null, slotItem.slotId);
        }
    }

    // Checks if its possible to add Items to the inventory;
    public void CheckAddItem(Item item, GameObject itemObject, int slot = -1)
    {
        if (_items.Count >= _inventorySize)
        {
            Debug.LogError("Inventory is full");
            return;
        }

        // Adds 'item' to the first empty slot in the inventory if the slot wasn't already set;
        if (slot < 0)
        {
            for (int i = 0; i < _inventorySize; i++)
            {
                if (!_items.ContainsKey(i))
                {
                    AddItem(item, itemObject, i);
                    return;
                }
            }
        }
        else
        {
            if (!_items.ContainsKey(slot))
                AddItem(item, itemObject, slot);
        }
    }

    private void AddItem(Item item, GameObject itemObject, int slot)
    {
        if (item.nameId == "Watering Can")
        {
            WateringCan wateringCan = new(item.nameId, item.itemDescription);
            _items.Add(slot, wateringCan);
            InventoryUIController.Instance.AddItem(wateringCan, slot);
        }
        else
        {
            Item newItem = new(item.nameId, item.itemDescription);
            _items.Add(slot, newItem);
            InventoryUIController.Instance.AddItem(newItem, slot);
        }

        if (itemObject != null)
            Destroy(itemObject);

        OnInventoryChange?.Invoke();
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
        OnInventoryChange?.Invoke();
    }

    public bool MoveItem(int fromSlot, int destinySlot)
    {
        bool swappedItems;
        // if there is already an item on destiny slot, swap them
        if (_items.ContainsKey(destinySlot))
        {
            Item fromItem = _items[fromSlot];
            Item destinyItem = _items[destinySlot];

            _items.Remove(fromSlot);
            _items.Remove(destinySlot);

            _items.Add(destinySlot, fromItem);
            _items.Add(fromSlot, destinyItem);
            swappedItems = true;
        }
        else
        {
            Item itemMoved = _items[fromSlot];
            _items.Remove(fromSlot);
            _items.Add(destinySlot, itemMoved);
            swappedItems = false;
        }
        OnInventoryChange?.Invoke();
        return swappedItems;
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
                CheckAddItem(new WateringCan(item.nameId, item.itemDescription), null);
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

    private void SaveData()
    {
        if (_saveCoroutine != null)
        {
            StopCoroutine(_saveCoroutine);
        }

        _saveCoroutine = StartCoroutine(SaveCoroutine());
    }

    // Save coroutine so it doesn't spam saving in case multiple changes to the inventory are made in sequence;
    private IEnumerator SaveCoroutine()
    {
        yield return _waitForSeconds0_3;
        SaveData data = new SaveData();
        data.inventory = new();
        string[] tempArray = new string[2];
        foreach (KeyValuePair<int, Item> item in _items)
        {
            tempArray[0] = item.Value.nameId;
            tempArray[1] = item.Value.itemDescription;
            data.inventory.Add(new InventorySlot { slotId = item.Key, item = tempArray });
        }
        SaveSystem.Save(data);
    }
}
