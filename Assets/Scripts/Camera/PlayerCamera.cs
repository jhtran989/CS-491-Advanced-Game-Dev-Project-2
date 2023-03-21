using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float xSensitivity, ySensitivity;
    float xRotation, yRotation;
    public Transform orientation;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate() {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * xSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * ySensitivity;

        xRotation -= mouseY;
        // xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        yRotation += mouseX;
        
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }   

}
