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

    // private void Awake()
    // {
    //     // FIXME FINAL: set to true
    //     _unlockCondition = false;
    //     _unlockFinish = false;
    // }

    protected override void Awake()
    {
        base.Awake();
        
        // FIXME FINAL: set to true
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
            Debug.Log(!_unlockFinish + ", " + _unlockCondition);
            _doorController.CheckUnlockDoor();
            
            if (!_unlockFinish && _unlockCondition)
            {
                Debug.Log("Opening door...");
                
                doorAnimator.SetTrigger(Constants.OpenDoorTrigger);
                doorOpenPowerCharge?.Invoke();

                // TODO: check the NEXT ROOM if it was already unlocked
                // update next room
                var nextRoomObject = _doorController.nextRoomObject;
                var nextRoomController = 
                    _doorController.nextRoomObject.GetComponent<RoomController>();
                
                // need to recalculate fires as well BEFORE fire spawning (and updating room)
                nextRoomController.RecalculateNumActiveFires();
                
                FireSpawn.updateCurrentRoom?.Invoke(nextRoomObject);
                
                // TODO: only create new fire ONLY if was FIRST reached (ORDER MATTERS - before updating next room for fire spawn)

                // FIXME FINAL: don't activate initial fires
                // if (nextRoomController.InitialUnlock)
                // {
                //     // TODO: need to make corresponding fire visible and set fire to present
                //     _doorController.nextRoomFire.gameObject.SetActive(true);
                //
                //     // set fire present to true for updated oxygen...
                //     _globalDoorManager.SetFirePresent();
                //
                //     Debug.Log("fire present: " + _globalDoorManager.oxygen.firePresent);
                //     
                //     // TODO: need to update initial unlock right after
                //     nextRoomController.UpdateInitialUnlock();
                //     
                //     // change the lights to red (initial fire when unlocking room)
                //     nextRoomController.UpdateRedLights();
                // }
                
                _unlockFinish = true;
                
                // FIXME FINAL: disable collider to not make it interactable anymore
                _collider = GetComponent<Collider>();
                DisableInteract();
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
