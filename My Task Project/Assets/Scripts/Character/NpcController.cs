using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NpcController : MonoBehaviour
{
    [SerializeField]
    private List<string> _dialogues;

    [SerializeField]
    private List<string> _conditionalDialogue;

    [SerializeField]
    private List<string> _secretDialogue;
    private int _currentLine = 0;
    private int _timesInteracted = 0;

    void OnEnable()
    {
        transform.Find("Interaction Text").GetComponent<MeshRenderer>().sortingOrder = 4;
    }

    public void Interact(int coinsCount)
    {
        List<string> currentDialogue;
        if (coinsCount == 5 && name == "Treant")
            currentDialogue = _conditionalDialogue;
        else if (_timesInteracted >= 10)
            currentDialogue = _secretDialogue;
        else
            currentDialogue = _dialogues;

        InventoryUIController.Instance.CloseButton();
        if (_currentLine < currentDialogue.Count)
        {
            Camera.main.GetComponent<CameraFollow>().SetCameraTarget(transform);
            DialogueUIController.Instance.ShowDialogue(name, currentDialogue[_currentLine]);
        }
        else
        {
            Camera.main.GetComponent<CameraFollow>().ResetCameraTarget();
            DialogueUIController.Instance.EndConversation();

            if (currentDialogue == _conditionalDialogue)
            {
                InventoryUIController.Instance.playerInventory.TreantInteraction();
                _timesInteracted = 0;
            }

            Debug.Log($"timesInteracted {_timesInteracted}");

            _currentLine = 0;
            if (_timesInteracted < 10)
            {
                _timesInteracted++;
            }
            else
            {
                _timesInteracted = 0;
            }
            return;
        }
        _currentLine++;
    }
}
