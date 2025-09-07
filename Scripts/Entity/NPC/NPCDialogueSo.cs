using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCDialogueSo : MonoBehaviour
{
    [System.Serializable]
    public class DialogueLine
    {
        [TextArea(2, 5)] public string[] Text0;
        [TextArea(2, 5)] public string[] Text1;
        [TextArea(2, 5)] public string[] Text2;
        [TextArea(2, 5)] public string[] Text3;

        public string[] GetDialogue(int timeCode)
        {
            return timeCode switch
            {
                0 => Text0,
                1 => Text1,
                2 => Text2,
                3 => Text3
            };
        }
    }

    [CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue/NPC Dialogue")]
    public class DialogueData : ScriptableObject
    {
        public string NPCName;
        public DialogueLine dialogueLines;
    }
}
