using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    public static InventoryUIController Instance;
    private Inventory _playerInventory;

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

    // Update is called once per frame
    void Update() { }
}
