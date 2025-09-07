using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Linq;

public class PlayerInventory : MonoBehaviour
{
    public List<ItemInstance> Items = new List<ItemInstance>();
    private Equipment equipment;
    private UIManager uiManager;
    private bool isOpen = false;
    [SerializeField] private Transform equipParent;
    [SerializeField] private Vector3 footPosition;
    private SoundManager soundManager;
    private ItemDatabase database;

    public void Start()
    {
        //equipParent.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z + 0.6f);
        footPosition = new Vector3(transform.position.x, transform.position.y -1f, transform.position.z);
        uiManager = UIManager.Instance;
        soundManager = SoundManager.Instance;
        database = ItemDatabase.Instance;
        if(equipment == null)
        {
            equipment = GetComponent<Equipment>();
        }

        uiManager.ShowUI("InventoryUI");
        uiManager.HideUI("InventoryUI");
    }

    public void OnEnable()
    {
        EventBus.Subscribe<GenericItemDataSO>("AddItem", OnAddItem);
        EventBus.Subscribe<string>("UseItem", OnUseItem);
        EventBus.Subscribe<string>("DropItem", OnDropItem);
        EventBus.Subscribe<string>("RemoveItem", OnRemoveItem);
    }

    public void OnDisable()
    {
        EventBus.Unsubscribe<GenericItemDataSO>("AddItem", OnAddItem);
        EventBus.Unsubscribe<string>("UseItem", OnUseItem);
        EventBus.Unsubscribe<string>("DropItem", OnDropItem);
        EventBus.Unsubscribe<string>("RemoveItem", OnRemoveItem);
    }




    public void OnAddItem(GenericItemDataSO itemDataSO)
    {
        if(itemDataSO == null) return;

        // 스택 가능한 아이템인 경우 기존 아이템과 합치기 시도
        if(itemDataSO.MaxStack > 1)
        {
            var existingItem = Items.FirstOrDefault(item => item.itemID == itemDataSO.ItemID);
            if(existingItem != null && existingItem.quantity < itemDataSO.MaxStack)
            {
                existingItem.AddQuantity(1);
                return;
            }
        }

        // 새 아이템 추가
        Items.Add(new ItemInstance(itemDataSO, 1));
    }

    public void OnRemoveItem(string itemID)
    {
        var item = Items.FirstOrDefault(i => i.itemID == itemID);
        if(item != null)
        {
            Items.Remove(item);
        }
    }

    public void OnUseItem(string itemID)
    {
        var item = Items.FirstOrDefault(i => i.itemID == itemID);
        if(item != null)
        {
            var itemDataSO = ItemDatabase.Instance.GetItemByID(item.itemID);
            if(itemDataSO != null)
            {
                itemDataSO.UseItem();
                item.SubtractQuantity(1);
                soundManager.PlaySFX(itemDataSO.UseOrEquipSound);

                if(item.IsEmpty())
                {
                    Items.Remove(item);
                }
            }
        }
    }

    public void OnDropItem(string itemID)
    {
        var item = Items.FirstOrDefault(i => i.itemID == itemID);
        if(item != null)
        {
            GenericItemDataSO itemDataSO = ItemDatabase.Instance.GetItemByID(item.itemID);
            if(itemDataSO != null)
            {
                Vector3 dropPosition = transform.position + transform.up * -1f + transform.forward * 0.5f;

                // 월드에 아이템 오브젝트 생성
                database.SpawnItem(dropPosition, itemDataSO);

                var dropData = new DroppedItemData(item.itemID, dropPosition);
                EventBus.Publish<DroppedItemData>("DropItem", dropData);

                item.SubtractQuantity(1);
                if(item.IsEmpty())
                {
                    Items.Remove(item);
                }
            }
        }
    }

    public List<ItemInstance> GetItems()
    {
        return Items;
    }

    public GenericItemDataSO GetItemDataSO(string itemID)
    {
        return ItemDatabase.Instance.GetItemByID(itemID);
    }

    public Equip GetWeapon()
    {
        return equipment.GetWeapon();
    }

    public Equip GetArmor()
    {
        return equipment.GetArmor();
    }

    public void OnInventory(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            if(isOpen)
            {
                CloseInventory();
            }
            else
            {
                OpenInventory();
            }
        }
    }

    public void OnCloseInventory(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            if(isOpen)
            {
                CloseInventory();
            }
        }
    }

    public void OpenInventory()
    {
        isOpen = true;
        uiManager.ShowUI("InventoryUI");
        EventBus.Publish("OpenInventory", isOpen);
    }

    public void CloseInventory()
    {
        isOpen = false;
        uiManager.HideUI("InventoryUI");
        EventBus.Publish("OpenInventory", isOpen);
    }

    public void OnAttackInput(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            equipment.OnAttackInput();
        }
    }

    public bool HasItem(GenericItemDataSO itemDataSO)
    {
        return Items.Any(item => item.itemID == itemDataSO.ItemID);
    }
}