using TMPro;
using UnityEngine;

public class DialogueUIController : MonoBehaviour
{
    public static DialogueUIController Instance;
    private GameObject _backgroundObject;

    [SerializeField]
    private TextMeshProUGUI _dialogueText;

    void Start()
    {
        InitSingleton();
    }

    private void InitSingleton()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ShowDialogue(string text)
    {
        if (!_backgroundObject.activeSelf)
            _backgroundObject.SetActive(true);

        _dialogueText.text = text;
    }

    public void EndConversation()
    {
        _backgroundObject.SetActive(false);
    }
}
