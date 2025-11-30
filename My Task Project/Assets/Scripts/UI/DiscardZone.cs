using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DiscardZone : MonoBehaviour, IDropHandler
{
    private GraphicRaycaster _graphicRaycaster;

    void Awake()
    {
        _graphicRaycaster = GetComponent<GraphicRaycaster>();
    }

    public void OnDrop(PointerEventData eventData)
    {
        GameObject draggedItem = eventData.pointerDrag;
        ItemUI item = draggedItem.GetComponent<ItemUI>();
        InventoryUIController.Instance.playerInventory.RemoveItem(item.lastSlot);
        _graphicRaycaster.enabled = false;
        Destroy(draggedItem);
    }
}
