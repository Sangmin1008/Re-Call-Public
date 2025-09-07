using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PurificationConsole : MonoBehaviour, IInteractable
{
    public GameObject puzzlePanel;
    public ChemicalPurifier purifier;

    public int requiredBatteryCount = 3;

    public void OnInteract()
    {
        if (BatteryInteractable.collectedBatteryCount >= requiredBatteryCount)
        {
            Debug.Log("정화 장치 활성화됨");
            puzzlePanel.SetActive(true);
            purifier.Start();
        }
        else
        {
            Debug.Log("배터리가 부족합니다.");
        }
    }

    public string GetInteractText()
    {
        return "정화 퍼즐 시작 (E)";
    }
}
