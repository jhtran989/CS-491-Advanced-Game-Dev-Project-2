using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable 
{
    // public string InteractText
    // {
    //     get;
    //     set;
    // }
    
    public void Interact(Transform interactorTransform);
    public string GetInteractText();
    public Transform GetTransform();
}