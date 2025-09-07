using System;

[Serializable]
public class ItemInstance
{
    public string itemID;
    public int quantity;

    public ItemInstance(string id, int qty)
    {
        itemID = id;
        quantity = qty;
    }

    public ItemInstance(GenericItemDataSO itemDataSO, int qty)
    {
        itemID = itemDataSO.ItemID;
        quantity = qty;
    }

    public bool CanStack(GenericItemDataSO itemDataSO)
    {
        return itemID == itemDataSO.ItemID && quantity < itemDataSO.MaxStack;
    }

    public void AddQuantity(int amount)
    {
        quantity += amount;
    }

    public void SubtractQuantity(int amount)
    {
        quantity = Math.Max(0, quantity - amount);
    }

    public bool IsEmpty()
    {
        return quantity <= 0;
    }
}