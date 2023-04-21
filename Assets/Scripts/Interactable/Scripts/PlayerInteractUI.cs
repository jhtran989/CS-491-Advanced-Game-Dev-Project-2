using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerInteractUI : MonoBehaviour {

    [SerializeField] private GameObject containerGameObject;
    [SerializeField] private PlayerInteract playerInteract;
    
    // NOT MODULAR...
    [SerializeField] private PlayerInteractDoor playerInteractDoor;
    
    [SerializeField] private TextMeshProUGUI interactTextMeshProUGUI;

    private void Update()
    {
        var interactableObject = playerInteract.GetInteractableObject();
        var interactableObjectDoor = playerInteractDoor.GetInteractableObject();

        if (interactableObject != null) {
            Show(interactableObject);
            
            // Debug.Log("Showing E...");
        } 
        else if (interactableObjectDoor != null)
        {
            Show(interactableObjectDoor);
        }
        else {
            Hide();
        }
    }

    private void Show(IInteractable interactable) {
        containerGameObject.SetActive(true);
        interactTextMeshProUGUI.text = interactable.GetInteractText();
    }

    private void Hide() {
        containerGameObject.SetActive(false);
    }

}