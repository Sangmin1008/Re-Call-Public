using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedItemManager : Singleton<DroppedItemManager>
{
    [System.NonSerialized]
    [SerializeField] private List<DroppedItemData> droppedItems = new List<DroppedItemData>();


    public void RemoveDroppedItem(string itemID, Vector3 position)
    {
        droppedItems.RemoveAll(item => item.itemID == itemID && Vector3.Distance(item.position, position) < 0.1f);
    }

    public void RemoveDroppedItem(string itemID, Vector3 position, Quaternion quaternion)
    {
        droppedItems.RemoveAll(item =>
                item.itemID == itemID &&
                Vector3.Distance(item.position, position) < 0.1f &&
                Quaternion.Angle(item.quaternion, quaternion) < 1f
        );
    }

    public void SaveDroppedItem(string itemID, Vector3 position)
    {
        droppedItems.Add(new DroppedItemData(itemID, position));
    }

    public void SaveDroppedItem(string itemID, Vector3 position, Quaternion quaternion)
    {
        droppedItems.Add(new DroppedItemData(itemID, position, quaternion));
    }

    [ContextMenu("Respawn All Items")]
    public void RespawnAllItems()
    {
        foreach (var data in droppedItems)
        {
            var itemData = ItemDatabase.Instance.GetItemByID(data.itemID);
            if (itemData != null)
            {
                ItemDatabase.Instance.SpawnItem(data.position, data.quaternion, itemData);
            }
        }
    }

    public void Clear() => droppedItems.Clear();
}
