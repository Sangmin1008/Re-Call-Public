using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<PlayerController>(out var player))
            GameManager.Instance.GameClear();
    }
}
