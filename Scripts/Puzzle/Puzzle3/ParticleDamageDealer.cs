using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDamageDealer : MonoBehaviour
{
    public float damageAmount = 10f;

    private void OnParticleCollision(GameObject other)
    {
        Debug.Log($"[파티클 충돌] 대상: {other.name}");

        // 충돌한 대상에 데미지 주기
        EventBus.Publish("PlayerTakeDamage", damageAmount);
    }
}