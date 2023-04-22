using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DropInteractable : MonoBehaviour
{
    [SerializeField]
    private Camera playerCamera;
    // private Transform hand;
    
    [FormerlySerializedAs("interactableComponents")] [SerializeField]
    private FireExtinguisherInteractableComponents fireExtinguisherInteractableComponents;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // FIXME: race condition when both checking for interaction and drop...easiest way is to bind to different keys
        // TODO: rebind to right mouse click
        if (Input.GetMouseButton(1))
        {
            if (fireExtinguisherInteractableComponents.currentInteractableRigidbody != null)
            {
                // interactableComponents.currentInteractableRigidbody.isKinematic = false;
                fireExtinguisherInteractableComponents.currentInteractableRigidbody.useGravity = true;
                fireExtinguisherInteractableComponents.currentInteractableCollider.enabled = true;
                
                fireExtinguisherInteractableComponents.currentInteractableRigidbody.AddForce(playerCamera.transform.forward, ForceMode.Impulse);
                
                fireExtinguisherInteractableComponents.currentInteractableRigidbody.useGravity = false;
                fireExtinguisherInteractableComponents.currentInteractableRigidbody = null;
                fireExtinguisherInteractableComponents.currentInteractableCollider = null;
            }
            
            fireExtinguisherInteractableComponents.isHoldingFireExtinguisher = false;
            
            // hide the UI
            fireExtinguisherInteractableComponents.PlayerInteractUIIconFireExtinguisher.Hide();
        }

        // if (interactableComponents.currentInteractableRigidbody)
        // {
        //     interactableComponents.currentInteractableRigidbody.position = hand.position;
        //     interactableComponents.currentInteractableRigidbody.rotation = hand.rotation;
        // }
    }
}
