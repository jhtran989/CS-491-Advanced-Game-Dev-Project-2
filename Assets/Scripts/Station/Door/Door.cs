using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public Animator doorAnimator;
    
    public delegate void DoorDelegate();

    public static DoorDelegate DoorOpen;
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(Constants.PlayerTag))
        {
            doorAnimator.SetTrigger(Constants.OpenDoorTrigger);
            DoorOpen?.Invoke();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Constants.PlayerTag))
        {
            doorAnimator.SetTrigger(Constants.CloseDoorTrigger);
        }
    }
}
