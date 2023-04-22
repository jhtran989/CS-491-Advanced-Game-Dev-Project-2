using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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

    /**********************************************************************/
    // get objects that influence when a door is unlocked
    // without need of doing spatial checks...
    [Space, Header("Fire")] 
    public string fireObjectName;
    
    [FormerlySerializedAs("fire")] [FormerlySerializedAs("_fire")] 
    public Fire currentDoorFire;
    
    private bool _fireCheck = false;

    [FormerlySerializedAs("nextFire")] 
    public Fire nextDoorFire;

    // FIXME: change fire scripts to rooms...
    // TODO: update unlocked boundary room controllers (or in controller itself) EVERY TIME A ROOM IS UNLOCKED
    public RoomController[] boundaryRoomControllersList;
    private RoomController[] _unlockedBoundaryRoomControllersList;

    public GameObject nextDoorObject;
    
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
        // FIXME: don't need to rely on string anymore since everything is in the same scene (just drag in inspector)
        // FIXME: need to check for empty string instead of null
        // get the corresponding fire and terminal (if applicable -- should be null otherwise)
        
        // ALL rooms will have a fire to check
        _fireCheck = true;
        
        // only doors leading OUT of a room with a terminal can be checked (NOT initial three doors)
        if (terminalGameObject != null)
        {
            // IMPORTANT: need to include inactive in search
            // terminalGameObject = GameObject.Find(terminalObjectName);
            _terminalTrigger = terminalGameObject
                .GetComponentInChildren<TerminalTrigger>(true);
            _terminalController = terminalGameObject
                .GetComponentInChildren<TerminalController>(true);
            _terminalCheck = true;
        }

        _globalDoorManager = transform.GetComponentInParent<GlobalDoorManager>();
        
        // FIXME: update to get top level parent (two levels up)
        nextDoorObject = nextDoorFire.transform.parent.parent.gameObject;
        
        // TODO: update to get top level parent (two levels up)
        _roomManager = transform.parent.parent.GetComponent<RoomManager>();
        
        // if EscapePodDoor is part of the boundary rooms
        if (boundaryRoomControllersList.Contains(_roomManager.initialRoomController))
        {
            _doorReachedCheck = true;
        }
        else
        {
            _doorReachedCheck = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
        // TODO: check boundary, update fire AND terminal of the door (ON THE ROOM NOT UNLOCKED)
        // TODO: move stuff to room controller (not tied to door anymore...)
        if (doorOptionsEnum == DoorOptionsEnum.DoorReached)
        {
            return _doorReachedCheck;
        }
        else if (doorOptionsEnum == DoorOptionsEnum.Fire)
        {
            return !_fireCheck || currentDoorFire.unlockDoor;
        } 
        else if (doorOptionsEnum == DoorOptionsEnum.Terminal)
        {
            return !_terminalCheck || _terminalController.unlockDoor;
        }

        // default to false for testing
        return false;
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
        Debug.Log("Door manager, check unlock door");
        
        if (doorOptionCheck)
        {
            var unlockCondition = true;
            
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
                // doorTrigger.UnlockDoor();
                doorInteractableStationary.UnlockDoor();
                doorOptionCheck = false;
            }
        }
    }
}
