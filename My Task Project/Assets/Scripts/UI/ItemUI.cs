using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private GraphicRaycaster _graphicRaycaster;
    private RectTransform _rectTransform;
    public int lastSlot = -1;

    void Awake()
    {
        _graphicRaycaster = GetComponent<GraphicRaycaster>();
        _rectTransform = GetComponent<RectTransform>();
        _rectTransform.GetComponent<RawImage>().SetNativeSize();
        _rectTransform.localScale = Vector3.one * 5f; // Value that makes the Image fill the slot
        InventoryUIController.Instance.OnCloseInventory += Reset;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _graphicRaycaster.enabled = false;
        InventoryUIController.Instance.ToggleRaycaster(true);
        lastSlot = transform.parent.GetComponent<SlotUI>().slotPosition;
        transform.parent = transform.parent.parent;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _graphicRaycaster.enabled = true;
    }

    public void Reset()
    {
        if (transform.parent.TryGetComponent<SlotUI>(out _))
            return;

        _graphicRaycaster.enabled = true;
        transform.parent = InventoryUIController.Instance.inventorySlotsUI[lastSlot].transform;
        transform.position = transform.parent.position;
        lastSlot = -1;
    }

    void OnDestroy()
    {
        InventoryUIController.Instance.OnCloseInventory -= Reset;
    }
}
