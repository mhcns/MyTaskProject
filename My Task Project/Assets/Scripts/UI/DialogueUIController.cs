using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUIController : MonoBehaviour
{
    public static DialogueUIController Instance;
    private GameObject _backgroundObject;

    [SerializeField]
    private TextMeshProUGUI _npcNameText;

    [SerializeField]
    private TextMeshProUGUI _dialogueText;

    void Start()
    {
        InitSingleton();
        _backgroundObject = transform.GetChild(0).gameObject;
    }

    private void InitSingleton()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ShowDialogue(string npcName, string text)
    {
        if (!_backgroundObject.activeSelf)
            _backgroundObject.SetActive(true);

        _npcNameText.text = npcName;
        _dialogueText.text = text;
    }

    public void EndConversation()
    {
        _backgroundObject.SetActive(false);
    }

    public static bool IsActive()
    {
        return Instance._backgroundObject.activeSelf;
    }
}
