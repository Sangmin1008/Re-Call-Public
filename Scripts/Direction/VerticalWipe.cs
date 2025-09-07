using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class VerticalWipe : MonoBehaviour
{
    public List<Material> materials = new List<Material>();
    public float duration = 3f;
    Material dissolveMaterial;

    public bool isChange = false;
    public Material test;
    public string ttest;
    void Start()
    {
        EventBus.Subscribe("VerticalWipeEvent", VerticalWipeDissolve);
        //GameManager.Instance.OnGameEnd += VerticalWipeDissolve;
        var renders = GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renders.Length; i++)
        {
            materials.AddRange(renders[i].materials);
        }
        test = materials[0];
        ttest = materials[0].name;
        if (materials[0].name != "Shader Graphs_Dissolve_Dissolve_Metallic (Instance)")
        {
            isChange = true;
            dissolveMaterial = Resources.Load<Material>("Materials/Shader Graphs_Dissolve_Dissolve_Metallic");
        }
        else
        {
            dissolveMaterial = materials[0];
        }
        

    }
    private void OnDisable()
    {
        EventBus.Unsubscribe("VerticalWipeEvent", VerticalWipeDissolve);
        //GameManager.Instance.OnGameEnd -= VerticalWipeDissolve;
    }

    GameObject tempGo;
    public void VerticalWipeDissolve(object obj)
    {
        if (obj == null)
        {
            obj = gameObject;
        }


        //gameObject.meshRenderers[0].material.name == 

        tempGo = obj as GameObject;

        if (tempGo.TryGetComponent<Enemy>(out var enemy))
        {
            // Enemy가 있다면 Dissolve용 머티리얼 적용
            Material newMat = new Material(dissolveMaterial);
            enemy.meshRenderers[0].material = newMat;
            materials.Clear();
            materials.Add(newMat);
        }
        else
        {
            // 일반 오브젝트 처리
            if (tempGo.TryGetComponent<Renderer>(out var renderer))
            {
                Material[] mats = renderer.materials;
                if (mats.Length > 0)
                {
                    if (isChange == true)
                    {
                        Debug.Log(gameObject.name);
                        Material newMat;
                        newMat = new Material(dissolveMaterial);

                        mats[0] = newMat;
                        renderer.materials = mats; // 머티리얼 교체 적용

                        materials.Clear();
                        materials.Add(newMat);
                    }
                }
            }
        }


        DoTweenExtensions.TweenFloat(0, 1f, duration, SetValue, () => { Destroy(tempGo); });
    }

    public void SetValue(float value)
    {
        for (int i = 0; i < materials.Count; i++)
        {
            materials[i].SetFloat("_Dissolve", value);
        }
    }

    /*
    public void SetValue(float value)
    {
        Vector3 tempVector = new Vector3(0,value,0);
        for (int i = 0; i < materials.Count; i++)
        {

            materials[i].SetVector("_DissolveOffest", tempVector);
        }
    }
    */

}
