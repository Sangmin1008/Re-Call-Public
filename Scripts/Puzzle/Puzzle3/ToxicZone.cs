using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToxicZone : MonoBehaviour
{
    public GameObject toxicEffect;

    public void Cleanse()
    {
        Debug.Log("오염 물질이 제거되었습니다!");
        toxicEffect.SetActive(false); // 실제로 막고 있던 VFX/콜라이더 비활성화
    }
}
