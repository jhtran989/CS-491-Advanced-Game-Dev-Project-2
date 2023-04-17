using System;
using System.Collections;
using System.Collections.Generic;
using UI_Elements;
using UnityEngine;
using UnityEngine.Assertions;

public class Door : MonoBehaviour
{
    public Animator doorAnimator;
    
    private GlobalDoorManager _globalDoorManager;
    
    public delegate void DoorDelegate();
    public static DoorDelegate doorOpenPowerCharge;

    private HUDManager _hudManager;
    private PowerCharge _powerCharge;

    private bool _unlockCondition;
    private bool _unlockFinish;

    public bool UnlockCondition
    {
        get => _unlockCondition;
        set => _unlockCondition = value;
    }

    private void Awake()
    {
        _unlockCondition = false;
        _unlockFinish = false;
    }

    private void Start()
    {
        var mainCanvasObject = GameObject.Find(Constants.MainCanvasObjectName);
        Assert.IsNotNull(mainCanvasObject);
        
        _hudManager = mainCanvasObject.transform.Find(Constants.HUDObjectName).gameObject
            .GetComponent<HUDManager>();
        Assert.IsNotNull(_hudManager);
        
        // should search RECURSIVELY down into the children
        _powerCharge = _hudManager.DoorPowerBarObject.GetComponentInChildren<PowerCharge>();
        Assert.IsNotNull(_powerCharge);
        
        // should go up to the top level object
        _globalDoorManager = transform.GetComponentInParent<GlobalDoorManager>();
    }

    // FIXME: move to a button press instead of entering collider...
    // private void OnTriggerEnter(Collider other)
    // {
    //     if (other.CompareTag(Constants.PlayerTag))
    //     {
    //         doorAnimator.SetTrigger(Constants.OpenDoorTrigger);
    //         DoorOpen?.Invoke();
    //     }
    // }

    // FIXME: need to restrict when a door charge is used (and only when there are door charges left)
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(Constants.PlayerTag))
        {
            if (Input.GetKeyDown(Constants.FireKey))
            {
                if (_powerCharge.CurrentNumPowerCharges > 0)
                {
                    if (!_unlockFinish && _unlockCondition)
                    {
                        doorAnimator.SetTrigger(Constants.OpenDoorTrigger);
                        doorOpenPowerCharge?.Invoke();
                        _unlockFinish = true;
                        
                        // set fire present to true for updated oxygen...
                        _globalDoorManager.oxygen.firePresent = true;
                        
                        Debug.Log("fire present: " + _globalDoorManager.oxygen.firePresent);
                    }
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(Constants.PlayerTag))
        {
            doorAnimator.SetTrigger(Constants.CloseDoorTrigger);
        }
    }

    public void UnlockDoor()
    {
        doorAnimator.SetBool(Constants.UnlockDoorBool, true);
    }
}
