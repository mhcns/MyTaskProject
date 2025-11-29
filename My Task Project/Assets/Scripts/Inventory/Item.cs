using UnityEngine;

public class Item : MonoBehaviour
{
    public string nameId;
    public string itemDescription;

    public Item(string nameId, string itemDescription)
    {
        this.nameId = nameId;
        this.itemDescription = itemDescription;
    }
}
