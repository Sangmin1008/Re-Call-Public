using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Dissolve : MonoBehaviour
{
    List<Material> materials = new List<Material>();
    public float value;
    public float duration = 1f;

    bool PingPong = false;
    void Start()
    {
        var renders = GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renders.Length; i++)
        {
            materials.AddRange(renders[i].materials);
        }
    }

    private void Reset()
    {
        Start();
        SetValue(0);
    }

    // Update is called once per frame
    void Update()
    {
        SetValue(value);
    }

    public void SetValue(float value)
    {
        for (int i = 0; i < materials.Count; i++)
        {
            materials[i].SetFloat("_Dissolve", value);
        }
    }


    public void dissolve()
    {
        TweenFloat(0, 1, duration);
    }
    public void TweenFloat(float start, float end, float duration, System.Action<float> onUpdate = null, System.Action onComplete = null)
    {
        value = start;
        DOTween.To(() => value, x =>
        {
            value = x;
            onUpdate?.Invoke(x);
        }, end, duration).OnComplete(() =>
        {
            onComplete?.Invoke();
        });
    }
}
