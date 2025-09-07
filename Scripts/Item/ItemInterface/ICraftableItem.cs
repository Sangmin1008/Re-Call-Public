using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICraftableItem
{
    GenericItemDataSO.RecipeEntry[] RecipeEntries { get; }
    float CraftTime { get; }
    void OnCraftComplete();
}
