using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipeData", menuName = "Scriptable Objects/Recipe/Recipe Data")]
public class RecipeDataSO : ScriptableObject
{
    public CraftableItemDataSO itemData;

    public string GetRecipeName() => itemData.ItemName;
    public string GetRecipeDescription() => itemData.Description;
}
