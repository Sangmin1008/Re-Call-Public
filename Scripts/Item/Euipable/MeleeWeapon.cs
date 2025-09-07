using System.Collections;
using UnityEngine;
using DG.Tweening;

public class MeleeWeapon : BaseWeapon
{
    [Header("Melee Animation")]
    private Tween idleTween;
    private Vector3 originalPosition;
    private Vector3 originalRotation;
    private Transform weaponTransform;

    protected override void Start()
    {
        base.Start();

        originalPosition = transform.localPosition;
        originalRotation = transform.localEulerAngles;
        weaponTransform = transform;
    }

    public override void PlayIdleAnimation()
    {
        if (idleTween != null && idleTween.IsActive()) return;

        // Y축으로 0.05만큼 1초에 걸쳐 부드럽게 오르내림
        idleTween = transform.DOLocalMoveY(Constants.WEAPON_IDLE_MOVE_Y, Constants.WEAPON_IDLE_DURATION)
            .SetRelative()
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    public override void StopIdleAnimation()
    {
        if (idleTween != null)
        {
            idleTween.Kill();
            idleTween = null;
            transform.localRotation = Quaternion.identity;
        }
    }

    protected override IEnumerator PerformAttack()
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