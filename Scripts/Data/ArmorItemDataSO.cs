using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewArmorItemData", menuName = "Scriptable Objects/Item/Armor Item Data")]
public class ArmorItemDataSO : GenericItemDataSO
{
    [SerializeField] private float defense;

    public float Defense => defense;
}
