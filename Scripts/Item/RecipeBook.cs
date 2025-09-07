using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeBook : Singleton<RecipeBook>
{
    public List<RecipeDataSO> KnownRecipes = new List<RecipeDataSO>();

    private void OnEnable()
    {
        EventBus.Subscribe<RecipeDataSO>("GetRecipeEvent", AddRecipe);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<RecipeDataSO>("GetRecipeEvent", AddRecipe);
    }

    private void AddRecipe(RecipeDataSO recipe)
    {
        if (!KnownRecipes.Contains(recipe))
        {
            KnownRecipes.Add(recipe);
            Debug.Log("레시피 습득: " + recipe.name);
        }
    }

    public bool HasRecipe(RecipeDataSO recipe) => KnownRecipes.Contains(recipe);
}
