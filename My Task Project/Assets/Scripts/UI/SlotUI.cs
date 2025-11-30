using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotUI : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [NonSerialized]
    public int slotPosition;
    private Outline _outline;

    void Awake()
    {
        _outline = GetComponent<Outline>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject draggedItem = eventData.pointerDrag;

        if (draggedItem != null)
        {
            InventoryUIController.Instance.MoveItem(
                draggedItem,
                draggedItem.GetComponent<ItemUI>().lastSlot,
                slotPosition
            );
        }
        InventoryUIController.Instance.ToggleRaycaster(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _outline.effectColor = Color.white;
        InventoryUIController.Instance.HighlightItem(slotPosition);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _outline.effectColor = Color.black;
        InventoryUIController.Instance.HighlightItem(-1);
    }
}
