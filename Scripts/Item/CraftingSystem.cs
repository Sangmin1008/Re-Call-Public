using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    [SerializeField] private PlayerInventory inventory;
    private bool _isCrafting = false;
    public bool IsCrafting => _isCrafting;

    private void OnEnable()
    {
        EventBus.Subscribe<CraftableItemDataSO>("CraftItemEvent", TryCraft);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<CraftableItemDataSO>("CraftItemEvent", TryCraft);
    }

    private void TryCraft(CraftableItemDataSO craftable)
    {
        if (!CanCraft(craftable))
        {
            Debug.Log("제작 불가능");
            EventBus.Publish<bool>("CraftableItemEvent", false);
            return;
        }
        _isCrafting = true;
        StartCoroutine(CraftRoutine(craftable));
        EventBus.Publish<bool>("CraftableItemEvent", true);
    }

    private bool CanCraft(CraftableItemDataSO craftable)
    {
        foreach (var entry in craftable.RecipeEntries)
        {
            var targetItem = entry.ingredient;
            var quantity = entry.quantity;

            ItemInstance foundItem = inventory.Items.Find(item => item.itemID == targetItem.ItemID);

            if (foundItem != null)
            {
                if (foundItem.quantity >= quantity) return true;
                else return false;
            }
            else
            {
                return false;
            }
        }

        return true;
    }

    private IEnumerator CraftRoutine(CraftableItemDataSO craftable)
    {
        foreach (var entry in craftable.RecipeEntries)
        {
            var targetItem = entry.ingredient;
            var quantity = entry.quantity;

            ItemInstance foundItem = inventory.Items.Find(item => item.itemID == targetItem.ItemID);
            foundItem.AddQuantity(-quantity);

            if (foundItem.quantity == 0)
            {
                inventory.OnRemoveItem(targetItem.ItemID);
            }
        }

        float craftTime = craftable.CraftTime;

        if (craftTime > 0f)
        {
            float elapsed = 0f;

            while (elapsed < craftTime)
            {
                elapsed += Time.deltaTime;
                float progress = Mathf.Clamp01(elapsed / craftTime);
                EventBus.Publish<float>("CraftingProgressEvent", progress);
                yield return null;
            }
        }

        EventBus.Publish<float>("CraftingProgressEvent", 1f);

        craftable.OnCraftComplete();
        EventBus.Publish<int>("QuestClear", 6);
        Debug.Log("제작 완료: " + ((ScriptableObject)craftable).name);
        inventory.OnAddItem(craftable);
        _isCrafting = false;
    }
}
