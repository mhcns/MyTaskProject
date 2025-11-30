using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class InventoryUIController : MonoBehaviour, IDropHandler
{
    public static InventoryUIController Instance;

    [SerializeField]
    private GameObject _inventoryItemPrefab;

    [SerializeField]
    private List<SlotUI> _inventorySlotsUI;

    [SerializeField]
    private Inventory _playerInventory;

    void Start()
    {
        InitSingleton();
    }

    private void InitSingleton()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void MoveItem(GameObject itemObject, int fromSlot, int destinySlot)
    {
        bool moveCompleted = _playerInventory.MoveItem(fromSlot, destinySlot);
        if (moveCompleted)
        {
            itemObject.transform.parent = _inventorySlotsUI[destinySlot].transform;
            itemObject.transform.position = _inventorySlotsUI[destinySlot].transform.position;
        }
        else
        {
            itemObject.transform.parent = _inventorySlotsUI[fromSlot].transform;
            itemObject.transform.position = _inventorySlotsUI[fromSlot].transform.position;
        }
    }

    public void AddItem(Item item, int slot)
    {
        GameObject newItemObject = Instantiate(_inventoryItemPrefab);
        Item newItem = newItemObject.GetComponent<Item>();
        newItemObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(
            $"Sprites/{item.nameId}"
        );
        newItemObject.transform.parent = _inventorySlotsUI[slot].transform;
        newItemObject.transform.position = _inventorySlotsUI[slot].transform.position;
    }

    public void RemoveItem(int slot)
    {
        GameObject inventoryItemObject = _inventorySlotsUI[slot].transform.GetChild(0).gameObject;
        Destroy(inventoryItemObject);
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject draggedItem = eventData.pointerDrag;
        ItemUI item = draggedItem.GetComponent<ItemUI>();
        _playerInventory.RemoveItem(item.lastSlot);
    }
}
