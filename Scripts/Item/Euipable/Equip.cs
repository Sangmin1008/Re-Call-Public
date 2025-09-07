using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equip : MonoBehaviour
{
    protected GenericItemDataSO itemData;

    protected virtual void Start()
    {
    }

    public virtual void OnEquip()
    {
    }

    public virtual void OnUnequip()
    {
    }

    public virtual void OnHit()
    {
    }

    public virtual void Initialize(GenericItemDataSO itemData)
    {
        this.itemData = itemData;
    }
    public virtual void OnAttackInput()
    {

    }
    public GenericItemDataSO GetItemData() => itemData;
}
