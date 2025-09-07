using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CraftingTableUI : MonoBehaviour
{
    [SerializeField] private GameObject craftingWindow;
    [SerializeField] private Transform recipesPanel;
    [SerializeField] private GameObject recipeButtonPrefab;
    [SerializeField] private GameObject table;

    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemDescriptionText;
    [SerializeField] private Button craftButton;

    [SerializeField] private GameObject recipeSlotPanel;
    [SerializeField] private GameObject recipeSlotPrefab;

    [SerializeField] private TextMeshProUGUI craftStateText;
    [SerializeField] private Slider progressBar;

    [SerializeField] private CraftingSystem craftingSystem;

    private bool isOpen = false;

    private void OnEnable()
    {
        EventBus.SubscribeVoid("CraftingTableEvent", OpenUI);
        EventBus.Subscribe<bool>("CraftableItemEvent", IsCraftable);
        EventBus.SubscribeVoid("ItemCraftCompleteEvent", OnCraftComplete);
        EventBus.Subscribe<float>("CraftingProgressEvent", UpdateCraftingSlider);
    }

    private void OnDisable()
    {
        EventBus.UnsubscribeVoid("CraftingTableEvent", OpenUI);
        EventBus.Unsubscribe<bool>("CraftableItemEvent", IsCraftable);
        EventBus.UnsubscribeVoid("ItemCraftCompleteEvent", OnCraftComplete);
        EventBus.Unsubscribe<float>("CraftingProgressEvent", UpdateCraftingSlider);
    }

    private void UpdateCraftingSlider(float progress)
    {
        progressBar.gameObject.SetActive(true);
        progressBar.value = progress;

        if (Mathf.Approximately(progress, 1f))
        {
            StartCoroutine(HideSliderAfterDelay());
        }
    }

    private IEnumerator HideSliderAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        progressBar.gameObject.SetActive(false);
    }

    public void SetOffSlider()
    {
        progressBar.gameObject.SetActive(false);
    }

    private void OnCraftComplete()
    {
        craftStateText.text = "Done!";
    }

    private void Update()
    {
        if (isOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseUI();
        }
    }

    private void OpenUI()
    {
        PopulateRecipeButtons();
        //craftingWindow.SetActive(true);
        isOpen = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void CloseUI()
    {
        craftingWindow.SetActive(false);
        isOpen = false;
        EventBus.Publish<bool>("OpenInventory", false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void PopulateRecipeButtons()
    {
        foreach (Transform child in recipesPanel)
        {
            Destroy(child.gameObject);
        }

        foreach (var recipe in RecipeBook.Instance.KnownRecipes)
        {
            GameObject buttonObj = Instantiate(recipeButtonPrefab, recipesPanel);
            Button button = buttonObj.GetComponent<Button>();
            TextMeshProUGUI buttonText = buttonObj.GetComponentInChildren<TextMeshProUGUI>();

            if (buttonText != null)
            {
                buttonText.text = recipe.GetRecipeName();
            }

            var currentRecipe = recipe;
            button.onClick.AddListener(() =>
            {
                foreach (Transform child in recipeSlotPanel.transform)
                {
                    Destroy(child.gameObject);
                }

                Debug.Log("선택된 레시피: " + currentRecipe.GetRecipeName());

                table.SetActive(true);
                craftStateText.gameObject.SetActive(false);
                itemNameText.text = currentRecipe.GetRecipeName();
                itemDescriptionText.text = currentRecipe.GetRecipeDescription();

                craftButton.onClick.RemoveAllListeners();
                craftButton.onClick.AddListener(() =>
                {
                    craftStateText.gameObject.SetActive(true);
                    Debug.Log("Craft 버튼 눌림!");
                    if (!craftingSystem.IsCrafting)
                        EventBus.Publish<CraftableItemDataSO>("CraftItemEvent", currentRecipe.itemData);
                });

                foreach (var recipeEntry in currentRecipe.itemData.RecipeEntries)
                {
                    GameObject slot = Instantiate(recipeSlotPrefab, recipeSlotPanel.transform);

                    TextMeshProUGUI slotText = slot.GetComponentInChildren<TextMeshProUGUI>();
                    if (slotText != null)
                    {
                        slotText.text = recipeEntry.quantity.ToString();
                    }

                    Transform iconTransform = slot.transform.Find("Icon");
                    if (iconTransform != null)
                    {
                        Image icon = iconTransform.GetComponent<Image>();
                        if (icon != null)
                        {
                            icon.sprite = recipeEntry.ingredient.Icon;
                        }
                    }
                }
            });
        }
    }

    private void IsCraftable(bool result)
    {
        if (result) craftStateText.text = "Please wait a moment";
        else craftStateText.text = "Items that cannot be crafted";
    }
}
