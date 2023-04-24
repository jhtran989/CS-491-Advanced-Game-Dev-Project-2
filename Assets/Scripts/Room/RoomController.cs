using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Room;
using UnityEngine;
using UnityEngine.Serialization;
using static RoomLocations;

public class RoomController: MonoBehaviour
{
    public RoomLocationsEnum roomLocationsEnum;
    
    [FormerlySerializedAs("fireParent")] 
    public GameObject roomFireParent;

    public GameObject roomLightParent;
    private Light[] _roomLightChildrenList;
    private Color _normalRoomLightColor;
    private Color _redRoomLightColor;
    private bool _isRedLight;
    
    // delegate to change lights back to normal
    public delegate void UpdateNormalLightDelegate();
    public static UpdateNormalLightDelegate updateNormalLight;

    public bool IsRedLight => _isRedLight;

    // relative to corresponding FIRE CONTAINER local positions
    [FormerlySerializedAs("fireLocations")] 
    public Vector3[] fireLocationPositions;
    
    private List<RoomFireEntry> _roomFireLocationsList;

    public List<DoorController> boundaryDoorControllersList;

    private bool _initialUnlock;

    public bool InitialUnlock => _initialUnlock;

    /*************************/
    // moved stuff from other scripts
    public TerminalController terminalController;
    
    /*************************/

    private int _numInitialFires;
    private int _numSpawnFires;

    private void Awake()
    {
        _roomFireLocationsList = new List<RoomFireEntry>();
        // Debug.Log("Num fires: " + GetNumActiveFires());

        _initialUnlock = true;
        
        // get the children lights
        _roomLightChildrenList = GetComponentsInChildren<Light>();
        
        // set the initial colors (normal and red)
        // _normalRoomLightColor = new Color32(236, 181, 95, 255);

        _isRedLight = false;
            
        // normal - golden
        if (!ColorUtility.TryParseHtmlString("#FFBD4E", out _normalRoomLightColor))
        {
            Debug.LogError("Invalid color for NORMAL LIGHT...");
        }
        
        // FIXME: dark red for better contrast
        // red - warm red "#FF4EB1"
        // red - dark red 
        if (!ColorUtility.TryParseHtmlString("#FF4EB1", out _redRoomLightColor))
        {
            Debug.LogError("Invalid color for RED LIGHT...");
        }
        
        // IMPORTANT - need to initially recalculate fires (especially for initial room - Escape Pod)
        RecalculateNumActiveFires();
    }

    private void OnEnable()
    {
        updateNormalLight += UpdateNormalLights;
    }
    
    private void OnDisable()
    {
        updateNormalLight -= UpdateNormalLights;
    }

    public List<RoomFireEntry> GetRoomFireLocationsList()
    {
        if (!_roomFireLocationsList.Any())
        {
            foreach (var fireLocationPosition in fireLocationPositions)
            {
                _roomFireLocationsList.Add(
                    new RoomFireEntry(roomLocationsEnum, roomFireParent, 
                        fireLocationPosition, this));
            }
        }

        return _roomFireLocationsList;
    }
    
    public int GetNumInitialFires()
    {
        // FIXME: didn't use c in name check
        
        // var currentRoomTransform = currentRoom.transform;
        var fireParentTransform = roomFireParent.transform;
        
        // if it is NOT a spawned fire
        return fireParentTransform.GetTransformCountPredicate(c => 
            !c.name.StartsWith(Constants.SpawnedFireObjectName) && c.gameObject.activeSelf);
    }

    public int GetNumSpawnFires()
    {
        // return currentRoom.transform.GetChildCountActive();
        // var currentRoomTransform = currentRoom.transform;
        var fireParentTransform = roomFireParent.transform;
        
        // if it is a spawned fire
        return fireParentTransform.GetTransformCountPredicate(c => 
            c.name.StartsWith(Constants.SpawnedFireObjectName) && c.gameObject.activeSelf);
    }
    
    public int GetNumActiveFires()
    {
        return _numInitialFires + _numSpawnFires;
    }
    
    public void RecalculateNumActiveFires()
    {
        // return both initial fires and spawned fires
        // need to check if null
        
        // numInitialFires = 0;
        // numInitialFires = _fireParentTransform.GetChildCountActive();
        
        _numInitialFires = GetNumInitialFires();
        _numSpawnFires = GetNumSpawnFires();
        
        // Debug.Log("num initial fires: " + _numInitialFires);
        // Debug.Log("num spawn fires: " + _numSpawnFires);
        // Debug.Log("num total fires: " + GetNumActiveFires());
    }

    public void UpdateBoundaryDoorReachedList()
    {
        Debug.Log("-----------------------------------------------");
        Debug.Log("boundary room check: " + this.roomLocationsEnum);

        foreach (var boundaryDoorController in boundaryDoorControllersList)
        {
            // feed itself into the update
            boundaryDoorController.UpdateDoorReached(this);
        }
        
        Debug.Log("-----------------------------------------------");
    }

    public void UpdateInitialUnlock()
    {
        _initialUnlock = false;
    }

    public void UpdateRedLights()
    {
        Debug.Log("Initial room");
        Debug.Log("is red light: " + _isRedLight);
        Debug.Log("num active fires: " + GetNumActiveFires());
        
        // only update to red if there are fires in the room 
        // to improve performance, just check if the room was NOT red before
        if (!_isRedLight && GetNumActiveFires() > 0)
        {
            foreach (var roomLight in _roomLightChildrenList)
            {
                roomLight.color = _redRoomLightColor;
            }

            _isRedLight = true;
        }
    }
    
    public void UpdateNormalLights()
    {
        // only update to normal if there are no fires left in the room 
        // to improve performance, just check if the room was red before
        if (_isRedLight && GetNumActiveFires() == 0)
        {
            foreach (var roomLight in _roomLightChildrenList)
            {
                roomLight.color = _normalRoomLightColor;
            }

            _isRedLight = false;
        }
    }
}
