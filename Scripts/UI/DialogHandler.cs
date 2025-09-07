using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogHandler : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText;
    private void OnEnable()
    {
        EventBus.Subscribe<GameObject>("DialogueEvent", InitDialogue);
        
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe<GameObject>("DialogueEvent", InitDialogue);
    }

    public void InitDialogue(GameObject go)
    {
        NPCController npc = go.GetComponent<NPCController>();

        npc.dialogueText = dialogueText;
        npc.nameText = nameText;

        npc.ShowDialogue();
}
}
