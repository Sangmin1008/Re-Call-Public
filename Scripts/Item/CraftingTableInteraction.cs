using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingTableInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject craftingTableUI;
    public void OnInteract()
    {
        //TODO: UI 띄우고 레시피 북에서 레시피 정보 가져오기 -> 소지중인 레시피를 옆에 표시하고, 클릭하면 TryCraft
        //TODO: 여기서는 일단 상호작용만 하도록
        Debug.Log("작업대 상호작용");
        craftingTableUI.SetActive(true);
        craftingTableUI.GetComponent<CraftingTableUI>().SetOffSlider();
        EventBus.Publish<bool>("OpenInventory", true);
        EventBus.PublishVoid("CraftingTableEvent");
    }

    public string GetInteractText() => "[E] 키를 누르세요: 제작대";
}
