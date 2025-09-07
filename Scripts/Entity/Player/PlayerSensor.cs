using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum SensorType
{
    Ground,
    Interactable,
}

[Serializable]
public struct SensorLayer
{
    public SensorType sensorType;
    public LayerMask layerMask;
}



public class PlayerSensor : MonoBehaviour
{
    [Header("Layer Settings")]
    [SerializeField] private List<SensorLayer> sensorLayers;
    [SerializeField] private float distance = Constants.DEFAULT_SENSOR_DISTANCE;

    [Header("Ground Check")]
    [SerializeField] private Transform footTransform;

    private Camera camera;
    private bool isSensorEnabled = true;

    // 착지 감지를 위한 변수
    private bool wasGroundedLastFrame = true;
    private float lastLandingTime = 0f;

    public void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        // 매 프레임마다 착지 감지
        CheckLanding();
    }
    public GameObject RaycastInteractableObject()
    {
        if(!isSensorEnabled) return null;

        var layer = sensorLayers.Find(l => l.sensorType == SensorType.Interactable);
        Ray ray = camera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Constants.SENSOR_RAY_DISTANCE, layer.layerMask))
        {
            //hit.collider.GetComponentInParent<IInteractable>().ToggleOutLine();
            var interactableObject = hit.collider.gameObject;
            if (interactableObject != null)
            {
                return interactableObject;
            }
        }

        return null;
    }

    private void CheckLanding()
    {
        if(Time.time - lastLandingTime < Constants.LANDING_CHECK_INTERVAL) return;
        lastLandingTime = Time.time;

        bool isGroundedNow = CheckGrounded();

        // 착지 감지: 이전 프레임에는 공중에 있었고 현재 프레임에는 땅에 있으면 착지
        if (!wasGroundedLastFrame && isGroundedNow)
        {
            EventBus.PublishVoid("OnPlayerLanded");
        }

        wasGroundedLastFrame = isGroundedNow;
    }

    public bool CheckGrounded()
    {
        var layer = sensorLayers.Find(l => l.sensorType == SensorType.Ground);

        Ray[] rays = new Ray[4];
        rays[0] = new Ray(footTransform.position + (footTransform.position * Constants.FOOT_SENSOR_OFFSET) + footTransform.up * Constants.FOOT_SENSOR_UP_OFFSET, Vector3.down);
        rays[1] = new Ray(footTransform.position + (-footTransform.forward * Constants.FOOT_SENSOR_OFFSET) + footTransform.up * Constants.FOOT_SENSOR_UP_OFFSET, Vector3.down);
        rays[2] = new Ray(footTransform.position + (footTransform.right * Constants.FOOT_SENSOR_OFFSET) + footTransform.up * Constants.FOOT_SENSOR_UP_OFFSET, Vector3.down);
        rays[3] = new Ray(footTransform.position + (-footTransform.right * Constants.FOOT_SENSOR_OFFSET) + footTransform.up * Constants.FOOT_SENSOR_UP_OFFSET, Vector3.down);

        foreach(var ray in rays)
        {
            if(Physics.Raycast(ray, out RaycastHit hit, distance, layer.layerMask))
            {
                return true;
            }
        }

        return false;
    }
}
