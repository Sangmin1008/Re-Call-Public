using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPlaceableItemData", menuName = "Scriptable Objects/Item/Placeable Item Data")]
public class PlaceableItemDataSO : CraftableItemDataSO, IPlaceable
{
    [Header("Preview Object Prefab")]
    [SerializeField] private GameObject previewPrefab;
    public GameObject GetPreviewPrefab() => previewPrefab;
}
