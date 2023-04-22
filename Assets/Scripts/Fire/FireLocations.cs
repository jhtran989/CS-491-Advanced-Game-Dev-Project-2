using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using static RoomLocations;

public class FireLocations : MonoBehaviour
{
    public RoomLocationsEnum roomLocationsEnum;
    
    // relative to corresponding FIRE CONTAINER local positions
    [FormerlySerializedAs("fireLocations")] 
    public Vector3[] fireLocationPositions;

    private RoomLocationsEnum a = RoomLocationsEnum.BlueRoom;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
