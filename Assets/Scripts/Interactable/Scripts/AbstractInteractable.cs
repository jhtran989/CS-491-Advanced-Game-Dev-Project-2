using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public abstract class AbstractInteractable : MonoBehaviour
{
    [SerializeField] 
    protected string interactText;

    protected Collider _collider;
    
    [FormerlySerializedAs("_customOutline")] public CustomOutline customOutline;

    protected virtual void Awake()
    {
        // FIXME FINAL: add outline script
        customOutline = gameObject.AddComponent<CustomOutline>();
    }

    public string GetInteractText()
    {
        return interactText;
    }

    public Transform GetTransform()
    {
        return transform;
    }
    
    public void DisableInteract()
    {
        _collider.enabled = false;
    }
    
    public void EnableInteract()
    {
        _collider.enabled = true;
    }
}
