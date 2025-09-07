using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType   { Resource, Consumable, Weapon, Armor, Tool, Structure }
[CreateAssetMenu(fileName = "NewGenericItemData", menuName = "Scriptable Objects/Item/Generic Item Data")]
public class GenericItemDataSO : ScriptableObject
{
    [Header("Identity")]
    [SerializeField] private string itemID;
    [SerializeField] private string itemName;
    [SerializeField] [TextArea] private string description;
    [SerializeField] private Sprite icon;

    [Header("Classification")]
    [SerializeField] private ItemType type;
    [SerializeField] private int maxStack = 1;
    [SerializeField] private List<bool> loop = new List<bool> { true, true, true, true, true, };

    [Header("Prefab")]
    [SerializeField] protected GameObject prefab;
    [SerializeField] protected GameObject equipablePrefab;

    [Header("Trait")]
    [SerializeField] private List<ScriptableObject> traits;

    [Header("Sound")]
    [SerializeField] private AudioClip useOrEquipSound;

    [System.Serializable]
    public struct RecipeEntry
    {
        public GenericItemDataSO ingredient;
        public int quantity;
    }

    public string ItemID => itemID;
    public string ItemName => itemName;
    public string Description => description;
    public Sprite Icon => icon;
    public ItemType ItemType => type;
    public List<bool> Loop => loop;
    public GameObject WorldPickupPrefab => prefab;
    public GameObject EquipablePrefab => equipablePrefab;
    public int MaxStack => maxStack;
    public AudioClip UseOrEquipSound => useOrEquipSound;

    public void UseItem()
    {
        foreach (var trait in traits)
        {
            if (trait is IItemTrait itemTrait)
            {
                itemTrait.Apply();
            }
        }
    }
}
