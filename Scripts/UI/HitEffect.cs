using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitEffect : MonoBehaviour
{
    public Image image;
    public float flashSpeed;

    private Coroutine coroutine;


    private void OnEnable()
    {
        EventBus.Subscribe("HitEffectEvent", Flash);
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe("HitEffectEvent", Flash);
    }
    public void Flash(object obj)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }


        image.enabled = true;
        coroutine = StartCoroutine(FadeAway());
    }


    public IEnumerator FadeAway()
    {
        float startAlpha = Constants.HIT_EFFECT_START_ALPHA;
        float a = startAlpha;

        while (a > Constants.Normalized_Value_ZERO)
        {
            a -= (startAlpha / flashSpeed) * Time.deltaTime;
            image.color = new Color(image.color.r, image.color.g, image.color.b, a);
            yield return null;
        }
        image.enabled = false;
    }

    public void TestHit()
    {
        EventBus.Publish("HitEffectEvent",null);
    }
}
