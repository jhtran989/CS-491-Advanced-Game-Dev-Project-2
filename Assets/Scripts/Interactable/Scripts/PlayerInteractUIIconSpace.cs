using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class PlayerInteractUIIconSpace : MonoBehaviour {

    [SerializeField] private GameObject containerGameObject;
    [FormerlySerializedAs("playerInteract")] [SerializeField] 
    private PlayerInteractDoor playerInteractDoor;

    private void Update()
    {
        var interactableObject = playerInteractDoor.GetInteractableObject();

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