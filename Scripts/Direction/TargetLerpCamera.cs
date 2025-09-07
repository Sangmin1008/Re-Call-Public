using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLerpCamera : MonoBehaviour
{
    public GameObject target;

    public float duration = 1f;
    private float offsetX = 1.5f;
    private float offsetY = 0.5f;

    private void Start()
    {
        
    }

    public void Return(Vector3 lastPos)
    {
        //target = go;
        //Vector3 targetPosition = target.transform.position + target.transform.up * offsetY;
        //targetPosition = targetPosition + target.transform.up * offsetY;



        //transform.DOMove(lastPos, duration).SetEase(Ease.OutQuad).OnComplete(() => { EventBus.PublishVoid("EnablePlayerInput"); });

        Vector3 targetLocalPos = new Vector3(0f, 0.75f, 0f);

        // 로컬 위치로 이동
        transform.DOLocalMove(targetLocalPos, duration)
                 .SetEase(Ease.OutQuad)
                 .OnComplete(() => { EventBus.PublishVoid("EnablePlayerInput"); });
    }



    public void MoveToTarget(GameObject go)
    {
        target = go;
        Vector3 targetPosition = target.transform.position + target.transform.forward * offsetX;
        targetPosition = targetPosition + target.transform.up * offsetY;
        LookTarget(transform, targetPosition,  target.transform, duration);

        
        transform.DOMove(targetPosition, duration).SetEase(Ease.OutQuad);
    }

    public void LookTarget(Transform myTransform, Vector3 targetPosition, Transform targetTransform, float duration = 0.5f)
    {
        Vector3 direction = targetTransform.position - targetPosition;
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            myTransform
                .DORotateQuaternion(targetRotation, duration)
                .SetEase(Ease.OutQuad);
        }
    }
}
