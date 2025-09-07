using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldItemPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private GenericItemDataSO itemData;

    public GenericItemDataSO ItemData => itemData;

    private void Start()
    {
        if (!itemData.Loop[GameManager.Instance.LoopCount % 4])
            gameObject.SetActive(false);
    }

    public void OnInteract()
    {
        Debug.Log("아이템 획득");
        EventBus.Publish<GenericItemDataSO>("AddItem", itemData);
        EventBus.Publish("HideInteractPrompt", null);

        DroppedItemManager.Instance.RemoveDroppedItem(itemData.ItemID, transform.position);
        Destroy(gameObject);
    }

    public string GetInteractText() => $"[E] 키를 누르세요: {itemData.ItemName}\n{itemData.Description}";

    public void ToggleOutLine()
    {
        return;
    }
}