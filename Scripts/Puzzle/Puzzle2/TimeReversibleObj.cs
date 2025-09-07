using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeInteractableObject : MonoBehaviour, IInteractable
{
    [Header("상태 오브젝트")]
    public GameObject currentStateObject; // 현재 상태 (장애물 등)
    public GameObject pastStateObject;    // 과거 상태 (사라진 상태 등)

    [Header("필요한 아이템")]
    public GenericItemDataSO requiredItem; // 시간 역행 장치 SO

    private PlayerInventory playerInventory;

    public float duration = 3f;

    void Start()
    {
        playerInventory = FindObjectOfType<PlayerInventory>();
    }

    public void OnInteract()
    {
        Debug.Log("상호작용 시도");

        if (playerInventory == null)
        {
            Debug.LogWarning("PlayerInventory를 찾을 수 없습니다.");
            return;
        }

        if (requiredItem == null)
        {
            Debug.LogWarning("requiredItem 설정이 필요합니다.");
            return;
        }

        if (playerInventory.HasItem(requiredItem))
        {
            EventBus.Publish<int>("QuestClear", 7);
            Debug.Log("시간 역행 성공!");
            ApplyPastState();
        }
        else
        {
            Debug.Log("시간 장치를 갖고 있어야 작동합니다.");
        }
    }

    public void ApplyPastState()
    {
        GetComponent<Collider>().enabled = false;
        /*
        if (currentStateObject != null)
            //currentStateObject.GetComponent<VerticalWipe>().VerticalWipeDissolve(currentStateObject);
            currentStateObject.SetActive(false);
        */
        currentStateObject.transform.DOMove(pastStateObject.transform.position, duration).SetEase(Ease.InOutQuad);
        currentStateObject.transform.DORotate(pastStateObject.transform.rotation.eulerAngles,duration, RotateMode.FastBeyond360).SetEase(Ease.OutBack);


        /*if (pastStateObject != null)
            pastStateObject.SetActive(true);*/
    }

    public string GetInteractText()
    {
        return "시간 되돌리기 (E)";
    }
}
