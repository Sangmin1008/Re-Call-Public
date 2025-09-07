using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericItemTraitSO<T> : ScriptableObject, IItemTrait
{
    [SerializeField] private T data;
    [SerializeField] private string eventName;
    public void Apply()
    {
        EventBus.Publish(eventName, data);
    }

    protected void SetData(T value)
    {
        data = value;
    }
}
