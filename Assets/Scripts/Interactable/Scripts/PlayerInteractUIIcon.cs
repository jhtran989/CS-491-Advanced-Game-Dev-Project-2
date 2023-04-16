using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PlayerInteractUIIcon : MonoBehaviour {

    [SerializeField] private GameObject containerGameObject;
    [SerializeField] private PlayerInteract playerInteract;

    private void Update()
    {
        var interactableObject = playerInteract.GetInteractableObject();

        if (interactableObject != null) {
            Show(interactableObject);
            
            // Debug.Log("Showing E...");
        } else {
            Hide();
        }
    }

    private void Show(IInteractable interactable) {
        containerGameObject.SetActive(true);
    }

    private void Hide() {
        containerGameObject.SetActive(false);
    }

}