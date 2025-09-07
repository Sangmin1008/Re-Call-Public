using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class ConditionHandler : MonoBehaviour
{
    public Condition health;
    public Condition hunger;
    public Condition stamina;

    public ConditionList GetConditionList()
    {
        return new ConditionList
        {
            health = this.health,
            hunger = this.hunger,
            stamina = this.stamina
        };
    }


    private void OnEnable()
    {
        EventBus.Subscribe<float>("HPAddEvent", health.Add);
        EventBus.Subscribe<float>("HPSubtractEvent", health.Subtract);

        EventBus.Subscribe<float>("HungerAddEvent", hunger.Add);
        EventBus.Subscribe<float>("HungerSubtractEvent", hunger.Subtract);

        EventBus.Subscribe<float>("StaminaAddEvent", stamina.Add);
        EventBus.Subscribe<float>("StaminaSubtractEvent", stamina.Subtract);

        EventBus.Subscribe<float>("HPAddWithoutEffectEvent", health.AddWithoutEffect);
        EventBus.Subscribe<float>("HPSubtractWithoutEffectEvent", health.SubtractWithoutEffect);

        EventBus.Subscribe<float>("HungerAddWithoutEffectEvent", hunger.AddWithoutEffect);
        EventBus.Subscribe<float>("HungerSubtractWithoutEffectEvent", hunger.SubtractWithoutEffect);

        EventBus.Subscribe<float>("StaminaAddWithoutEffectEvent", stamina.AddWithoutEffect);
        EventBus.Subscribe<float>("StaminaSubtractWithoutEffectEvent", stamina.SubtractWithoutEffect);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<float>("HPAddEvent", health.Add);
        EventBus.Unsubscribe<float>("HPSubtractEvent", health.Subtract);

        EventBus.Unsubscribe<float>("HungerAddEvent", hunger.Add);
        EventBus.Unsubscribe<float>("HungerSubtractEvent", hunger.Subtract);

        EventBus.Unsubscribe<float>("StaminaAddEvent", stamina.Add);
        EventBus.Unsubscribe<float>("StaminaSubtractEvent", stamina.Subtract);

        EventBus.Unsubscribe<float>("HPAddWithoutEffectEvent", health.AddWithoutEffect);
        EventBus.Unsubscribe<float>("HPSubtractWithoutEffectEvent", health.SubtractWithoutEffect);

        EventBus.Unsubscribe<float>("HungerAddWithoutEffectEvent", hunger.AddWithoutEffect);
        EventBus.Unsubscribe<float>("HungerSubtractWithoutEffectEvent", hunger.SubtractWithoutEffect);

        EventBus.Unsubscribe<float>("StaminaAddWithoutEffectEvent", stamina.AddWithoutEffect);
        EventBus.Unsubscribe<float>("StaminaSubtractWithoutEffectEvent", stamina.SubtractWithoutEffect);

    }


    private void Start()
    {
        EventBus.Publish("InitConditionEvent", GetConditionList());
    }


    public class ConditionList
    {
        public Condition health;
        public Condition hunger;
        public Condition stamina;
    }
}
