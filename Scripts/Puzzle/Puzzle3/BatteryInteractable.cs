using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryInteractable : MonoBehaviour, IInteractable
{
    public static int collectedBatteryCount = 0;

    public void OnInteract()
    {
        collectedBatteryCount++;
        Debug.Log("배터리 습득 완료, 현재 개수: " + collectedBatteryCount);
        gameObject.SetActive(false); // 배터리 사라짐
    }

    public string GetInteractText()
    {
        return "배터리 줍기 (E)";
    }
}
