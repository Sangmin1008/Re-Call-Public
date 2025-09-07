using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleEnemy : Enemy
{
    protected override void Start()
    {
        base.Start();
        maxHealth = 130f;
        damage = 30f;
    }

}
