using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class InteractableComponents : MonoBehaviour
{
    public Rigidbody currentInteractableRigidbody;
    public Collider currentInteractableCollider;
    
    // hand of the Player camera (so it will rotate as the camera rotates)
    public Transform hand;
    
    // FIXME: only for fire extinguisher...
    public bool isHoldingFireExtinguisher;

    // private void Awake()
    // {
    //     currentInteractableRigidbody = null;
    //     currentInteractableCollider = null;
    // }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
