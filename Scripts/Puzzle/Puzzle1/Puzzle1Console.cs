using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle1Console : MonoBehaviour, IInteractable
{
    public GameObject puzzlePanel;  // 퍼즐 UI (비활성 상태에서 시작)
    public Puzzle1UI puzzleUI;       // UI 스크립트
    public Door connectedDoor;      // 연결된 퍼즐 문

    public void UnlockDoor()
    {
        connectedDoor.OpenByPuzzle(); // 퍼즐 성공 시 문 열기
    }

    public string GetInteractText()
    {
        return "퍼즐 시작 (E)";
    }

    public void OnInteract()
    {
        Debug.Log("콘솔과 상호작용했습니다.");
        puzzlePanel.SetActive(true);
        puzzleUI.Setup(this);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        EventBus.PublishVoid("DisablePlayerInput");
    }
}
