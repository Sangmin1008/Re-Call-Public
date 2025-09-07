using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LoopInfo : MonoBehaviour
{
    public TextMeshProUGUI text;

    /*
     * 이거 안쓰는건가?
    private void Awake()
    {
        EventBus.Subscribe("LoopRefreshEvent", Refresh);
        GameManager.Instance.loopInfo += Refresh;
    }


    public void Refresh(object obj)
    {
        //?섍쿋吏癒?
        text.text = obj.ToString();// + "F";
        Debug.Log("UI 실행"+obj.ToString());
        

        //?섏씠?쒖븘?껋쓣 ?ｌ쓣源?
    }

    */
}
