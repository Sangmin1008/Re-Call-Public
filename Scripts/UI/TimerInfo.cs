using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimerInfo : MonoBehaviour
{
    public TextMeshProUGUI text;


    private void OnEnable()
    {
        GameManager.Instance.timerInfo += Refresh;
    }

    private void OnDisable()
    {
        GameManager.Instance.timerInfo -= Refresh;
    }



    public void Refresh(object obj)
    {
        //?섍쿋吏癒?
        if(obj == null)
        {
          
        }
        else
            text.text = obj.ToString();// + "F";

        //?섏씠?쒖븘?껋쓣 ?ｌ쓣源?
    }
}
