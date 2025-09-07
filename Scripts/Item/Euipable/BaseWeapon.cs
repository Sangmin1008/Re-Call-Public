using System.Collections;
using UnityEngine;

public abstract class BaseWeapon : Equip
{
    [Header("Base Weapon Settings")]
    protected float attackDuration;
    protected float attackRange;
    protected float damage;

    protected PlayerStat playerStat;
    protected bool isAttacking = false;
    protected Camera camera;
    protected SoundManager soundManager;

    protected virtual void Start()
    {
        playerStat = GetComponentInParent<PlayerStat>();
        camera = Camera.main;
        soundManager = SoundManager.Instance;

        // WeaponItemDataSO에서 데이터 가져오기
        var weaponData = itemData as WeaponItemDataSO;
        if (weaponData != null)
        {
            attackDuration = weaponData.AttackSpeed;
            damage = weaponData.Damage;
            attackRange = weaponData.AttackRange;
        }
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
                float finalDamage = damage;
                if (playerStat != null)
                {
                    finalDamage = playerStat.GetTotalAttackDamage();
                }
                damageable.TakePhysicalDamage(finalDamage);
            }
        }
    }

    public override void OnAttackInput()
    {
        if (isAttacking) return;

        // 공격 사운드 재생
        var weaponData = itemData as WeaponItemDataSO;
        if(weaponData?.AttackSound != null)
        {
            soundManager.PlaySFX(weaponData.AttackSound);
        }

        StartCoroutine(PerformAttack());
    }

    // 하위 클래스에서 구현해야 하는 추상 메서드들
    public abstract void PlayIdleAnimation();
    public abstract void StopIdleAnimation();
    protected abstract IEnumerator PerformAttack();
}