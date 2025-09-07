using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldPlaceableItemPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private GenericItemDataSO itemData;
    public void OnInteract()
    {
        var placeable = itemData as PlaceableItemDataSO;
        if (placeable != null)
        {
            EventBus.Publish<PlaceableItemDataSO>("buildStartEvent", placeable);
            EventBus.Publish<Vector3>("buildOriginPosition", transform.position);
            EventBus.Publish<Quaternion>("buildOriginRotation", transform.rotation);
        }

        DroppedItemManager.Instance.RemoveDroppedItem(itemData.ItemID, transform.position, transform.rotation);
        Destroy(gameObject);
    }

    public string GetInteractText() => $"[E] 키를 누르세요: {itemData.ItemName}\n{itemData.Description}";
}
