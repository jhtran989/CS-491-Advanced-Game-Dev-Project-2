using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FireExtinguisherInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] 
    private string interactText;
    
    [FormerlySerializedAs("interactableComponents")] [SerializeField]
    private FireExtinguisherInteractableComponents fireExtinguisherInteractableComponents;
    
    private float _interactableSpeed;

    private Rigidbody _rigidbodyFireExtinguisher;

    private void Awake()
    {
        fireExtinguisherInteractableComponents.isHoldingFireExtinguisher = false;
        _interactableSpeed = 17.0f;
        
        _rigidbodyFireExtinguisher = GetComponent<Rigidbody>();
        
        // set initial velocity to 0 (gravity is false now)
        _rigidbodyFireExtinguisher.velocity = Vector3.zero;
        _rigidbodyFireExtinguisher.angularVelocity = Vector3.zero;
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
        if (fireExtinguisherInteractableComponents.isHoldingFireExtinguisher)
        {
            // interactableComponents.currentInteractableRigidbody.position = interactableComponents.hand.position;
            // interactableComponents.currentInteractableRigidbody.rotation = interactableComponents.hand.rotation;
            
            UpdateInteractableVelocity();
        }
    }

    public void Interact(Transform interactorTransform)
    {
        if (fireExtinguisherInteractableComponents.currentInteractableRigidbody != null)
        {
            // interactableComponents.currentInteractableRigidbody.isKinematic = false;
            fireExtinguisherInteractableComponents.currentInteractableRigidbody.useGravity = true;
            fireExtinguisherInteractableComponents.currentInteractableCollider.enabled = true;
            
            // var interactableGameObject = interactorTransform.gameObject;
            // interactableComponents.currentInteractableRigidbody = interactorTransform.GetComponent<Rigidbody>();
            // interactableComponents.currentInteractableCollider = interactorTransform.GetComponent<Collider>();
            fireExtinguisherInteractableComponents.currentInteractableRigidbody = transform.GetComponent<Rigidbody>();
            fireExtinguisherInteractableComponents.currentInteractableCollider = transform.GetComponent<Collider>();
            
            // interactableComponents.currentInteractableRigidbody.isKinematic = true;
            fireExtinguisherInteractableComponents.currentInteractableRigidbody.useGravity = false;
            // interactableComponents.currentInteractableRigidbody.freezeRotation = true;
            fireExtinguisherInteractableComponents.currentInteractableCollider.enabled = false;
        }
        else
        {
            fireExtinguisherInteractableComponents.currentInteractableRigidbody = transform.GetComponent<Rigidbody>();
            fireExtinguisherInteractableComponents.currentInteractableCollider = transform.GetComponent<Collider>();
            
            // interactableComponents.currentInteractableRigidbody.isKinematic = true;
            fireExtinguisherInteractableComponents.currentInteractableRigidbody.useGravity = false;
            // interactableComponents.currentInteractableRigidbody.freezeRotation = true;
            fireExtinguisherInteractableComponents.currentInteractableCollider.enabled = false;
        }
        
        // interactableComponents.currentInteractableRigidbody.position = interactableComponents.hand.position;
        // interactableComponents.currentInteractableRigidbody.rotation = interactableComponents.hand.rotation;
        UpdateInteractableVelocity();

        fireExtinguisherInteractableComponents.isHoldingFireExtinguisher = true;
    }

    private void UpdateInteractableVelocity()
    {
        // TODO: replace setting absolute positions and rely on speed (approximate target location with smoother transitions)
        
        Vector3 DirectionToHand = fireExtinguisherInteractableComponents.hand.position -
                                   fireExtinguisherInteractableComponents.currentInteractableRigidbody.position;
        float DistanceToHand = DirectionToHand.magnitude;

        fireExtinguisherInteractableComponents.currentInteractableRigidbody.velocity = 
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
