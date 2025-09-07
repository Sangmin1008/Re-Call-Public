using System.Collections.Generic;
using UnityEngine;

public class ItemDatabase : Singleton<ItemDatabase>
{
    [SerializeField] private List<GenericItemDataSO> itemDatabase;
    private Dictionary<string, GenericItemDataSO> itemDictionary = new Dictionary<string, GenericItemDataSO>();

    protected override void Awake()
    {
        base.Awake();
        LoadItemDatabase();
        BuildDatabase();
    }

    private void BuildDatabase()
    {
        itemDictionary.Clear();
        foreach (var item in itemDatabase)
        {
            if (item != null && !itemDictionary.ContainsKey(item.ItemID))
            {
                itemDictionary.Add(item.ItemID, item);
            }
        }

    }

    public GenericItemDataSO GetItemByID(string itemID)
    {
        itemDictionary.TryGetValue(itemID, out GenericItemDataSO item);
        if (item == null)
        {
            Debug.LogWarning($"아이템을 찾을 수 없습니다: {itemID}");
        }
        return item;
    }

    public void AddItemToDatabase(GenericItemDataSO item)
    {
        if (item != null && !itemDictionary.ContainsKey(item.ItemID))
        {
            itemDatabase.Add(item);
            itemDictionary.Add(item.ItemID, item);
        }
    }

    public void LoadItemDatabase()
    {
        itemDatabase = new List<GenericItemDataSO>(Resources.LoadAll<GenericItemDataSO>("ScriptableObjects"));
    }

    public GameObject SpawnItem(Vector3 position, GenericItemDataSO itemDataSO)
    {
        if(itemDataSO.WorldPickupPrefab == null)
        {
            return null;
        }

        GameObject droppedItem = Instantiate(itemDataSO.WorldPickupPrefab, position, Quaternion.identity);
        return droppedItem;
    }

    public GameObject SpawnItem(Vector3 position, Quaternion quaternion, GenericItemDataSO itemDataSO)
    {
        if(itemDataSO.WorldPickupPrefab == null)
        {
            return null;
        }

        GameObject droppedItem = Instantiate(itemDataSO.WorldPickupPrefab, position, quaternion);
        return droppedItem;
    }
}