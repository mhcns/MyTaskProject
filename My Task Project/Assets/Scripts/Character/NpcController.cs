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

    public void Interact()
    {
        InventoryUIController.Instance.CloseButton();
        if (_currentLine < _dialogues.Count)
        {
            Camera.main.GetComponent<CameraFollow>().SetCameraTarget(transform);
            DialogueUIController.Instance.ShowDialogue(name, _dialogues[_currentLine]);
        }
        else
        {
            Camera.main.GetComponent<CameraFollow>().ResetCameraTarget();
            DialogueUIController.Instance.EndConversation();
            _currentLine = 0;
            return;
        }
        _currentLine++;
    }
}
