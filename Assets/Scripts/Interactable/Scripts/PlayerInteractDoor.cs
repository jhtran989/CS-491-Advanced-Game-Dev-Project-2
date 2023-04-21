using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractDoor : MonoBehaviour
{
    private FireExtinguisherInteractableComponents _fireExtinguisherInteractableComponents;

    private void Awake()
    {
        _fireExtinguisherInteractableComponents = GetComponent<FireExtinguisherInteractableComponents>();
    }

    private void Update() {
        // TODO: abstract to allow multiple keys depending on type of interaction
        // KeyCode.E
        if (Input.GetKeyDown(Constants.OpenDoorKey)) {
            IInteractable interactable = GetInteractableObject();
            if (interactable != null) {
                Debug.Log("Found interactable...");
                interactable.Interact(transform);
            }
        }
    }

    public IInteractable GetInteractableObject() {
        List<IInteractable> interactableList = new List<IInteractable>();
        float interactRange = 3f;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliderArray) {
            // check if it IS a door
            // && interactable is IInteractableDoor
            // && Utilities.IsOfType(interactable, typeof(IInteractableDoor))
            if (collider.TryGetComponent(out IInteractable interactable)
                && Utilities.IsOfType(interactable, typeof(IInteractableDoor))) {
                interactableList.Add(interactable);

                if (collider.TryGetComponent(out TerminalInteractable terminalInteractable))
                {
                    Debug.Log("TERMINAL");
                }
            }
        }

        IInteractable closestInteractable = null;
        foreach (IInteractable interactable in interactableList) {
            if (closestInteractable == null) {
                closestInteractable = interactable;
            } else {
                if (Vector3.Distance(transform.position, interactable.GetTransform().position) < 
                    Vector3.Distance(transform.position, closestInteractable.GetTransform().position)) {
                    // Closer
                    closestInteractable = interactable;
                }
            }
        }

        return closestInteractable;
    }

}