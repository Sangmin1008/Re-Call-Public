using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WASD : MonoBehaviour
{
    public Image w;
    public Image a;
    public Image s;
    public Image d;

    private int clickNum=10;

    private void Update()
    {
        if (GameManager.Instance.questNum!=0||clickNum <=0)
        {
            Destroy(gameObject);
            EventBus.Publish<int>("QuestClear",0);
        }

        if (Input.GetKeyDown(KeyCode.W))
            ChangeColor(w);
        if (Input.GetKeyDown(KeyCode.S))
            ChangeColor(s);
        if (Input.GetKeyDown(KeyCode.D))
            ChangeColor(d);
        if (Input.GetKeyDown(KeyCode.A))
            ChangeColor(a);
        if(Input.GetKeyUp(KeyCode.W))
            rollbackColor(w);
        if(Input.GetKeyUp(KeyCode.S)) rollbackColor(s);
        if(Input.GetKeyUp(KeyCode.D))
            rollbackColor(d);
        if (Input.GetKeyUp(KeyCode.A))
            rollbackColor(a);
    }
    private void ChangeColor(Image x)
    {
        x.color = new Color(0.5f, 0.1f, 0.1f);
    }
    private void rollbackColor(Image x)
    {
        x.color = Color.white;
        clickNum--;
    }

}
