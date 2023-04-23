using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float xSensitivity, ySensitivity;
    float xRotation, yRotation, zRotation;
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
        Cursor.visible = false;
        Application.targetFrameRate = 60;
        
    }

    private void FixedUpdate() 
    {
        if (!allowedToLook) return;

        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * xSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * ySensitivity;

        // add flipped controls if upside down (only side by side motion controlled by yRotation)
        if (IsUpsideDown())
        {
            yRotation -= mouseX; 
            xRotation -= mouseY;
            zRotation = transform.rotation.z;
            
            // Debug.Log("Upside Down...");
        }
        else
        {
            yRotation += mouseX; 
            xRotation -= mouseY;
            zRotation = transform.rotation.z;
            
            // Debug.Log("Upside Up...");
        }
        
        // yRotation += mouseX; 
        // xRotation -= mouseY;
        // zRotation = transform.rotation.z;

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }   

    public void AllowLook(bool canLook) {
        allowedToLook = canLook;
    }

    private bool IsUpsideDown()
    {
        // FIXME: z rotation doesn't matter (changes in inspector 180/-180, but actual value does not change)
        // need to take the mod (%) of 360 (actual value keeps accumulating past 360 and -360)
        // between 90 and 270 or -90 and -270 (can just take abs) -- symmetric if divisor is positive (360 for our case)
        // 5 % 4 = 1
        // -5 % 4 = -1
        // Debug.Log("x rotation = " + xRotation);
        // Debug.Log("z rotation = " + zRotation);
        
        // if xRotation is >90 or <-90 OR if z-rotation is 180/-180 (just through observations)
        // just based on transform (don't check orientation)
        // need xRotation because flip is along x-direction, not yRotation
        var xRotationStandard = Mathf.Abs(xRotation % 360);
        
        // Debug.Log("x rotation standard = " + xRotationStandard);
        
        if (xRotationStandard is > 90 and < 270)
        {
            return true;
        }

        return false;
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
