using UnityEngine;

public class DropController : MonoBehaviour
{
    public static DropController Instance;
    public GameObject[] itemPrefabs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitSingleton();
        itemPrefabs = Resources.LoadAll<GameObject>("Prefabs/Items");
    }

    private void InitSingleton()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void DropItem(Item item, Vector3 position)
    {
        foreach (GameObject itemPrefab in itemPrefabs)
        {
            if (itemPrefab.name == item.nameId)
            {
                Item droppedItem = Instantiate(itemPrefab, position, Quaternion.identity)
                    .GetComponent<Item>();
                droppedItem.itemDescription = item.itemDescription;
            }
        }
    }

    public void DropFlower(Vector3 position)
    {
        foreach (GameObject itemPrefab in itemPrefabs)
        {
            if (itemPrefab.name == "Flower")
            {
                Instantiate(itemPrefab, position, Quaternion.identity);
            }
        }
    }
}
