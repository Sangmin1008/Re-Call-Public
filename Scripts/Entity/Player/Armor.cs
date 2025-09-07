using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Armor : Equipment
{

    void Start()
    {

    }

    public void OnEquip()
    {
        // PlayIdleAnimation();
        // EventBus.SubscribeVoid("AttackInput", OnAttackInput);

    }

    public void OnUnequip()
    {
        // StopIdleAnimation();
        // EventBus.UnsubscribeVoid("AttackInput", OnAttackInput);
    }
}