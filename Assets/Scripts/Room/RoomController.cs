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

    private RoomLocationsEnum a = RoomLocationsEnum.BlueRoom;

    private void Awake()
    {
        roomFireLocationsList = new List<RoomFireEntry>();
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
}
