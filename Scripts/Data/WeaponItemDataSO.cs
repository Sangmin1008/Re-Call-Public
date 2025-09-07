using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponItemData", menuName = "Scriptable Objects/Item/Weapon Item Data")]
public class WeaponItemDataSO : GenericItemDataSO
{
    [Header("Weapon")]
    [SerializeField] private float damage;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float attackRange;
    [SerializeField] private AudioClip attackSound;

    public float Damage => damage;
    public float AttackSpeed => attackSpeed;
    public float AttackRange => attackRange;
    public AudioClip AttackSound => attackSound;
}
