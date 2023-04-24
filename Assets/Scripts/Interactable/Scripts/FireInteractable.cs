using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Player;
using UnityEngine;

public class FireInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] 
    private string interactText;

    // FIXME: pulling script from ANOTHER PREFAB doesn't work...
    public PlayerMovement playerMovement;
    
    // interact radius (sphere overlap below)
    private float _interactRadius;

    private bool playerWithinRange;

    private void Awake()
    {
        playerWithinRange = false;

        _interactRadius = 5.0f;
    }

    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GameManager.instance.playerObject.GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    private void Update()
    {
        Collider playerCollider = GetPlayerCollider();

        if (!playerWithinRange && playerCollider != null)
        {
            Debug.Log("Player too close to fire...");
            
            playerMovement.UpdateOxygenRateFire?.Invoke();
            
            playerWithinRange = true;
        }
        else if (playerWithinRange && playerCollider == null)
        {
            Debug.Log("Player out of reach of fire...");
            
            playerMovement.UpdateOxygenRateExit?.Invoke();
            
            playerWithinRange = false;
        }

        // if (Input.GetKeyDown(Constants.InteractableKey)) {
        //     IInteractable interactable = GetInteractableObject();
        //     if (interactable != null) {
        //         Debug.Log("Found interactable...");
        //         interactable.Interact(transform);
        //     }
        // }
    }

    public Collider GetPlayerCollider() {
        List<IInteractable> interactableList = new List<IInteractable>();

        // TODO: add layer mask
        // int playerLayerIndex = LayerMask.NameToLayer(Constants.PlayerLayer);
        LayerMask playerLayerMask = LayerMask.GetMask(Constants.PlayerLayer);
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, _interactRadius, playerLayerMask);
        
        // foreach (Collider collider in colliderArray) {
        //     if (collider.TryGetComponent(out IInteractable interactable)
        //         && !Utilities.IsOfType(interactable, typeof(IInteractableDoor))) {
        //         interactableList.Add(interactable);
        //     }
        // }
        //
        // IInteractable closestInteractable = null;
        // foreach (IInteractable interactable in interactableList) {
        //     if (closestInteractable == null) {
        //         closestInteractable = interactable;
        //     } else {
        //         if (Vector3.Distance(transform.position, interactable.GetTransform().position) < 
        //             Vector3.Distance(transform.position, closestInteractable.GetTransform().position)) {
        //             // Closer
        //             closestInteractable = interactable;
        //         }
        //     }
        // }
        //
        // return closestInteractable;
        
        // if not empty -- there should ONLY be 1 player...
        Collider playerCollider = null;
        if (colliderArray.Any())
        {
            playerCollider = colliderArray[0];
        }

        return playerCollider;
    }

    public void Interact(Transform interactorTransform)
    {
        // throw new System.NotImplementedException();
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
