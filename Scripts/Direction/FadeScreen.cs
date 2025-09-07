using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScreen : MonoBehaviour
{
    public Image image;
    public float duration;

    private Coroutine coroutine;

    public bool fadeFlag = false;
    Color temp = new Color(255, 255, 255);

    public CanvasGroup canvasGroup;

    private void OnEnable()
    {
        EventBus.Subscribe("FadeEvent", FadeToggle);
        //GameManager.Instance.OnGameEnd += FadeToggle;
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe("FadeEvent", FadeToggle);
        //GameManager.Instance.OnGameEnd -= FadeToggle;
    }

    private void Start()
    {
        fadeFlag = true;
        image.enabled = true;
    }
    public void FadeIn()
    {

        image.enabled = true;
        Color c = image.color;
        c.a = 0f;
        image.color = c;
        //image.color = new Color(image.color.r, image.color.g, image.color.b, 0f);

        DoTweenExtensions.TweenFloat(0f, 1f, duration/3,
        alpha =>
        {
            var currentColor = image.color;
            currentColor.a = alpha;
            image.color = currentColor;
        },
        FadeFlagSwitch
        );
    }
    public void FadeOut()
    {
        if (GameManager.Instance.isEnd == false)
        {
            DoTweenExtensions.TweenFloat(1f, 0f, duration / 3, alpha => { canvasGroup.alpha = alpha; });
        }
        else
        {
            canvasGroup.alpha = 0f;
        }
        DoTweenExtensions.TweenFloat(1f, 0f, duration, alpha => {var c = image.color; c.a = alpha; image.color = c;}, FadeFlagSwitch);
    }

    public void FadeFlagSwitch()
    {
        
        fadeFlag = !fadeFlag;
        if(fadeFlag == false)
        {
            image.enabled = false;
        }
    }

    public void FadeToggle(object obj)
    {

        if (!fadeFlag)
        {
            FadeIn();
        }
        else
        {
            FadeOut();
        }
    }


}
