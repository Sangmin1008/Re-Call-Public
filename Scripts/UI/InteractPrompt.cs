using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractPrompt : MonoBehaviour
{
    public TextMeshProUGUI promptText;
    public GameObject interactObject;

    private void OnEnable()
    {
        EventBus.Subscribe<GameObject>("ShowInteractPrompt", ShowInteractObject);
        EventBus.Subscribe("HideInteractPrompt", HideInteractObject);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<GameObject>("ShowInteractPrompt", ShowInteractObject);
        EventBus.Unsubscribe("HideInteractPrompt", HideInteractObject);
    }

    public void ShowInteractObject(GameObject obj)
    {
        if(obj.TryGetComponent(out IInteractable asd))
        {
            promptText.text = asd.GetInteractText();
        }
        else
        {
            promptText.text = "";
        }
        interactObject.SetActive(true);
    }

    public void HideInteractObject(object obj)
    {
        promptText.text = "";
        interactObject.SetActive(false);
    }
}
