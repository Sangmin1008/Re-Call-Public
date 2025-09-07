using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldRecipePickup : MonoBehaviour, IInteractable
{
    [SerializeField] private RecipeDataSO recipeData;
    public void OnInteract()
    {
        Debug.Log("레시피 습득");
        EventBus.Publish<RecipeDataSO>("GetRecipeEvent", recipeData);
        Destroy(gameObject);
    }

    public string GetInteractText() => $"[E] 키를 누르세요: {recipeData.GetRecipeName()}";
}
