using DG.Tweening;
using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class Condition : MonoBehaviour
{
    public float curValue;
    public float startValue;
    public float maxValue;
    public float passiveValue;

    private float pastValue;

    public Image UIBar;
    public Image pastBar;

    public float duration = 0.5f;



    void Start()
    {
        curValue = maxValue;
        pastValue = curValue;
    }

    // Update is called once per frame
    void Update()
    {
        UIBar.fillAmount = curValue / maxValue;
        pastBar.fillAmount = pastValue / maxValue;
    }

    public void Add(float value)
    {
        float start = curValue;
        float end = Mathf.Min(curValue + value, maxValue);

        DOTween.To(() => start,
                   x => {
                       start = x;
                       curValue = x;
                   },
                   end,
                   duration
        ).SetEase(Ease.OutQuad);
    }

    public void AddWithoutEffect(float value)
    {
        curValue = Mathf.Min(curValue + value, maxValue);
        pastValue = curValue;

    }

    public void SubtractWithoutEffect(float value)
    {
        curValue = Mathf.Max(curValue - value, Constants.Normalized_Value_ZERO);
        pastValue = curValue;
    }

    public void Subtract(float value)
    {
        curValue = Mathf.Max(curValue - value, Constants.Normalized_Value_ZERO);
        ReduceEffect();
    }

    public void ReduceEffect()
    {
        float start = pastValue;
        float end = curValue;

        DOTween.To(() => start,
                   x => {
                       start = x;
                       pastValue = x;
                   },
                   end,
                   duration
        ).SetEase(Ease.OutQuad);
    }
}
