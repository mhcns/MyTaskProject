using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string nameId;
    public string itemDescription;

    void OnEnable()
    {
        transform.Find("Interaction Text").GetComponent<MeshRenderer>().sortingOrder = 4;
    }

    public Item(string nameId, string itemDescription)
    {
        this.nameId = nameId;
        this.itemDescription = itemDescription;
    }
}
