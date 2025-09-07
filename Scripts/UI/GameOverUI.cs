using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameOverUI : MonoBehaviour
{
    // Start is called before the first frame update
    public Image image;
    public GameObject obj;
    public void Awake()
    {
        EventBus.Subscribe<object>("GameOver", GameOver);

        image.enabled = false;
       
    }
    public void OnDestroy()
    {
        EventBus.Unsubscribe<object>("GameOver", GameOver);
    }
    public void GameOver(object a)
    {
        Debug.Log("엔딩 클릭");
        obj.SetActive(true);
        image.enabled = true;

    }


}
