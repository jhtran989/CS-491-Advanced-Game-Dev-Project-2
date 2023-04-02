using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float xSensitivity, ySensitivity;
    float xRotation, yRotation;
    public Transform orientation;
    private bool allowedToLook = true;

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

}
