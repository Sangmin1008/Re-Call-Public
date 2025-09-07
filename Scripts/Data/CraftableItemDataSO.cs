using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCraftableItemData", menuName = "Scriptable Objects/Item/Craftable Item Data")]
public class CraftableItemDataSO : GenericItemDataSO, ICraftableItem
{
    [Header("Crafting")]
    [SerializeField] private RecipeEntry[] recipeEntries;
    [SerializeField] private float craftTime;
    public RecipeEntry[] RecipeEntries => recipeEntries;
    public float CraftTime => craftTime;
    public void OnCraftComplete()
    {
        EventBus.PublishVoid("ItemCraftCompleteEvent");
    }
}
