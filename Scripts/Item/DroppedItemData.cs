using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DroppedItemData
{
    [SerializeField] public string itemID;
    [SerializeField] public Vector3 position;
    [SerializeField] public Quaternion quaternion;

    public DroppedItemData(string itemID, Vector3 position)
    {
        this.itemID = itemID;
        this.position = position;
        this.quaternion = Quaternion.identity;
    }

    public DroppedItemData(string itemID, Vector3 position, Quaternion quaternion)
    {
        this.itemID = itemID;
        this.position = position;
        this.quaternion = quaternion;
    }
}
