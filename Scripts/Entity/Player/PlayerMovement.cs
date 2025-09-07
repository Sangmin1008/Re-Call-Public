using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    private Vector3 moveDirection;

    [Header("Movement and Rotation")]
    [SerializeField] private float movementSpeed = Constants.DEFAULT_MOVEMENT_SPEED;
    [SerializeField] private float rotationSpeed = Constants.DEFAULT_ROTATION_SPEED;
    [SerializeField] private float runMovementSpeed = Constants.RUN_MOVEMENT_SPEED;
    private float startMovementSpeed;


    [Header("Jump")]
    [SerializeField] private float jumpForce = Constants.DEFAULT_JUMP_FORCE;
    [SerializeField] private float staminaConsumption = Constants.JUMP_STAMINA_CONSUMPTION;

    // 플레이어 이동 입력
    private Vector2 currentMoveMentInput;
    // 마우스 이동 입력
    private Vector2 mouseDelta;
    // 카메라 회전 각도
    private float cameraCurrentXRotation;
    // 플레이어 이동 활성화 여부
    public bool isMovementEnabled = true;


    private PlayerSensor playerSensor;
    private PlayerCondition playerCondition;

    private Rigidbody rb;
    private Transform tr;

    [SerializeField]private Transform cameraTransform;

    private float footStepSoundInterval = 0.5f;
    private float lastFootstepTime = 0f;

    private bool isMoving = false;
    private bool isRunning = false;

    public bool IsRunning => isRunning;
    public float MovementSpeed => movementSpeed;

    private SoundManager soundManager;
    void Start()
    {
        tr = transform;
        rb = GetComponent<Rigidbody>();
        playerSensor = GetComponent<PlayerSensor>();
        playerCondition = GetComponent<PlayerCondition>();
        soundManager = SoundManager.Instance;
        startMovementSpeed = movementSpeed;
    }

    void OnEnable()
    {
        EventBus.SubscribeVoid("OnPlayerLanded", OnPlayerLanded);
    }

    void OnDisable()
    {
        EventBus.UnsubscribeVoid("OnPlayerLanded", OnPlayerLanded);
    }

    private void OnPlayerLanded()
    {
        }

    void Update()
    {
        if(!isMovementEnabled) return;

        // 스태미나가 부족하면 달리기 중단
        if(isRunning && !playerCondition.CanRun())
        {
            isRunning = false;
            movementSpeed = startMovementSpeed;
            footStepSoundInterval = Constants.FOOTSTEP_SOUND_INTERVAL;
        }

        Move();
        CameraLook();
        Footstep();
    }

    private void Footstep()
    {
        if(playerSensor.CheckGrounded() && isMoving && Time.time - lastFootstepTime >= footStepSoundInterval)
        {
            lastFootstepTime = Time.time;
            if(isRunning)
            {
                soundManager.PlaySFX(SFXType.RunFootstep);
            }
            else
            {
                soundManager.PlaySFX(SFXType.Footstep);
            }
        }
    }

    void Move()
    {
        Vector3 dir = tr.forward * currentMoveMentInput.y + tr.right * currentMoveMentInput.x;
        dir *= movementSpeed;
        dir.y = rb.velocity.y;
        rb.velocity = dir;
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            currentMoveMentInput = context.ReadValue<Vector2>();
            isMoving = true;
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            currentMoveMentInput = Vector2.zero;
            isMoving = false;
        }
    }

    public void OnRun(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed)
        {
            if(!playerCondition.CanRun())
            {
                return;
            }

            isRunning = true;
            movementSpeed = runMovementSpeed;
            footStepSoundInterval = Constants.RUN_FOOTSTEP_SOUND_INTERVAL;
        }
        else if(context.phase == InputActionPhase.Canceled)
        {
            isRunning = false;
            movementSpeed = startMovementSpeed;
            footStepSoundInterval = Constants.FOOTSTEP_SOUND_INTERVAL;
        }
    }

    private void CameraLook()
    {
        cameraCurrentXRotation += mouseDelta.y * rotationSpeed;
        cameraCurrentXRotation = Mathf.Clamp(cameraCurrentXRotation, Constants.MIN_X_LOOK, Constants.MAX_X_LOOK);
        cameraTransform.localRotation = Quaternion.Euler(-cameraCurrentXRotation, 0, 0);

        tr.rotation *= Quaternion.Euler(0, mouseDelta.x * rotationSpeed, 0);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseDelta = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if(context.phase == InputActionPhase.Performed && playerSensor.CheckGrounded())
        {
            if(!playerCondition.CanJump())
            {
                return;
            }

            soundManager.PlaySFX(SFXType.Jump);
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            playerCondition.UseStaminaForJump();
        }
    }

    public void StopMovement()
    {
        rb.velocity = Vector3.zero;
    }
}
