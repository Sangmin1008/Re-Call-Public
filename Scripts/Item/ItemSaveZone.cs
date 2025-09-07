using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSaveZone : MonoBehaviour
{
    private bool _isPlayerInside = false;

    private void OnEnable()
    {
        EventBus.Subscribe<DroppedItemData>("DropItem", OnDropItem);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<DroppedItemData>("DropItem", OnDropItem);
    }

    private void OnDropItem(DroppedItemData data)
    {
        if (_isPlayerInside)
        {
            DroppedItemManager.Instance.SaveDroppedItem(data.itemID, data.position, data.quaternion);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _isPlayerInside = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            _isPlayerInside = false;
        }
    }
}
