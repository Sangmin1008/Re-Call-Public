using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class EndingUI : MonoBehaviour
{
    public GameObject obj;
    public Image image;
    public TextMeshProUGUI text;
    public RectTransform creditTransform;
    public float startY = -500f;
    public float scrollDuration = 10f;
    public float endY = 500f;
    public string insertText = "\n";

    string ttext;
    public void Awake()
    {
        EventBus.Subscribe<string>("Ending", EndingFade);

        image.enabled = false;
        text.enabled = false;
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<string>("Ending", EndingFade);
    }

    public void EndingFade(string a)
    {
        EventBus.Publish("FadeEvent", null);
        ttext = a;
        Invoke("FadeCall", 4f/2);
        
        
        
    }

    void FadeCall()
    {
        EventBus.Publish("FadeEvent", null);
        Ending(ttext);
    }

    public void Ending(string a)
    {
        obj.SetActive(true);
        image.enabled = true;
        text.enabled = true;
        Debug.Log("클리어");
        text.text = a + insertText;

        creditTransform.anchoredPosition = new Vector2(0, startY);


        creditTransform.DOAnchorPosY(endY, scrollDuration).SetEase(Ease.Linear);
    }
}
