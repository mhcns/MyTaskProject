using System;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string nameId;
    public string itemDescription;
    public bool usable = false;

    void OnEnable()
    {
        transform.Find("Interaction Text").GetComponent<MeshRenderer>().sortingOrder = 4;
    }

    public Item(string nameId, string itemDescription, bool usable = false)
    {
        this.nameId = nameId;
        this.itemDescription = itemDescription;
        this.usable = usable;
    }

    public virtual bool UseItem()
    {
        return false;
    }
}
