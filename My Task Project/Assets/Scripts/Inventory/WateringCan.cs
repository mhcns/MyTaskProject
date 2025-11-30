using UnityEngine;

public class WateringCan : Item
{
    public WateringCan(string nameId, string itemDescription, bool usable = false)
        : base(nameId, itemDescription, usable) { }

    public override bool UseItem()
    {
        Debug.Log($"usable {usable}");
        if (!usable)
            return false;

        DropController.Instance.DropFlower(
            InventoryUIController.Instance.playerInventory.transform.position
        );
        return true;
    }
}
