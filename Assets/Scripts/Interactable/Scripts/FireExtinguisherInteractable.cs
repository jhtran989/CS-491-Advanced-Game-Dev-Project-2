using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExtinguisherInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] 
    private string interactText;
    
    [SerializeField]
    private InteractableComponents interactableComponents;
    
    private float _interactableSpeed;

    private void Awake()
    {
        interactableComponents.isHoldingFireExtinguisher = false;
        _interactableSpeed = 12.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (interactableComponents.isHoldingFireExtinguisher)
        {
            // interactableComponents.currentInteractableRigidbody.position = interactableComponents.hand.position;
            // interactableComponents.currentInteractableRigidbody.rotation = interactableComponents.hand.rotation;
            
            UpdateInteractableVelocity();
        }
    }

    public void Interact(Transform interactorTransform)
    {
        if (interactableComponents.currentInteractableRigidbody != null)
        {
            // interactableComponents.currentInteractableRigidbody.isKinematic = false;
            interactableComponents.currentInteractableRigidbody.useGravity = true;
            interactableComponents.currentInteractableCollider.enabled = true;
            
            // var interactableGameObject = interactorTransform.gameObject;
            // interactableComponents.currentInteractableRigidbody = interactorTransform.GetComponent<Rigidbody>();
            // interactableComponents.currentInteractableCollider = interactorTransform.GetComponent<Collider>();
            interactableComponents.currentInteractableRigidbody = transform.GetComponent<Rigidbody>();
            interactableComponents.currentInteractableCollider = transform.GetComponent<Collider>();
            
            // interactableComponents.currentInteractableRigidbody.isKinematic = true;
            interactableComponents.currentInteractableRigidbody.useGravity = false;
            interactableComponents.currentInteractableCollider.enabled = false;
        }
        else
        {
            interactableComponents.currentInteractableRigidbody = transform.GetComponent<Rigidbody>();
            interactableComponents.currentInteractableCollider = transform.GetComponent<Collider>();
            
            // interactableComponents.currentInteractableRigidbody.isKinematic = true;
            interactableComponents.currentInteractableRigidbody.useGravity = false;
            interactableComponents.currentInteractableCollider.enabled = false;
        }
        
        // interactableComponents.currentInteractableRigidbody.position = interactableComponents.hand.position;
        // interactableComponents.currentInteractableRigidbody.rotation = interactableComponents.hand.rotation;
        UpdateInteractableVelocity();

        interactableComponents.isHoldingFireExtinguisher = true;
    }

    private void UpdateInteractableVelocity()
    {
        // TODO: replace setting absolute positions and rely on speed (approximate target location with smoother transitions)
        
        Vector3 DirectionToHand = interactableComponents.hand.position -
                                   interactableComponents.currentInteractableRigidbody.position;
        float DistanceToHand = DirectionToHand.magnitude;

        interactableComponents.currentInteractableRigidbody.velocity = 
            DirectionToHand * DistanceToHand * _interactableSpeed;
    }

    public string GetInteractText()
    {
        return interactText;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
