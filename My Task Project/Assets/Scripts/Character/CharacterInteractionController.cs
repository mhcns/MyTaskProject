using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterInteractionController : MonoBehaviour
{
    private Inventory _inventory;
    private List<GameObject> _interactionList;

    void Awake()
    {
        _interactionList = new List<GameObject>();
        _inventory = GetComponent<Inventory>();
    }

    // If the player walks into a interaction trigger, puts into a priority list (works like a queue);
    // If the gameObject of said trigger is the first, activate its Interaction Text;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item") || collision.gameObject.CompareTag("NPC"))
        {
            _interactionList.Add(collision.gameObject);
            if (_interactionList[0] == collision.gameObject)
            {
                _interactionList[0].transform.Find("Interaction Text").gameObject.SetActive(true);
            }
        }
        else if (collision.gameObject.CompareTag("Finish"))
        {
            _inventory.UnlockWateringCan(true);
        }
    }

    // When the player exits a interaction trigger, if its the first on the list, deactivate its Icon, and activates the next on the list;
    void OnTriggerExit2D(Collider2D collision)
    {
        if (
            !collision.gameObject.IsDestroyed()
            && _interactionList.Count > 0
            && _interactionList[0] == collision.gameObject
        )
        {
            _interactionList[0].transform.Find("Interaction Text").gameObject.SetActive(false);
        }

        if (_interactionList.Contains(collision.gameObject))
        {
            _interactionList.Remove(collision.gameObject);
            if (_interactionList.Count > 0)
            {
                _interactionList[0].transform.Find("Interaction Text").gameObject.SetActive(true);
            }
        }

        if (collision.gameObject.CompareTag("Finish"))
        {
            _inventory.UnlockWateringCan(false);
        }
    }

    public void OnInteract(UnityEngine.InputSystem.InputValue value)
    {
        if (_interactionList.Count == 0)
            return;

        switch (_interactionList[0].tag)
        {
            case "Item":
                _inventory.CheckAddItem(
                    _interactionList[0].GetComponent<Item>(),
                    _interactionList[0]
                );
                break;
            case "NPC":
                _interactionList[0].GetComponent<NpcController>().Interact(_inventory.items.Count);
                break;
        }
    }

    public void OnToggleInventory(UnityEngine.InputSystem.InputValue value)
    {
        InventoryUIController.Instance.ToggleInventory();
    }
}
