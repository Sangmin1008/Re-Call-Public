using UnityEngine;

public class PlayerStat : MonoBehaviour
{
    [Header("Base Stats")]
    [SerializeField] private float baseAttackDamage = 5f;
    [SerializeField] private float baseDefense = 0;

    private Equipment equipment;

    private void Awake()
    {
        equipment = GetComponent<Equipment>();
    }

    public float GetTotalAttackDamage()
    {
        float totalDamage = baseAttackDamage;

        var weapon = equipment.GetWeapon();
        if (weapon != null)
        {
            var weaponData = weapon.GetItemData() as WeaponItemDataSO;
            if (weaponData != null)
            {
                totalDamage += weaponData.Damage;
            }
        }

        return totalDamage;
    }

    public float GetTotalDefense()
    {
        float totalDefense = baseDefense;

        var armor = equipment.GetArmor();
        if (armor != null)
        {
            var armorData = armor.GetItemData() as ArmorItemDataSO;
            if (armorData != null)
            {
                totalDefense += armorData.Defense;
            }
        }

        return totalDefense;
    }
    public float CalculateDamage(float damage)
    {
        return damage - GetTotalDefense();
    }
}