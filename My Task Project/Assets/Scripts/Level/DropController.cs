using System.Collections.Generic;
using Mono.Cecil;
using Unity.VisualScripting;
using UnityEngine;

public class DropController : MonoBehaviour
{
    public static DropController Instance;
    private GameObject[] _itemPrefabs;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InitSingleton();
        _itemPrefabs = Resources.LoadAll<GameObject>("Items");
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
        foreach (GameObject itemPrefab in _itemPrefabs)
        {
            if (itemPrefab.name == item.nameId)
            {
                Item droppedItem = Instantiate(itemPrefab, position, Quaternion.identity)
                    .GetComponent<Item>();
                droppedItem.itemDescription = item.itemDescription;
            }
        }
    }
}
