using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerInteract : MonoBehaviour
{
    private FireExtinguisherInteractableComponents _fireExtinguisherInteractableComponents;
    private IInteractable _currentInteractable;
    private AbstractInteractable _currentAbstractInteractable;

    private void Awake()
    {
        _fireExtinguisherInteractableComponents = GetComponent<FireExtinguisherInteractableComponents>();
        _currentInteractable = null;
        _currentAbstractInteractable = null;
    }

    private void Update() {
        // TODO: abstract to allow multiple keys depending on type of interaction
        // KeyCode.E
        if (Input.GetKeyDown(Constants.InteractableKey)) {
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
            // check if it is NOT a door
            // interactable is not IInteractableDoor
            // interactable.GetType().IsInstanceOfType(typeof(IInteractableDoor))
            // && !Utilities.IsOfType(interactable, typeof(IInteractableDoor))
            if (collider.TryGetComponent(out IInteractable interactable)
                && !Utilities.IsOfType(interactable, typeof(IInteractableDoor))) {
                interactableList.Add(interactable);

                // if (collider.TryGetComponent(out TerminalInteractable terminalInteractable))
                // {
                //     Debug.Log("TERMINAL");
                // }
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
        
        // FIXME FINAL: highlight...
        // just the fire extinguisher for now (ONLY one that works...because of renderer)
        if (closestInteractable != null && closestInteractable is FireExtinguisherInteractable)
        {
            if (_currentInteractable == null)
            {
                // enable highlight
                _currentInteractable = closestInteractable;
            
                // cast to AbstractInteractable and ACTIVATE highlight
                _currentAbstractInteractable = (AbstractInteractable)_currentInteractable;

                if (_currentAbstractInteractable != null)
                {
                    Assert.IsNotNull(_currentAbstractInteractable.customOutline);
                    _currentAbstractInteractable.customOutline.EnableOutline();
                }
            } 
            else if (_currentInteractable != closestInteractable)
            {
                // disable first
                
                // disable highlight
                _currentAbstractInteractable.customOutline.DisableOutline();
            
                // reset current stuff
                _currentInteractable = null;
                _currentAbstractInteractable = null;
                
                // then reenable highlight
                
                // enable highlight
                _currentInteractable = closestInteractable;
            
                // cast to AbstractInteractable and ACTIVATE highlight
                _currentAbstractInteractable = (AbstractInteractable)_currentInteractable;

                if (_currentAbstractInteractable != null)
                {
                    Assert.IsNotNull(_currentAbstractInteractable.customOutline);
                    _currentAbstractInteractable.customOutline.EnableOutline();
                }
            }
        }
        else
        {
            if (_currentInteractable != null)
            {
                // disable highlight
                _currentAbstractInteractable.customOutline.DisableOutline();
            
                // reset current stuff
                _currentInteractable = null;
                _currentAbstractInteractable = null;
            }
        }

        return closestInteractable;
    }

}