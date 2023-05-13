using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FireExtinguisherInteractableComponents : MonoBehaviour
{
    public Rigidbody currentInteractableRigidbody;
    public Collider currentInteractableCollider;
    
    // hand of the Player camera (so it will rotate as the camera rotates)
    public Transform hand;
    
    // FIXME: only for fire extinguisher...
    public bool isHoldingFireExtinguisher;

    private PlayerInteractUIIconFireExtinguisher _playerInteractUIIconFireExtinguisher;

    public PlayerInteractUIIconFireExtinguisher PlayerInteractUIIconFireExtinguisher => _playerInteractUIIconFireExtinguisher;

    [FormerlySerializedAs("_fireExtinguisher")] public FireExtinguisher fireExtinguisher;

    private void Awake()
    {
        _playerInteractUIIconFireExtinguisher = GetComponent<PlayerInteractUIIconFireExtinguisher>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // FIXME FINAL: set the fire extinguisher in hand (child)
        fireExtinguisher = GetComponentInChildren<FireExtinguisher>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
