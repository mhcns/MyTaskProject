using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CharacterInteractionController : MonoBehaviour
{
    private List<GameObject> _interactionList;

    void Awake()
    {
        _interactionList = new List<GameObject>();
    }

    // If the player walks into a interaction trigger, puts into a priority list (works like a queue);
    // If the gameObject of said trigger is the first, activate its Interaction Icon;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Item") || collision.gameObject.CompareTag("NPC"))
        {
            _interactionList.Add(collision.gameObject);
            if (_interactionList[0] == collision.gameObject)
            {
                _interactionList[0].transform.Find("Interaction Icon").gameObject.SetActive(true);
            }
        }
    }

    // When the player exits a interaction trigger, if its the first on the list, deactivate its Icon, and activates the next on the list;
    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.IsDestroyed() && _interactionList[0] == collision.gameObject)
        {
            _interactionList[0].transform.Find("Interaction Icon").gameObject.SetActive(false);
        }

        if (_interactionList.Contains(collision.gameObject))
        {
            _interactionList.Remove(collision.gameObject);
            if (_interactionList.Count > 0)
            {
                _interactionList[0].transform.Find("Interaction Icon").gameObject.SetActive(true);
            }
        }
    }
}
