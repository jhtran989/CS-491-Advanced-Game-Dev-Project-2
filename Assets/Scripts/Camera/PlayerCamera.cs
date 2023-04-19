using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float xSensitivity, ySensitivity;
    float xRotation, yRotation;
    public Transform orientation;
    private bool allowedToLook = true;
    
    // FIXME: freeze the rotation of the transform
    // instead of enabling/disabling game object, just edit the Camera component
    private Camera playerCamera;
    private Rigidbody playerCameraRigidbody;

    private void Awake()
    {
        playerCamera = GetComponent<Camera>();
        // playerCameraRigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        TerminalInteractable.TerminalTriggerPlayerEnter += DisableCamera;
        TerminalController.TerminalControllerPlayerLeave += EnableCamera;
    }

    private void OnDisable()
    {
        TerminalInteractable.TerminalTriggerPlayerEnter -= DisableCamera;
        TerminalController.TerminalControllerPlayerLeave -= EnableCamera;
    }

    void Start()
    {       
        Cursor.lockState = CursorLockMode.Locked;
        Application.targetFrameRate = 60;
        Cursor.visible = false;
    }

    private void FixedUpdate() 
    {
        if (!allowedToLook) return;

        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * xSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * ySensitivity;

        
        yRotation += mouseX; 
        xRotation -= mouseY;

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }   

    public void AllowLook(bool canLook) {
        allowedToLook = canLook;
    }

    public void EnableCamera()
    {
        // gameObject.SetActive(true);
        // playerCamera.enabled = true;
        // playerCameraRigidbody.constraints &= ~RigidbodyConstraints.FreezeRotation;
        allowedToLook = true;
    }
    
    private void DisableCamera()
    {
        // gameObject.SetActive(false);
        // playerCamera.enabled = false;
        // playerCameraRigidbody.constraints |= RigidbodyConstraints.FreezeRotation;
        allowedToLook = false;
    }
}
