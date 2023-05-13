using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Serialization;

public class DoorController : MonoBehaviour
{
    [FormerlySerializedAs("door")] [Space, Header("Door")]
    public DoorTrigger doorTrigger;

    public DoorInteractableStationary doorInteractableStationary;
    
    /**********************************************************************/
    // [Space, Header("Managers")] 
    
    // TODO: put in inspector (PREFAB)
    private RoomManager _roomManager;
    
    private GlobalDoorManager _globalDoorManager;

    private bool _doorReachedCheck;

    // public bool DoorReachedCheck => _doorReachedCheck;

    /**********************************************************************/
    // get objects that influence when a door is unlocked
    // without need of doing spatial checks...
    [Space, Header("Fire")] 
    public string fireObjectName;
    
    [FormerlySerializedAs("currentDoorFire")] [FormerlySerializedAs("fire")] [FormerlySerializedAs("_fire")] 
    public Fire currentRoomFire;
    
    private bool _fireCheck = false;

    [FormerlySerializedAs("nextDoorFire")] [FormerlySerializedAs("nextFire")] 
    public Fire nextRoomFire;

    // FIXME: change fire scripts to rooms...
    // TODO: update unlocked boundary room controllers (or in controller itself) EVERY TIME A ROOM IS UNLOCKED
    public List<RoomController> boundaryRoomControllersList;
    private List<RoomController> _unlockedBoundaryRoomControllersList;

    [FormerlySerializedAs("nextDoorObject")] 
    public GameObject nextRoomObject;
    
    /**********************************************************************/
    [Space, Header("Terminal")] 
    public string terminalObjectName;
    
    [FormerlySerializedAs("_terminalGameObject")] 
    public GameObject terminalGameObject;
    
    private TerminalTrigger _terminalTrigger;
    private TerminalController _terminalController;
    private bool _terminalCheck = false;

    public bool doorOptionCheck;

    private static readonly string EmptyString = Utilities.EmptyString;

    public delegate void UnlockDoorDelegate();
    public static UnlockDoorDelegate unlockDoor;

    /******************************************************************/
    
    private enum DoorOptionsEnum
    {
        DoorReached,
        Fire,
        Terminal
    }
    
    private class DoorOptions
    {
        
    }

    private Dictionary<DoorOptionsEnum, List<DoorOptions>> doorOptionsDict = 
        new Dictionary<DoorOptionsEnum, List<DoorOptions>>();

    /******************************************************************/

    private void Awake()
    {
        // returns only the first component found - only the DoorCenterFrame should have it
        doorTrigger = gameObject.GetComponentInChildren<DoorTrigger>();
        // also find the interactable script
        doorInteractableStationary = gameObject.GetComponentInChildren<DoorInteractableStationary>();
        
        doorOptionCheck = true;
        
        // initially set unlocked rooms to empty (updated when adding new room)
        _unlockedBoundaryRoomControllersList = new List<RoomController>();
        
        // updated in fire spawn when adding a new room
        _doorReachedCheck = false;
        
        // ALL rooms will have a fire to check
        // FIXME FINAL: set to false
        _fireCheck = false;
        
        // initially set terminal check to false, then update when adding rooms
        _terminalCheck = false;
    }

    private void OnEnable()
    {
        unlockDoor += CheckUnlockDoor;
    }

    private void OnDisable()
    {
        unlockDoor -= CheckUnlockDoor;
    }

    // Start is called before the first frame update
    void Start()
    {
        // TODO: update to get top level parent (two levels up)
        _roomManager = transform.parent.parent.GetComponent<RoomManager>();
        _globalDoorManager = transform.GetComponentInParent<GlobalDoorManager>();
        
        
        
        
        // FIXME: don't need to rely on string anymore since everything is in the same scene (just drag in inspector)
        // FIXME: need to check for empty string instead of null
        // get the corresponding fire and terminal (if applicable -- should be null otherwise)

        // FIXME: moved below when updating door reached check
        // // only doors leading OUT of a room with a terminal can be checked (NOT initial three doors)
        // if (terminalGameObject != null)
        // {
        //     // IMPORTANT: need to include inactive in search
        //     // terminalGameObject = GameObject.Find(terminalObjectName);
        //     _terminalTrigger = terminalGameObject
        //         .GetComponentInChildren<TerminalTrigger>(true);
        //     _terminalController = terminalGameObject
        //         .GetComponentInChildren<TerminalController>(true);
        //     _terminalCheck = true;
        // }

        // FIXME: done when adding a new room
        // FIXME: update to get top level parent (two levels up)
        // nextDoorObject = nextDoorFire.transform.parent.parent.gameObject;

        // FIXME: done in fire spawn
        // if EscapePodDoor is part of the boundary rooms
        // if (boundaryRoomControllersList.Contains(_roomManager.initialRoomController))
        // {
        //     _doorReachedCheck = true;
        // }
        // else
        // {
        //     _doorReachedCheck = false;
        // }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateDoorReached(RoomController currentRoomController)
    {
        // do not do redundant checks if door was already reached...
        if (!_doorReachedCheck)
        {
            // TODO: update the next room if door reached was NOT ALREADY true
            _doorReachedCheck = true;

            if (!_unlockedBoundaryRoomControllersList.Contains(currentRoomController))
            {
                _unlockedBoundaryRoomControllersList.Add(currentRoomController);
            }
        
            // TODO: currentDoorFire, _terminalController, nextDoorFire
            currentRoomFire = GetRoomFire(currentRoomController);
            _terminalController = GetRoomTerminalController(currentRoomController);

            List<RoomController> remainingLockedRoomControllerList =
                boundaryRoomControllersList.Except(_unlockedBoundaryRoomControllersList).ToList();
        
            Debug.Log("door controller: " + gameObject.name);
            Debug.Log("unlocked size: " + _unlockedBoundaryRoomControllersList.Count); 
            Debug.Log("remaining size: " + remainingLockedRoomControllerList.Count); 
        
            // there should ONLY be 1 other room left (other side of door)
            Assert.IsTrue(remainingLockedRoomControllerList.Count == 1);

            RoomController nextRoomController = remainingLockedRoomControllerList[0];
            nextRoomFire = GetRoomFire(nextRoomController);
            
            // TODO: need to update next room object
            nextRoomObject = nextRoomController.gameObject;
            
            // FIXME FINAL: check unlock door
            CheckUnlockDoor();
        }
    }

    private Fire GetRoomFire(RoomController roomController)
    {
        // find FIRST child, depth first search (should be fine since spawned fires are BELOW the main fire)
        // IMPORTANT: need to include inactive in search
        return roomController.roomFireParent.GetComponentInChildren<Fire>(true);
    }

    private TerminalController GetRoomTerminalController(RoomController roomController)
    {
        var terminalController = roomController.terminalController;
        
        // only doors leading OUT of a room with a terminal can be checked (NOT initial three doors)
        if (terminalController != null)
        {
            // IMPORTANT: need to include inactive in search
            terminalGameObject = transform.parent.gameObject;
            _terminalTrigger = terminalGameObject
                .GetComponentInChildren<TerminalTrigger>(true);
            
            // FIXME FINAL: no more terminal checks
            _terminalCheck = false;
        }
        
        return terminalController;
    }

    /// <summary>
    ///
    /// logic is dependent on the corresponding check (if is not null) and if so, check the unlock condition
    /// 
    /// </summary>
    /// <param name="doorOptionsEnum"></param>
    /// <returns></returns>
    private bool CheckDoorOptions(DoorOptionsEnum doorOptionsEnum)
    {
        // default to false for testing
        var finalDoorCheck = false;
        
        // TODO: check when door is unlocked
        // TODO: check boundary, update fire AND terminal of the door (ON THE ROOM NOT UNLOCKED)
        // TODO: move stuff to room controller (not tied to door anymore...)
        if (doorOptionsEnum == DoorOptionsEnum.DoorReached)
        {
            finalDoorCheck = _doorReachedCheck;
        }
        else if (doorOptionsEnum == DoorOptionsEnum.Fire)
        {
            finalDoorCheck = !_fireCheck || currentRoomFire.unlockDoor;
        } 
        else if (doorOptionsEnum == DoorOptionsEnum.Terminal)
        {
            finalDoorCheck = !_terminalCheck || _terminalController.unlockDoor;
        }
        
        Debug.Log(doorOptionsEnum + ", " + finalDoorCheck);
        
        return finalDoorCheck;
    }

    private bool CheckBoundaryRoomUnlock()
    {
        // for

        return false;
    }

    /// <summary>
    /// Should be moved to each instance where one portion of unlocking the door could occur
    /// </summary>
    public void CheckUnlockDoor()
    {
        Debug.Log("Checking unlock door...");
        
        if (doorOptionCheck)
        {
            var unlockCondition = true;
            
            // FIXME: assume doorReachedCheck is done FIRST...
            foreach (var doorOptionsEnum in Utilities.GetValues<DoorOptionsEnum>())
            {
                unlockCondition = unlockCondition && CheckDoorOptions(doorOptionsEnum);
                // doorTrigger.UnlockCondition = unlockCondition;
                doorInteractableStationary.UnlockCondition = unlockCondition;
                
                // short circuit (break early if false)
                if (!unlockCondition)
                {
                    break;
                }
            }
            
            // FIXME: removed constraint that fire needs to be put out to interact with terminal
            // FIXME: need to abstract this into rooms...
            // if (_terminalCheck)
            // {
            //     // make terminal interactable if fire was put out
            //     if (CheckDoorOptions(DoorOptionsEnum.Fire))
            //     {
            //         // _terminalController.EnableTerminalController();
            //         _terminalTrigger.gameObject.SetActive(true);
            //     }
            // }
            
            if (unlockCondition)
            {
                Debug.Log("Door successfully unlocked");
                
                // doorTrigger.UnlockDoor();
                doorInteractableStationary.UnlockDoor();
                doorOptionCheck = false;
            }
        }
    }
}
