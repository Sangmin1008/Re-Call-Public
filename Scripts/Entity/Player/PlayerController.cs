using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerCondition playerCondition;
    private PlayerMovement playerMovement;
    private PlayerInventory playerInventory;
    private PlayerSensor playerSensor;
    private Equipment equipment;
    private PlayerStat playerStat;
    private SoundManager soundManager;

    private float lastLayCheckTime = 0f;

    private static GameObject currentInteractableObject = null;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerInventory = GetComponent<PlayerInventory>();
        playerSensor = GetComponent<PlayerSensor>();
        playerCondition = GetComponent<PlayerCondition>();
        equipment = GetComponent<Equipment>();
        playerStat = GetComponent<PlayerStat>();
        soundManager = SoundManager.Instance;
    }

    private void OnEnable()
    {
        EventBus.SubscribeVoid("Dead", OnDead);
        EventBus.Subscribe<float>("PlayerTakeDamage", OnTakeDamage);
        EventBus.Subscribe<float>("Heal", OnHeal);
        EventBus.Subscribe<float>("Eat", OnEat);
        EventBus.Subscribe<bool>("OpenInventory", OnInventory);
        EventBus.SubscribeVoid("DisablePlayerInput", DisablePlayerInput);
        EventBus.SubscribeVoid("EnablePlayerInput", EnablePlayerInput);

    }

    private void OnDisable()
    {
        EventBus.UnsubscribeVoid("Dead", OnDead);
        EventBus.Unsubscribe<float>("PlayerTakeDamage", OnTakeDamage);
        EventBus.Unsubscribe<float>("Heal", OnHeal);
        EventBus.Unsubscribe<float>("Eat", OnEat);
        EventBus.Unsubscribe<bool>("OpenInventory", OnInventory);
        EventBus.UnsubscribeVoid("DisablePlayerInput", DisablePlayerInput);
        EventBus.UnsubscribeVoid("EnablePlayerInput", EnablePlayerInput);
    }

    public void Update()
    {
        lastLayCheckTime += Time.deltaTime;
        if(lastLayCheckTime >= Constants.LAY_CHECK_INTERVAL)
        {
            CheckInteractableObject();
            lastLayCheckTime = 0f;
        }
    }

    public void CheckInteractableObject()
    {
        if (playerMovement.isMovementEnabled == true)
        {
            GameObject _interactableObject = playerSensor.RaycastInteractableObject();

            //감지된게 있고 이전이 없으면
            if (_interactableObject != null && currentInteractableObject == null)
            {
                currentInteractableObject = _interactableObject;
                EventBus.Publish("ShowInteractPrompt", currentInteractableObject);
                if (_interactableObject.TryGetComponent(out OutlineScript outLine))
                {
                    if (outLine.outLineFlag == false)
                    {
                        outLine.ToggleOutLine();
                    }
                }
            }
            //감지된게 없고 이전이 지금이 다르다면
            else if (_interactableObject == null || currentInteractableObject != _interactableObject)
            {
                if (currentInteractableObject != null)
                {
                    if (currentInteractableObject.TryGetComponent(out OutlineScript outLine))
                    {
                        outLine.ToggleOutLine();
                    }
                    currentInteractableObject = null;
                    EventBus.Publish("HideInteractPrompt", null);
                }
            }

            if (currentInteractableObject != null && currentInteractableObject as Object == null) // 오브젝트가 파괴되었는지 확인 방어코드 추가
            {
                currentInteractableObject = null;
                EventBus.Publish("HideInteractPrompt", null);
            }
        }
    }

    public void OnDead()
    {
        DisablePlayerInput();

        // 죽음 효과 재생
        soundManager.PlaySFX(SFXType.Die);
        GameManager.Instance.GameOver();
    }

    public void OnHeal(float heal)
    {

        playerCondition.Heal(heal);

    }

    public void OnEat(float eat)
    {
        playerCondition.Eat(eat);
    }


    private void OnInteract(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            if(currentInteractableObject != null)
            {
                if (currentInteractableObject.TryGetComponent<IInteractable>(out IInteractable interactableObject))
                {
                    if(interactableObject is WorldItemPickup)
                    {
                        SoundManager.Instance.PlaySFX(SFXType.PickUpItem);
                    }

                    interactableObject.OnInteract();

                    Invoke("InvokeNull", Constants.INTERACT_INVOKE_DELAY);
                }
            }
        }
    }
    void InvokeNull()
    {
        currentInteractableObject = null;
        EventBus.Publish("HideInteractPrompt", null);
    }
    public void DisablePlayerInput()
    {
        playerMovement.StopMovement();

        if(playerMovement != null) playerMovement.isMovementEnabled = false;
        if(playerSensor != null) playerSensor.enabled = false;
        if(equipment != null) equipment.isEquipEnabled = false;
    }

    public void EnablePlayerInput()
    {
        if (playerMovement != null) playerMovement.isMovementEnabled = true;
        if (playerSensor != null) playerSensor.enabled = true;
        if (equipment != null) equipment.isEquipEnabled = true;
    }

    private void OnInventory(bool isOpen)
    {
        if(isOpen)
        {
            EventBus.Publish<int>("QuestClear", 1);
            DisablePlayerInput();
            EventBus.Publish("HideInteractPrompt", null);
        }
        else
        {
            EnablePlayerInput();
        }
    }

    private void OnTakeDamage(float damage)
    {
        playerCondition.TakePhysicalDamage(damage);
    }
}
