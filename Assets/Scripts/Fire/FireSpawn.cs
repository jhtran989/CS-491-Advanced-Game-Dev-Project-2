using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Room;
using Unity.VisualScripting;
using UnityEngine;
using static RoomLocations;

public class FireSpawn : MonoBehaviour
{
    public GameObject firePrefab;
    
    // TODO: change to use initial room in room manager (more modular)
    public GameObject currentRoom;
    private GameObject currentSpawnedFire;

    // FIXME: deprecated
    private RoomControllerOld _currentRoomControllerOld;

    private RoomController _currentRoomController;
    private RoomLocationsEnum _currentRoomLocationEnum;
    private GameObject _roomFireParent;
    private List<Vector3> _currentFireLocationPositions;

    // change to a list of fire locations (containing enum and a specific location)
    private List<RoomFireEntry> _roomFireLocationPositionsRandomList;

    public FireManager fireManager;
    public RoomManager roomManager;

    private float _startTime;
    private float _initialFireSpawnDelay;
    private float _fireSpawnDelay;
    
    public delegate void UpdateCurrentRoomDelegate(GameObject newRoom);
    public static UpdateCurrentRoomDelegate updateCurrentRoom;
    
    // [Space, Header("FireParentDictionary")]
    private Dictionary<RoomLocationsEnum, GameObject> roomLocationFireParentDictionary = 
        new Dictionary<RoomLocationsEnum, GameObject>();

    private void Awake()
    {
        // set fire dealays
        _initialFireSpawnDelay = 10.0f;
        _fireSpawnDelay = 15.0f;
        // _initialFireSpawnDelay = 60.0f;
        // _fireSpawnDelay = 120.0f;

        // initialize list
        _currentFireLocationPositions = new List<Vector3>();
        _roomFireLocationPositionsRandomList = new List<RoomFireEntry>();
        
        // update initial current room object
        currentRoom = roomManager.initialRoomController.gameObject;
    }
    
    private void OnEnable()
    {
        updateCurrentRoom += UpdateCurrentRoom;
    }

    private void OnDisable()
    {
        updateCurrentRoom -= UpdateCurrentRoom;
    } 

    // Start is called before the first frame update
    void Start() 
    {
        // FIXME: call update room instead with initial room
        // set initial room controller and current parameters
        UpdateCurrentRoom(roomManager.initialRoomController.gameObject);
        // UpdateFireSpawnParameters();

        // need to initially calculate num of fires
        // fireManager.RecalculateNumActiveFires();
        roomManager.RecalculateTotalNumActiveFires();
        
        // start the coroutine to spawn fires in current room every few seconds
        InvokeRepeating("TrySpawnFire", _initialFireSpawnDelay, _fireSpawnDelay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnFire()
    {
        // FIXME: fireParent can change depending on key selected in dictionary...also shuffle for dictionary
        // can't put it under current room to make math easier, need to keep track of all current fires...
        // Edit: split calculation into initial fires and spawned fires
        
        // FIXME: choose a random location in fire locations
        RoomFireEntry randomRoomFireEntry = _roomFireLocationPositionsRandomList[0];
        RoomLocationsEnum randomRoomLocationEnum = randomRoomFireEntry.RoomLocationsEnum;
        Vector3 randomFireLocationPosition = randomRoomFireEntry.FireLocationPosition;
        GameObject randomFireParent = randomRoomFireEntry.RoomFireParent;
        
        // reshuffle again (don't remove from list)
        _roomFireLocationPositionsRandomList = _roomFireLocationPositionsRandomList.Shuffle();
        
        // spawn in the fire parent specified by the current room controller
        // test spawn fire
        // update with random room fire parent (fireParent.transform)
        Vector3 position = new Vector3(0, 0, 0);
        GameObject fire = Instantiate(firePrefab, position, Quaternion.identity, randomFireParent.transform);

        // FIXME: already added height to each of the containers...
        // need to add a little height for the y component (smoke doesn't rise otherwise)

        fire.transform.localPosition = randomFireLocationPosition;

        Debug.Log("Spawn room: " + randomRoomLocationEnum);
        Debug.Log("Spawned fire location: " + fire.transform.localPosition);

        // put constant first for searching below...initial fires
        fire.name = Constants.SpawnedFireObjectName + currentRoom.name;
        currentSpawnedFire = fire;
        
        // TODO: fix with room manager
        // fireManager.RecalculateNumActiveFires();
        // Debug.Log("Num fires after spawn: " + fireManager.GetNumActiveFires());
        
        roomManager.RecalculateTotalNumActiveFires();
        
        // change the color of the lights to red
        RoomController randomRoomController = randomRoomFireEntry.RoomController;
        randomRoomController.UpdateRedLights();

        // AUDIO
        GameManager.instance.PlayFireAlert();
    }

    private void TrySpawnFire()
    {
        // TODO: fix with room manager
        // fireManager.RecalculateNumActiveFires();
        roomManager.RecalculateTotalNumActiveFires();
        
        // change to just number of spawned fires (at most two fires)
        // fireManager.GetNumActiveFires() == 0
        // FIXME: need to use room manager version...
        if (roomManager.GetTotalNumSpawnFies() == 0)
        {
            Debug.Log("SPAWNING FIRE...");
            SpawnFire();
        }
        else
        {
            Debug.Log("NO SPAWN FIRE - Fire already exists");
        }
    }

    public int GetNumInitialFires()
    {
        // FIXME: didn't use c in name check
        
        // var currentRoomTransform = currentRoom.transform;
        var fireParentTransform = _roomFireParent.transform;
        
        // if it is NOT a spawned fire
        return fireParentTransform.GetTransformCountPredicate(c => 
            !c.name.StartsWith(Constants.SpawnedFireObjectName) && c.gameObject.activeSelf);
    }

    public int GetNumSpawnFires()
    {
        // return currentRoom.transform.GetChildCountActive();
        // var currentRoomTransform = currentRoom.transform;
        var fireParentTransform = _roomFireParent.transform;
        
        // if it is a spawned fire
        return fireParentTransform.GetTransformCountPredicate(c => 
            c.name.StartsWith(Constants.SpawnedFireObjectName) && c.gameObject.activeSelf);
    }
    
    private void UpdateCurrentRoom(GameObject newRoom)
    {
        currentRoom = newRoom;
        roomManager.CurrentRoomObject = newRoom;

        // update fire spawn stuff (controller and fire locations)
        UpdateFireSpawnParameters();
    }

    private void UpdateFireSpawnParameters()
    {
        UpdateCurrentRoomControllerParameters();
        UpdateFireLocations(_currentRoomController);
    }

    private void UpdateCurrentRoomControllerParameters()
    {
        _currentRoomController = currentRoom.GetComponent<RoomController>();
        _currentRoomLocationEnum = _currentRoomController.roomLocationsEnum;
        _roomFireParent = _currentRoomController.roomFireParent;
        
        // for DEBUG purposes - see which fire locations are added
        _currentFireLocationPositions.AddRange(_currentRoomController.fireLocationPositions);
        
        // update list of unlocked rooms
        roomManager.AddRoom(_currentRoomController);
        
        // TODO: update door reached check (get list of doors in the room...)
        _currentRoomController.UpdateBoundaryDoorReachedList();
    }

    private void UpdateFireLocations(RoomController roomController)
    {
        _roomFireLocationPositionsRandomList
            .AddRange(roomController.GetRoomFireLocationsList());
        _roomFireLocationPositionsRandomList = _roomFireLocationPositionsRandomList.Shuffle();
    }
}
