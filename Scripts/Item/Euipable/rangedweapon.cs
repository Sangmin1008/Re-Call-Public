using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Runtime.CompilerServices;


public class RangedWeapon : BaseWeapon
{
    [Header("Ranged Weapon Settings")]
    [SerializeField] private GameObject muzzleFlashPrefab;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private float destroyTimer;
    [SerializeField] private LayerMask levelColliderLayer;
    private Animator gunAnimator;

    public override void OnEquip()
    {
        base.OnEquip();
    }

    public override void OnUnequip()
    {
        base.OnUnequip();
    }

    protected override void Start()
    {
        base.Start();
        gunAnimator = GetComponentInChildren<Animator>();
    }

    public override void PlayIdleAnimation()
    {
        // 원거리 무기는 특별한 idle 애니메이션 없음
    }

    public override void StopIdleAnimation()
    {
        // 원거리 무기는 특별한 idle 애니메이션 없음
    }

    public void ShakeCamera()
    {
        var cameraTransform = camera.transform;
        // duration, strength, vibrato, randomness, snapping, fadeout
        cameraTransform.DOShakePosition(0.1f, 0.3f, 10, 90f, false, true);
    }

    protected override IEnumerator PerformAttack()
    {
        isAttacking = true;

        // 머즐 플래시 효과
        if(muzzleFlashPrefab != null)
        {
            GameObject tempFlash;
            tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation);
            Destroy(tempFlash, destroyTimer);
        }

        // 발사체 생성 및 발사
        if (projectilePrefab != null)
        {
            // 화면 중앙에서 레이캐스트로 목표 지점 계산
            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            Ray centerRay = camera.ScreenPointToRay(screenCenter);

            Vector3 targetPoint;
            RaycastHit hit;

            // 레이캐스트로 실제 충돌 지점 찾기
            if (Physics.Raycast(centerRay, out hit, attackRange, levelColliderLayer))
            {
                targetPoint = hit.point; // 충돌 지점
                Debug.Log($"충돌 감지: {hit.collider.name} at {hit.point}");
                Debug.DrawRay(centerRay.origin, centerRay.direction * hit.distance, Color.red, 2f);
            }
            else
            {
                targetPoint = centerRay.GetPoint(attackRange); // 충돌하지 않으면 최대 사거리 지점
                Debug.Log($"충돌 없음, 최대 사거리 지점: {targetPoint}");
                Debug.DrawRay(centerRay.origin, centerRay.direction * attackRange, Color.yellow, 2f);
            }

            // 총구에서 목표 지점으로의 방향 시각화
            Debug.DrawLine(barrelLocation.position, targetPoint, Color.green, 2f);
            Debug.Log($"총구 위치: {barrelLocation.position}, 목표 지점: {targetPoint}");

            // 총구에서 목표 지점으로의 방향 계산
            Vector3 shootDirection = (targetPoint - barrelLocation.position).normalized;

            // 총구 위치에서 발사체 생성, 목표 방향으로 회전
            Quaternion shootRotation = Quaternion.LookRotation(shootDirection);
            ProjectileController projectile = ProjectilePool.Instance.GetProjectile(0, barrelLocation.position, shootRotation);
            projectile.Init();
        }

        OnHit();

        // 총 애니메이션 재생
        if (gunAnimator != null)
        {
            gunAnimator.speed = 1 / attackDuration;
            gunAnimator.SetTrigger("Fire");
        }

        ShakeCamera();

        yield return new WaitForSeconds(attackDuration);
        isAttacking = false;
    }
}