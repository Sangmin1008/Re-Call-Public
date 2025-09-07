using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class UIInventory : MonoBehaviour
{
   [Header("Slot")]
   private ItemSlot[] ItemSlots;
   public Transform slotPanel;

   [SerializeField] private ItemSlot weaponSlot;
   [SerializeField] private ItemSlot armorSlot;

   [Header("Selected Item")]
   [SerializeField] private TextMeshProUGUI selectedItemName;
   [SerializeField] private TextMeshProUGUI selectedItemDescription;

   [SerializeField] private GameObject inventoryWindow;

   private ItemInstance _selectedItem;
   private int _selectedItemIndex;

   [SerializeField] private GameObject useButton;
   [SerializeField] private GameObject equipButton;
   [SerializeField] private GameObject unequipButton;
   [SerializeField] private GameObject dropButton;

   [Header("Inventory Reference")]
   public PlayerInventory playerInventory;
   private UIManager uiManager;
   private SoundManager soundManager;

   private void Awake()
   {
       uiManager = UIManager.Instance;
       if(playerInventory == null)
       {
           playerInventory = FindObjectOfType<PlayerInventory>();
       }

       Cursor.lockState = CursorLockMode.Locked;
       ItemSlots = new ItemSlot[slotPanel.childCount];
       for(int i = 0; i < slotPanel.childCount; i++)
       {
           ItemSlots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
           ItemSlots[i].Index = i;
       }
       ClearSelectedWindow();
       inventoryWindow.SetActive(false);
       soundManager = SoundManager.Instance;
   }

   private void OnEnable()
   {
       EventBus.Subscribe<int>("ClickItemSlot", OnClickItemSlot);
       EventBus.Subscribe<string>("ClickEquipmentButton", OnClickEquipmentButton);
       ClearSelectedWindow();
       RefreshUI();
       Cursor.lockState = CursorLockMode.None;
   }

   private void OnDisable()
   {
       EventBus.Unsubscribe<int>("ClickItemSlot", OnClickItemSlot);
       EventBus.Unsubscribe<string>("ClickEquipmentButton", OnClickEquipmentButton);
       ClearSelectedWindow();
       Cursor.lockState = CursorLockMode.Locked;
   }

   private void ClearSelectedWindow()
   {
       selectedItemName.text = "";
       selectedItemDescription.text = "";

       useButton.SetActive(false);
       equipButton.SetActive(false);
       unequipButton.SetActive(false);
       dropButton.SetActive(false);
   }

   void RefreshUI()
   {
       if(playerInventory == null) return;

       var weapon = playerInventory.GetWeapon();
       var armor = playerInventory.GetArmor();

       // 무기 슬롯 갱신
       if(weapon != null)
       {
           var weaponItemData = weapon.GetItemData();
           if(weaponItemData != null)
           {
               weaponSlot.itemData = new ItemInstance(weaponItemData, 1);
               weaponSlot.Set();
           }
       }
       else
       {
           weaponSlot.itemData = null;
           weaponSlot.Clear();
       }

       // 방어구 슬롯 갱신
       if(armor != null)
       {
           var armorItemData = armor.GetItemData();
           if(armorItemData != null)
           {
               armorSlot.itemData = new ItemInstance(armorItemData, 1);
               armorSlot.Set();
           }
       }
       else
       {
           armorSlot.itemData = null;
           armorSlot.Clear();
       }

       // 인벤토리 아이템 슬롯 갱신
       var items = playerInventory.GetItems();
       for(int i = 0; i < ItemSlots.Length; i++)
       {
           if(i < items.Count)
           {
               ItemSlots[i].itemData = items[i];
               ItemSlots[i].Set();
           }
           else
           {
               ItemSlots[i].itemData = null;
               ItemSlots[i].Clear();
           }
       }
   }

   ItemSlot GetItemSlot(ItemInstance item)
   {
       for(int i = 0; i < ItemSlots.Length; i++)
       {
            if(ItemSlots[i].itemData == item)
            {
                var itemDataSO = ItemDatabase.Instance.GetItemByID(item.itemID);
                if(itemDataSO != null && ItemSlots[i].itemData.quantity < itemDataSO.MaxStack)
                {
                    return ItemSlots[i];
                }
            }
        }
        return null;
    }

    public void OnClickItemSlot(int slotIndex)
    {
        if(slotIndex < 0 || slotIndex >= ItemSlots.Length) return;

        ItemSlot clickedSlot = ItemSlots[slotIndex];
        if(clickedSlot.itemData == null) return;

        _selectedItem = clickedSlot.itemData;
        _selectedItemIndex = slotIndex;

        var itemDataSO = ItemDatabase.Instance.GetItemByID(_selectedItem.itemID);
        if(itemDataSO == null) return;

        selectedItemName.text = itemDataSO.ItemName;
        selectedItemDescription.text = itemDataSO.Description;

        useButton.SetActive(itemDataSO.ItemType == ItemType.Consumable);
        equipButton.SetActive(itemDataSO.ItemType == ItemType.Weapon || itemDataSO.ItemType == ItemType.Armor);
        unequipButton.SetActive(false);
        dropButton.SetActive(true);
        soundManager.PlaySFX(SFXType.SelectItem);
    }

    public void OnClickEquipmentButton(string itemID)
    {
        var itemDataSO = ItemDatabase.Instance.GetItemByID(itemID);
        if(itemDataSO == null) return;

        selectedItemName.text = itemDataSO.ItemName;
        selectedItemDescription.text = itemDataSO.Description;

        useButton.SetActive(false);
        equipButton.SetActive(false);
        unequipButton.SetActive(true);
        dropButton.SetActive(false);
    }

    public void OnUseButton()
    {
        if(_selectedItem == null) return;
        EventBus.Publish("UseItem", _selectedItem.itemID);
        RefreshUI();
    }

    public void OnEquipButton()
    {
        if(_selectedItem == null) return;

        var itemDataSO = ItemDatabase.Instance.GetItemByID(_selectedItem.itemID);
        if(itemDataSO == null) return;

        EventBus.Publish("EquipItem", itemDataSO);
        EventBus.Publish("RemoveItem", _selectedItem.itemID);
        RefreshUI();
    }

    public void OnUnequipButton()
    {
        EventBus.Publish("UnequipItem", selectedItemName.text);
        RefreshUI();
    }

    public void OnDropButton()
    {
        if(_selectedItem == null) return;

        EventBus.Publish("DropItem", _selectedItem.itemID);
        ClearSelectedWindow();
        RefreshUI();
    }
}
