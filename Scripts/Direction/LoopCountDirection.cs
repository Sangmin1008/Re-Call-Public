using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class LoopCountDirection : MonoBehaviour
{
    public RectTransform curLoopCnt;
    public RectTransform nextLoopCnt;
    public TextMeshProUGUI curLoopCntText;
    public TextMeshProUGUI nextLoopCntText;

    public float duration = 5f;

    private void Awake()
    {
        curLoopCntText.text = GameManager.Instance.LoopCount.ToString();
        nextLoopCntText.text = (GameManager.Instance.LoopCount + 1).ToString();
    }
    private void Start()
    {
        ChangeLoopCnt();
    }

    public void ChangeLoopCnt()
    {
        curLoopCnt.DOAnchorPos(new Vector2(0,900), duration).SetEase(Ease.OutQuad);
        nextLoopCnt.DOAnchorPos(new Vector2(0, 0), duration).SetEase(Ease.OutQuad).OnComplete(() => EventBus.Publish("FadeEvent", null));

    }
    private void OnEnable()
    {
        if (GameManager.Instance.isEnd == true)
        {
            curLoopCntText.gameObject.SetActive(false);
            nextLoopCntText.gameObject.SetActive(false);
        }
    }
}
