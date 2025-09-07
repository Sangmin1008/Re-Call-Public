using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Runtime.CompilerServices;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float damage;

    public void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out IDamageable damageable))
        {
            damageable.TakePhysicalDamage(damage);
        }
    }
}