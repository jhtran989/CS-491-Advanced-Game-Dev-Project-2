using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Serialization;

public class PlayerInteractUIIconFireExtinguisher : MonoBehaviour {
    [FormerlySerializedAs("containerGameObject")] [SerializeField] 
    private GameObject[] containerGameObjectList;

    public bool isShowing;

    private void Awake()
    {
        isShowing = false;
        
        // initially hide the UI
        Hide();
    }

    // private void Update()
    // {
    //     var interactableObject = playerInteractDoor.GetInteractableObject();
    //
    //     if (interactableObject != null) {
    //         Show(interactableObject);
    //         
    //         // Debug.Log("Showing E...");
    //     } else {
    //         Hide();
    //     }
    // }

    public void Show() {
        foreach (var containerGameObject in containerGameObjectList)
        {
            containerGameObject.SetActive(true);
        }
    }

    public void Hide() {
        foreach (var containerGameObject in containerGameObjectList)
        {
            containerGameObject.SetActive(false);
        }
    }
}