using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DropInteractable : MonoBehaviour
{
    [SerializeField]
    private Camera playerCamera;
    // private Transform hand;
    
    [SerializeField]
    private InteractableComponents interactableComponents;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // FIXME: race condition when both checking for interaction and drop...easiest way is to bind to different keys
        if (Input.GetKeyDown(Constants.DropInteractableKey))
        {
            if (interactableComponents.currentInteractableRigidbody != null)
            {
                // interactableComponents.currentInteractableRigidbody.isKinematic = false;
                interactableComponents.currentInteractableRigidbody.useGravity = true;
                interactableComponents.currentInteractableCollider.enabled = true;
                
                interactableComponents.currentInteractableRigidbody.AddForce(playerCamera.transform.forward, ForceMode.Impulse);
                
                interactableComponents.currentInteractableRigidbody.useGravity = false;
                interactableComponents.currentInteractableRigidbody = null;
                interactableComponents.currentInteractableCollider = null;
            }
            
            interactableComponents.isHoldingFireExtinguisher = false;
        }

        // if (interactableComponents.currentInteractableRigidbody)
        // {
        //     interactableComponents.currentInteractableRigidbody.position = hand.position;
        //     interactableComponents.currentInteractableRigidbody.rotation = hand.rotation;
        // }
    }
}
