using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloorInfo : MonoBehaviour
{
    public TextMeshProUGUI text;

    private void OnEnable()
    {
        EventBus.Subscribe("FloorRefreshEvent", Refresh);
        GameManager.Instance.floorInfo += Refresh;
    }

    private void OnDisable()
    {
        EventBus.Unsubscribe("FloorRefreshEvent", Refresh);
        GameManager.Instance.floorInfo -= Refresh;
    }
    public void Refresh(object obj)
    {
        text.text = obj.ToString()+"F";// + "F";

    }


}
