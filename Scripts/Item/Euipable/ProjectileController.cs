// Trap.cs
using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
public class ProjectileController : MonoBehaviour
{
    [SerializeField] private float shotPower;
    [SerializeField] private LayerMask _levelColliderLayer;
    [SerializeField] private LayerMask _targetLayer;
    [SerializeField] private GenericItemDataSO itemData;
    [SerializeField] private Rigidbody rigidbody;
    [SerializeField] private float lifeTime = 5f; // 발사체 생존 시간 (5초)

    private float damage;
    private float attackRange;
    private Vector3 startPosition;

    public void Init()
    {
        if(rigidbody == null)
        {
            rigidbody = GetComponent<Rigidbody>();
        }
        damage = (itemData as WeaponItemDataSO).Damage;
        attackRange = (itemData as WeaponItemDataSO).AttackRange;
        startPosition = transform.position;
        ShootProjectile();
    }

    private void ShootProjectile()
    {
        // 화면 중앙 방향으로 발사 (카메라가 바라보는 방향)
        rigidbody.AddForce(transform.forward * shotPower);

        float elapsedTime = 0f;

        while (elapsedTime < lifeTime)
        {
            // 발사체가 최대 사거리를 넘었는지 확인
            float traveledDistance = Vector3.Distance(startPosition, transform.position);
            if (traveledDistance >= attackRange)
            {
                break;
            }

            elapsedTime += Time.deltaTime;
        }

        ProjectilePool.Instance.ReturnProjectile(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(_levelColliderLayer.value == (_levelColliderLayer.value | (1 << other.gameObject.layer)))
        {
            ProjectilePool.Instance.ReturnProjectile(this);
        }
        if(_targetLayer.value == (_targetLayer.value | (1 << other.gameObject.layer)))
        {
            if(other.gameObject.TryGetComponent(out IDamageable damageable))
            {
                damageable.TakePhysicalDamage(damage);
            }
            ProjectilePool.Instance.ReturnProjectile(this);
        }
    }
}