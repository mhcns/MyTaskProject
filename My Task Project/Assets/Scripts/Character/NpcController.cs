using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NpcController : MonoBehaviour
{
    [SerializeField]
    private List<string> _dialogues;
    private int _currentLine;

    void OnEnable()
    {
        transform.Find("Interaction Text").GetComponent<MeshRenderer>().sortingOrder = 4;
    }

    public void OnInteract(InputValue inputValue)
    {
        if (_currentLine < _dialogues.Count)
        {
            DialogueUIController.Instance.ShowDialogue(_dialogues[_currentLine]);
        }
        else
        {
            DialogueUIController.Instance.EndConversation();
            _currentLine = 0;
            return;
        }
        _currentLine++;
    }
}
