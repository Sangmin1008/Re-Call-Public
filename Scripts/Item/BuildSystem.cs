using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BuildSystem : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float range;
    [SerializeField] private float scrollSensitivity = 30f;

    [SerializeField] private GameObject buildHint;

    private bool _isPreviewActivated = false;

    private GameObject _previewPrefab;
    private GameObject _buildPrefab;

    private RaycastHit _hit;

    private Vector3 _initialPreviewPosition;
    private Quaternion _initialPreviewRotation;

    private float _rotationY = 0f;

    private GenericItemDataSO _currentItemData;

    private void Start()
    {
        EventBus.Subscribe<PlaceableItemDataSO>("buildStartEvent", BuildStart);
        EventBus.Subscribe<Vector3>("buildOriginPosition", SetOriginPosition);
        EventBus.Subscribe<Quaternion>("buildOriginRotation", SetOriginRotation);
        buildHint.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventBus.Unsubscribe<PlaceableItemDataSO>("buildStartEvent", BuildStart);
        EventBus.Unsubscribe<Vector3>("buildOriginPosition", SetOriginPosition);
        EventBus.Subscribe<Quaternion>("buildOriginRotation", SetOriginRotation);
    }

    private void Update()
    {
        if (_isPreviewActivated)
        {
            PreviewPositionUpdate();
            PreviewRotationUpdate();
        }


        if (Input.GetButtonDown("Fire1") && _isPreviewActivated)
            GetItem();

        if (Input.GetButtonDown("Fire2"))
            Build();

        if (Input.GetKeyDown(KeyCode.Escape))
            Cancel();
    }

    private void SetOriginPosition(Vector3 pos)
    {
        _initialPreviewPosition = pos;
    }

    private void SetOriginRotation(Quaternion quaternion)
    {
        _initialPreviewRotation = quaternion;
    }

    public void BuildStart(PlaceableItemDataSO placeableItemDataSo)
    {
        _previewPrefab = Instantiate(placeableItemDataSo.GetPreviewPrefab(), player.transform.position + player.transform.forward,
            Quaternion.identity);
        _buildPrefab = placeableItemDataSo.WorldPickupPrefab;
        _isPreviewActivated = true;
        _currentItemData = placeableItemDataSo;
        buildHint.gameObject.SetActive(true);
    }

    private void PreviewPositionUpdate()
    {
        int groundLayerMask = 1 << LayerMask.NameToLayer("Ground");

        Vector3 origin = Camera.main.transform.position;
        Vector3 direction = Camera.main.transform.forward;
        if (Physics.Raycast(origin, direction, out _hit, range, groundLayerMask))
        {
            _previewPrefab.transform.position = _hit.point;
        }
    }

    private void PreviewRotationUpdate()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) > 0.01f)
        {
            _rotationY += scroll * scrollSensitivity;
            _previewPrefab.transform.rotation = Quaternion.Euler(0f, _rotationY, 0f);
        }
    }

    private void GetItem()
    {
        if (_isPreviewActivated)
            Destroy(_previewPrefab);


        _isPreviewActivated = false;
        _previewPrefab = null;
        _buildPrefab = null;
        EventBus.Publish<GenericItemDataSO>("AddItem", _currentItemData);
        buildHint.gameObject.SetActive(false);
    }

    private void Build()
    {
        if (_isPreviewActivated && _previewPrefab.GetComponent<PreviewPlaceableObject>().IsBuildable())
        {
            var dropData = new DroppedItemData(_currentItemData.ItemID, _hit.point, _previewPrefab.transform.rotation);
            EventBus.Publish<DroppedItemData>("DropItem", dropData);

            Instantiate(_buildPrefab, _hit.point, _previewPrefab.transform.rotation);
            Destroy(_previewPrefab);
            _isPreviewActivated = false;
            _previewPrefab = null;
            _buildPrefab = null;
            buildHint.gameObject.SetActive(false);
            EventBus.Publish<int>("QuestClear", 5);
        }
    }

    private void Cancel()
    {
        if (_isPreviewActivated)
        {
            Destroy(_previewPrefab);
            _buildPrefab = Instantiate(_buildPrefab, _initialPreviewPosition, _initialPreviewRotation);
        }


        _isPreviewActivated = false;
        _previewPrefab = null;
        _buildPrefab = null;
        buildHint.gameObject.SetActive(false);
    }
}
