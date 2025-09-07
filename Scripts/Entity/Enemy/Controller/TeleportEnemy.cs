using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportEnemy : Enemy
{
    protected override void Start()
    {
        base.Start();
        maxHealth = 100f;
        damage = 15f;
    }

}
