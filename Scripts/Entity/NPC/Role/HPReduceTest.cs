using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPReduceTest : MonoBehaviour, INPCRole
{
    public void Role()
    {
        EventBus.Publish("HPSubtractEvent", 20f);
        //EventBus.Publish("HitEffectEvent", null);
    }


}
