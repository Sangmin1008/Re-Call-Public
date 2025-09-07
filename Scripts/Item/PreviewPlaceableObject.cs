using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreviewPlaceableObject : MonoBehaviour
{
    [SerializeField] private Material validMat;
    [SerializeField] private Material invalidMat;

    private List<Collider> _colliders = new List<Collider>();

    private void Update()
    {
        ChangeColor();
    }

    private void ChangeColor()
    {
        if (_colliders.Count > 0)
            SetColor(invalidMat);
        else
            SetColor(validMat);
    }

    private void SetColor(Material mat)
    {
        transform.GetComponent<Renderer>().material = mat;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("트리거 발동");
        if (other.gameObject.layer != LayerMask.NameToLayer("Ground"))
            _colliders.Add(other);
    }

    private void OnTriggerExit(Collider other)
    {
        Debug.Log("트리거 해제");
        if (other.gameObject.layer != LayerMask.NameToLayer("Ground"))
            _colliders.Remove(other);
    }

    public bool IsBuildable()
    {
        return _colliders.Count == 0;
    }
}
