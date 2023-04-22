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
    
    // relative to corresponding FIRE CONTAINER local positions
    [FormerlySerializedAs("fireLocations")] 
    public Vector3[] fireLocationPositions;

    private List<RoomFireEntry> roomFireLocationsList;

    private int _numInitialFires;
    private int _numSpawnFires;

    private void Awake()
    {
        roomFireLocationsList = new List<RoomFireEntry>();
        // Debug.Log("Num fires: " + GetNumActiveFires());
    }

    public List<RoomFireEntry> GetRoomFireLocationsList()
    {
        if (!roomFireLocationsList.Any())
        {
            foreach (var fireLocationPosition in fireLocationPositions)
            {
                roomFireLocationsList.Add(
                    new RoomFireEntry(roomLocationsEnum, roomFireParent, fireLocationPosition));
            }
        }

        return roomFireLocationsList;
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
}
