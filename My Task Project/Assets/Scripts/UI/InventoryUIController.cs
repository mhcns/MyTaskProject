using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class InventoryUIController : MonoBehaviour
{
    public static InventoryUIController Instance;
    private GameObject _backgroundObject;
    private GameObject _discardZone;
    private GraphicRaycaster _graphicRaycaster;

    [SerializeField]
    private GameObject _inventoryItemPrefab;

    [SerializeField]
    private List<SlotUI> _inventorySlotsUI;
    public List<SlotUI> inventorySlotsUI => _inventorySlotsUI;

    private Inventory _playerInventory;
    public Inventory playerInventory => _playerInventory;

    [SerializeField]
    private TextMeshProUGUI _highlightedItemName;

    [SerializeField]
    private TextMeshProUGUI _highlightedItemDescription;

    public Action OnCloseInventory;

    void Start()
    {
        InitSingleton();
        _playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
        _backgroundObject = transform.GetChild(0).gameObject;
        _discardZone = transform.GetChild(1).gameObject;
        _graphicRaycaster = _discardZone.GetComponent<GraphicRaycaster>();
        for (int i = 0; i < _inventorySlotsUI.Count; i++)
        {
            _inventorySlotsUI[i].slotPosition = i;
        }
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
        bool swappedItems = _playerInventory.MoveItem(fromSlot, destinySlot);
        if (swappedItems)
        {
            _inventorySlotsUI[destinySlot].transform.GetChild(0).parent = _inventorySlotsUI[
                fromSlot
            ].transform;
            _inventorySlotsUI[fromSlot].transform.GetChild(0).position = _inventorySlotsUI[fromSlot]
                .transform
                .position;
        }
        itemObject.transform.parent = _inventorySlotsUI[destinySlot].transform;
        itemObject.transform.position = _inventorySlotsUI[destinySlot].transform.position;
    }

    public void AddItem(Item item, int slot)
    {
        GameObject newItemObject = Instantiate(_inventoryItemPrefab);
        newItemObject.GetComponent<RawImage>().texture = Resources.Load<Texture2D>(
            $"Sprites/{item.nameId}"
        );
        newItemObject.transform.parent = _inventorySlotsUI[slot].transform;
        newItemObject.transform.position = _inventorySlotsUI[slot].transform.position;
    }

    public void RemoveItem(int slot)
    {
        if (_inventorySlotsUI[slot].transform.childCount == 0)
            return;

        GameObject inventoryItemObject = _inventorySlotsUI[slot].transform.GetChild(0).gameObject;
        Destroy(inventoryItemObject);
    }

    public void HighlightItem(int slot)
    {
        if (slot < 0 || !_playerInventory.items.ContainsKey(slot))
        {
            _highlightedItemName.text = "";
            _highlightedItemDescription.text = "";
        }
        else
        {
            _highlightedItemName.text = _playerInventory.items[slot].nameId;
            _highlightedItemDescription.text = _playerInventory.items[slot].itemDescription;
        }
    }

    public void ToggleInventory()
    {
        if (DialogueUIController.IsActive())
            return;

        _backgroundObject.SetActive(!_backgroundObject.activeSelf);
        _discardZone.SetActive(_backgroundObject.activeSelf);

        if (!_backgroundObject.activeSelf)
        {
            OnCloseInventory?.Invoke();
        }
    }

    public void ToggleRaycaster(bool value)
    {
        _graphicRaycaster.enabled = value;
    }

    public void CloseButton()
    {
        if (!_backgroundObject.activeSelf)
            return;

        _backgroundObject.SetActive(false);
        _discardZone.SetActive(false);
        OnCloseInventory?.Invoke();
    }
}
