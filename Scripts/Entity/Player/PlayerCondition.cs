using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ConditionHandler;

public class PlayerCondition : MonoBehaviour, IDamageable
{
    [SerializeField]
    private Condition health;
    [SerializeField]
    private Condition stamina;
    [SerializeField]
    private Condition hunger;

    private PlayerStat playerStat;
    private PlayerMovement playerMovement;
    private PlayerSensor playerSensor;
    private bool isDead = false;
    private float updateConditionInterval = 0.5f;
    private float lastUpdateConditionTime = 0f;
    private SoundManager soundManager;

    [Header("Stamina Settings")]
    public float jumpStaminaCost = Constants.JUMP_STAMINA_CONSUMPTION;
    private float lastStaminaActionTime = 0f;
    private float staminaRecoveryDelay = Constants.JUMP_STAMINA_RECOVERY_DELAY;

    public Condition Health => health;
    public Condition Stamina => stamina;
    public Condition Hunger => hunger;

    private void Awake()
    {
        //EventBus.Subscribe<ConditionList>("InitConditionEvent", InitCondition);
    }

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerStat = GetComponent<PlayerStat>();
        playerSensor = GetComponent<PlayerSensor>();
        soundManager = SoundManager.Instance;
    }

    public void Update()
    {
        if(Time.time - lastUpdateConditionTime >= updateConditionInterval)
        {
            lastUpdateConditionTime = Time.time;

            // 스태미나 관리
            HandleStaminaManagement();

            // 배고픔 관리
            EventBus.Publish("HungerSubtractWithoutEffectEvent", hunger.passiveValue * updateConditionInterval);
            if(hunger.curValue <= 0)
            {
                EventBus.Publish("HPSubtractWithoutEffectEvent", health.passiveValue * updateConditionInterval);
            }
        }

        if(health.curValue <= 0 && !isDead)
        {
            isDead = true;
            EventBus.PublishVoid("Dead");
        }
    }

    private void HandleStaminaManagement()
    {
        bool isRunning = playerMovement.IsRunning;
        bool isGrounded = playerSensor.CheckGrounded();
        float currentTime = Time.time;

        if (isRunning)
        {
            // 달리고 있으면 스태미나 소비
            float staminaCost = stamina.passiveValue * updateConditionInterval;
            EventBus.Publish("StaminaSubtractWithoutEffectEvent", staminaCost);
            lastStaminaActionTime = currentTime;
        }
        else if (isGrounded && currentTime - lastStaminaActionTime >= staminaRecoveryDelay)
        {
            // 땅에 있고, 마지막 액션 후 충분한 시간이 지났으면 회복
            float staminaRecovery = stamina.passiveValue * updateConditionInterval;
            EventBus.Publish("StaminaAddWithoutEffectEvent", staminaRecovery);
        }
    }

    public void InitCondition(ConditionList conditions)
    {
        health = conditions.health;
        stamina = conditions.stamina;
        hunger = conditions.hunger;
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<ConditionList>("InitConditionEvent", InitCondition);
    }

    public void Heal(float heal)
    {
        EventBus.Publish("HPAddEvent", heal);
    }

    public void Eat(float amount)
    {
        EventBus.Publish("HungerAddEvent", amount);
    }

    public void UseStaminaForJump()
    {
        if (stamina.curValue >= jumpStaminaCost)
        {
            lastStaminaActionTime = Time.time;
            EventBus.Publish("StaminaSubtractEvent", jumpStaminaCost);
        }
    }

    public bool CanJump()
    {
        return stamina.curValue >= jumpStaminaCost;
    }

    public bool CanRun()
    {
        return stamina.curValue > 0;
    }

    public void TakePhysicalDamage(float damage)
    {
        float realDamage = damage;
        EventBus.PublishVoid("HitEffectEvent");
        // PlayerStats가 있으면 방어력 적용
        if (playerStat != null)
        {
            realDamage = playerStat.CalculateDamage(damage);
        }
        EventBus.Publish("HPSubtractEvent", realDamage);
        soundManager.PlaySFX(SFXType.TakeDamage);
        EventBus.Publish("HitEffectEvent", null);
    }
}