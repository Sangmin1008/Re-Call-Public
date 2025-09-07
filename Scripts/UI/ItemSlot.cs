using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class ItemSlot : MonoBehaviour
{
    [HideInInspector] public ItemInstance itemData;
    [SerializeField] private TextMeshProUGUI quantityText;
    [SerializeField] private Image icon;
    public int Index;

    public void Set()
    {
        if(itemData == null) return;

        var itemDataSO = ItemDatabase.Instance.GetItemByID(itemData.itemID);
        if(itemDataSO == null) return;

        icon.gameObject.SetActive(true);
        icon.sprite = itemDataSO.Icon;
        quantityText.text = itemData.quantity > 1 ? itemData.quantity.ToString() : "";
    }

    public void Clear()
    {
        itemData = null;
        icon.gameObject.SetActive(false);
        quantityText.text = "";
    }

    public void OnClickButton()
    {
        EventBus.Publish("ClickItemSlot", Index);
    }

    public void OnClickEquipmentButton()
    {
        if(itemData != null)
        {
            EventBus.Publish("ClickEquipmentButton", itemData.itemID);
        }
    }
}
