using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal.Internal;

public class ReturnPlayer : MonoBehaviour, INPCRole
{
    public GameObject target;
    Vector3 spawnPos;

    public float duration = 4f;
    void Awake()
    {
        spawnPos = Camera.main.transform.position;
            //target.transform.position;
    }


    public void LoopPlayer()
    {
        EventBus.Publish("HPSubtractEvent", 200f);

        /*
        Camera.main.transform.DOMove(spawnPos, duration).SetEase(Ease.InOutQuad);
        EventBus.Publish("FadeEvent", null);
        EventBus.Publish("VerticalWipeEvent", null);
        */
    }

    public void Role()
    {
        LoopPlayer();
    }
}
