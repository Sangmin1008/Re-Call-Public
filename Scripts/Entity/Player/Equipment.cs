using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Equipment : MonoBehaviour
{
    private Equip weapon;
    private Equip armor;
    [SerializeField] private Transform equipParent;
    public bool isEquipEnabled = false;
    private SoundManager soundManager;

    private void Start()
    {
        soundManager = SoundManager.Instance;
    }

    private void OnEnable()
    {
        EventBus.Subscribe<GenericItemDataSO>("EquipItem", OnEquipItem);
        EventBus.Subscribe<string>("UnequipItem", OnUnequipItem);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<GenericItemDataSO>("EquipItem", OnEquipItem);
        EventBus.Unsubscribe<string>("UnequipItem", OnUnequipItem);
    }

    public void OnEquipItem(GenericItemDataSO itemDataSO)
    {
        if(itemDataSO == null) return;

        switch(itemDataSO.ItemType)
        {
            case ItemType.Weapon:
                EquipWeapon(itemDataSO);
                break;
            case ItemType.Armor:
                EquipArmor(itemDataSO);
                break;
        }

        if(itemDataSO.UseOrEquipSound != null)
        {
            soundManager.PlaySFX(itemDataSO.UseOrEquipSound);
        }
    }

    private void EquipWeapon(GenericItemDataSO itemDataSO)
    {
        if (itemDataSO == null || itemDataSO.EquipablePrefab == null) return;

        UnequipWeapon();
        GameObject weaponObj = Instantiate(itemDataSO.EquipablePrefab, equipParent);

        weapon = weaponObj.GetComponent<Equip>();
        weapon.OnEquip();
        EventBus.Publish<int>("QuestClear", 3);
        if (weapon != null)
        {
            weapon.Initialize(itemDataSO);
        }
    }

    private void EquipArmor(GenericItemDataSO itemDataSO)
    {
        if (itemDataSO == null || itemDataSO.EquipablePrefab == null) return;

        UnequipArmor();
        GameObject armorObj = Instantiate(itemDataSO.EquipablePrefab, equipParent);
        armor = armorObj.GetComponent<Equip>();
        armor.OnEquip();

        if (armor != null)
        {
            armor.Initialize(itemDataSO);
        }
    }

    public void OnUnequipItem(string itemName)
    {
        if(weapon.GetItemData().UseOrEquipSound != null)
        {
            soundManager.PlaySFX(weapon.GetItemData().UseOrEquipSound);
        }
        // 현재 장착된 장비의 이름과 비교
        if(weapon != null && weapon.GetItemData() != null && weapon.GetItemData().ItemName == itemName)
        {
            UnequipWeapon();
        }
        else if(armor != null && armor.GetItemData() != null && armor.GetItemData().ItemName == itemName)
        {
            UnequipArmor();
        }

    }

    private void UnequipWeapon()
    {
        if(weapon != null)
        {
            var itemDataSO = weapon.GetItemData();
            weapon.OnUnequip();
            Destroy(weapon.gameObject);
            weapon = null;

            if (itemDataSO != null)
            {
                EventBus.Publish("AddItem", itemDataSO);
            }
        }
    }

    private void UnequipArmor()
    {
        if(armor != null)
        {
            var itemDataSO = armor.GetItemData();
            armor.OnUnequip();
            Destroy(armor.gameObject);
            armor = null;

            if (itemDataSO != null)
            {
                EventBus.Publish("AddItem", itemDataSO);
            }
        }
    }

    public Equip GetWeapon() => weapon;
    public Equip GetArmor() => armor;

    public void OnAttackInput()
    {
        if(!isEquipEnabled) return;

        // 무기가 장착되어 있을 때만 공격 가능
        if(weapon != null)
        {
            weapon.OnAttackInput();
        }
        // 맨손 공격 제거 - 무기가 없으면 공격 불가
    }
}