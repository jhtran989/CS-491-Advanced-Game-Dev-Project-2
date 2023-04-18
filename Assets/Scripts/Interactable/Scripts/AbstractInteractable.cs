using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractInteractable : MonoBehaviour
{
    [SerializeField] 
    protected string interactText;

    protected Collider _collider;

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
