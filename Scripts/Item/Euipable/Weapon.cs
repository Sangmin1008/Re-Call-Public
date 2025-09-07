using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Runtime.CompilerServices;

public class Weapon : Equip
{
    private Tween idleTween;
    private Vector3 originalPosition;
    private Vector3 originalRotation;
    private float attackDuration;
    private float attackRange;
    private float damage;

    private Transform weaponTransform;
    private PlayerStat playerStat;
    private bool isAttacking = false;
    private Camera camera;
    private SoundManager soundManager;

    void Start()
    {
        originalPosition = transform.localPosition;
        originalRotation = transform.localEulerAngles;
        weaponTransform = transform;

        playerStat = GetComponentInParent<PlayerStat>();
        camera = Camera.main;

        attackDuration = (itemData as WeaponItemDataSO).AttackSpeed;
        damage = (itemData as WeaponItemDataSO).Damage;
        attackRange = (itemData as WeaponItemDataSO).AttackRange;
        soundManager = SoundManager.Instance;
    }

    public override void OnEquip()
    {
        PlayIdleAnimation();
        EventBus.SubscribeVoid("AttackInput", OnAttackInput);

    }

    public override void OnUnequip()
    {
        StopIdleAnimation();
        EventBus.UnsubscribeVoid("AttackInput", OnAttackInput);
    }

    public override void OnHit()
    {
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, attackRange))
        {
            if(itemData.ItemType == ItemType.Weapon && hit.collider.TryGetComponent(out IDamageable damageable))
            {
                if (playerStat != null)
                {
                    damage = playerStat.GetTotalAttackDamage();
                }
                damageable.TakePhysicalDamage(damage);
            }
        }
    }

    public void PlayIdleAnimation()
    {
        if (idleTween != null && idleTween.IsActive()) return;

        // Y축으로 0.05만큼 1초에 걸쳐 부드럽게 오르내림
        idleTween = transform.DOLocalMoveY(Constants.WEAPON_IDLE_MOVE_Y, Constants.WEAPON_IDLE_DURATION)
            .SetRelative()
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    public void StopIdleAnimation()
    {
        if (idleTween != null)
        {
            idleTween.Kill();
            idleTween = null;
            transform.localRotation = Quaternion.identity;
        }
    }

    public override void OnAttackInput()
    {
        if (isAttacking) return;
        if((itemData as WeaponItemDataSO).AttackSound != null)
        {
            soundManager.PlaySFX((itemData as WeaponItemDataSO).AttackSound);
        }
        StartCoroutine(OverheadAttack());
    }

    IEnumerator OverheadAttack()
    {
        isAttacking = true;
        StopIdleAnimation();

        // 준비자세
        Vector3 windupRotation = originalRotation + new Vector3(Constants.WINDUP_ROTATION_X, 0, 0);
        Vector3 windupPosition = originalPosition + new Vector3(0, Constants.WINDUP_POSITION_Y, Constants.WINDUP_POSITION_Z);

        var windupSequence = DOTween.Sequence();
        windupSequence.Append(weaponTransform.DOLocalRotate(windupRotation, attackDuration * Constants.WINDUP_DURATION_MULTIPLIER)
            .SetEase(Ease.OutBack));
        windupSequence.Join(weaponTransform.DOLocalMove(windupPosition, attackDuration * Constants.WINDUP_DURATION_MULTIPLIER)
            .SetEase(Ease.OutBack));

        yield return new WaitForSeconds(attackDuration * 0.3f);

        // 공격 모션
        Vector3 smashRotation = originalRotation + new Vector3(Constants.SMASH_ROTATION_X, 0, Constants.SMASH_ROTATION_Z);
        Vector3 smashPosition = originalPosition + new Vector3(-originalPosition.x, Constants.SMASH_POSITION_Y, Constants.SMASH_POSITION_Z);

        var smashSequence = DOTween.Sequence();
        smashSequence.Append(weaponTransform.DOLocalRotate(smashRotation, attackDuration * Constants.SMASH_DURATION_MULTIPLIER)
            .SetEase(Ease.InQuart));
        smashSequence.Join(weaponTransform.DOLocalMove(smashPosition, attackDuration * Constants.SMASH_DURATION_MULTIPLIER)
            .SetEase(Ease.InQuart));

        OnHit();

        yield return new WaitForSeconds(attackDuration * Constants.SMASH_DURATION_MULTIPLIER);

        // 복귀
        weaponTransform.DOLocalRotate(originalRotation, attackDuration * Constants.RECOVERY_DURATION_MULTIPLIER)
            .SetEase(Ease.InQuart);
        weaponTransform.DOLocalMove(originalPosition, attackDuration * Constants.RECOVERY_DURATION_MULTIPLIER)
            .SetEase(Ease.InQuart);

        yield return new WaitForSeconds(attackDuration * Constants.RECOVERY_DURATION_MULTIPLIER);

        PlayIdleAnimation();
        isAttacking = false;
    }

}