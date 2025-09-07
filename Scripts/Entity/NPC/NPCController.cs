using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using static NPCDialogueSo;


public interface INPCRole
{
    public void Role();
}



public class NPCController : MonoBehaviour, IInteractable
{
    [HideInInspector]
    public TextMeshProUGUI dialogueText;
    [HideInInspector]
    public TextMeshProUGUI nameText;
    public string NPCName;
    private string tempText;
    public Dissolve dissolve;

    [Header("0~5새벽, 6~11오전, 12~17오후, 18~23밤")]
    //public List<string[]> dialogue = new List<string[]>();

    public DialogueData NPCDialogueSo;
    private string[] dialogue;

    public bool isDialogue = false;
    public int timeIndex = 0;
    public int index;

    public float curTime = 0f;

    private Vector3 cameraCurPos;
    public INPCRole NPCRole;

    private void Awake()
    {
        //기능 있다면 가져오기
        NPCRole = GetComponent<INPCRole>();
    }

    private void Update()
    {
        Vector3 direction = Camera.main.transform.position - transform.position;

        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            float yAngle = targetRotation.eulerAngles.y;

            // x, z는 0, y만 유지
            transform.rotation = Quaternion.Euler(0f, yAngle, 0f);
        }

        if (isDialogue)
        {
            if (Input.GetMouseButtonDown(0))
            {
                NextDialogue();
            }
        }
    }


    public void InitDialogue()
    {
        UIManager.Instance.ShowUI("Dialogue");
        EventBus.Publish("DialogueEvent", gameObject);
    }
    public void ShowDialogue()
    {
        UIManager.Instance.HideUI("Condition");
        if (NPCName != null)
        {
            nameText.text = NPCName;
        }
        else
        {
            nameText.text = gameObject.name;
        }
        timeIndex = (int)(GameManager.Instance.mainTimer.CurrentTime + 6) % 24 / 6;

        //대사가 있긴 하다면
        if (NPCDialogueSo != null)
        {
            string[] result = NPCDialogueSo.dialogueLines.GetDialogue(timeIndex);
            //대사가 있긴한데 해당 시간대사도 있을경우
            if (result != null && result.Length > 0)
            {
                dialogue = result;
            }
            else
            {
                dialogue = new string[1];
                dialogue[0] = "지금은 할말이 없어보인다.";
            }
        }
        else
        {
            return;
        }


        //플레이 조작 금지
        EventBus.PublishVoid("DisablePlayerInput");
        
        isDialogue = true;
        cameraCurPos = Camera.main.transform.position;
        //연출
        Camera.main.GetComponent<TargetLerpCamera>().MoveToTarget(gameObject);
        index = 0;
        //대사 선택 (시간대별로)
        
        tempText = dialogue[index];
        dialogueText.text = tempText;

        //대사 애니메이션
        dialogueText.DoTextClean(tempText, 1f);
        dialogueText.text = dialogue[index];
        
    }

    public void NextDialogue()
    {
        index++;
        if (index >= dialogue.Length)
        {
            HideDialogue();
            //dissolve.dissolve();
            isDialogue = false;
            EventBus.PublishVoid("EnablePlayerInput");
            Camera.main.GetComponent<TargetLerpCamera>().Return(cameraCurPos);
            NPCRole?.Role();
            UIManager.Instance.ShowUI("Condition");
            return;
        }
        dialogueText.DoTextClean(dialogue[index], 1f);
    }
    public void HideDialogue()
    {
        UIManager.Instance.HideUI("Dialogue");
    }

    public void OnInteract()
    {
        if (isDialogue == false)
        {
            EventBus.Publish("HideInteractPrompt", null);
            InitDialogue();
        }
    }

    public string GetInteractText()
    {
        return "대화하기";
    }

}
