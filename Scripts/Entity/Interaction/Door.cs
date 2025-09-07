using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Door : MonoBehaviour, IInteractable
{
    bool isOpen = false;
    private Animator animator;

    public bool isPuzzleDoor = false;
    public void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void Interact()
    {
        if (isPuzzleDoor) return;
        OnInteract();
    }

    public void OnInteract()
    {
        EventBus.Publish<int>("QuestClear", 2);
        bool state = animator.GetBool("chracter_nearby");
        animator.SetBool("character_nearby", !animator.GetBool("character_nearby"));

        SoundManager.Instance.PlaySFX(state ? SFXType.DoorClose : SFXType.DoorOpen);
    }

    public string GetInteractText()
    {
        if (isOpen)
        {
            return "닫기";
        }
        else
        {
            return "열기";
        }
        //return isPuzzleDoor ? "" : "문 열기 (E)";
    }

    public void OpenByPuzzle()
    {
        EventBus.Publish<int>("QuestClear",9);
        animator.SetBool("character_nearby", true);
    }

}