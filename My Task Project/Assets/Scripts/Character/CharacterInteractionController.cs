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

    void Start()
    {
        var a = GetComponent<PlayerInput>().actions;
        foreach (var act in a)
        {
            Debug.Log("Action carregada: " + act.name);
            Debug.Log("Action habilitada: " + act.enabled);
        }
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
    }

    public void OnInteract(UnityEngine.InputSystem.InputValue value)
    {
        if (_interactionList.Count == 0)
            return;

        switch (_interactionList[0].tag)
        {
            case "Item":
                _inventory.AddItem(_interactionList[0].GetComponent<Item>());
                break;
            case "NPC":
                _interactionList[0].GetComponent<NpcController>().Interact();
                break;
        }
    }

    public void OnToggleInventory(UnityEngine.InputSystem.InputValue value)
    {
        InventoryUIController.Instance.ToggleInventory();
    }
}
