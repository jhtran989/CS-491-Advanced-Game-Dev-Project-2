using System.Collections;
using System.Collections.Generic;
using UI_Elements;
using UnityEngine;
using UnityEngine.Assertions;

// FIXME: DoorInteractable already exists in tutorial scripts...
public class DoorInteractableStationary : AbstractInteractable, IInteractableDoor
{
    public Animator doorAnimator;
    
    private GlobalDoorManager _globalDoorManager;
    private DoorController _doorController;
    
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
        _doorController = transform.GetComponentInParent<DoorController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact(Transform interactorTransform)
    {
        Debug.Log("Door interact...");
        
        if (_powerCharge.CurrentNumPowerCharges > 0)
        {
            if (!_unlockFinish && _unlockCondition)
            {
                doorAnimator.SetTrigger(Constants.OpenDoorTrigger);
                doorOpenPowerCharge?.Invoke();
                _unlockFinish = true;
                        
                // TODO: need to make corresponding fire visible and set fire to present
                _doorController.nextDoorFire.gameObject.SetActive(true);
                
                // set fire present to true for updated oxygen...
                _globalDoorManager.SetFirePresent();

                // update next room
                FireSpawn.updateCurrentRoom?.Invoke(_doorController.nextDoorObject);
                        
                Debug.Log("fire present: " + _globalDoorManager.oxygen.firePresent);
            }
        }
        else
        {
            // TODO: maybe give message that door charges are out?
            Debug.Log("Out of door charges...");
        }
    }
    
    public void UnlockDoor()
    {
        doorAnimator.SetBool(Constants.UnlockDoorBool, true);
    }
}
