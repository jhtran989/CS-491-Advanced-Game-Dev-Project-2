using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Room;
using UnityEngine;
using static RoomLocations;

public class FireSpawn : MonoBehaviour
{
    public GameObject firePrefab;
    public GameObject fireParent;
    public GameObject currentRoom;
    public GameObject roomParent;
    private GameObject currentSpawnedFire;

    // FIXME: deprecated
    private RoomControllerOld _currentRoomControllerOld;

    private RoomController _currentRoomController;
    private RoomLocationsEnum _currentRoomLocationEnum;
    private Vector3[] _currentFireLocationPositions;

    // change to a list of fire locations (containing enum and a specific location)
    private List<RoomFireEntry> _roomFireLocationPositionsRandomList;

    public FireManager fireManager;

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
        _initialFireSpawnDelay = 5.0f;
        _fireSpawnDelay = 15.0f;
        
        // set current room enum
        _currentRoomController = currentRoom.GetComponent<RoomController>();
        _currentRoomLocationEnum = _currentRoomController.roomLocationsEnum;
        _currentFireLocationPositions = _currentRoomController.fireLocationPositions;

        // initialize list
        _roomFireLocationPositionsRandomList = new List<RoomFireEntry>();
        UpdateFireLocations(_currentRoomController);
        
        // _roomFireLocationPositionsRandomList
        //     .AddRange(_currentRoomController.GetRoomFireLocationsList());
        // _roomFireLocationPositionsRandomList = _roomFireLocationPositionsRandomList.Shuffle();
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
        // set initial room controller
        _currentRoomControllerOld = currentRoom.GetComponent<RoomControllerOld>();
        fireParent = _currentRoomControllerOld.fireParent;
        
        // need to initially calculate num of fires
        fireManager.RecalculateNumActiveFires();
        
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

        Debug.Log("Spawned fire location: " + fire.transform.localPosition);

        // put constant first for searching below...initial fires
        fire.name = Constants.SpawnedFireObjectName + currentRoom.name;
        currentSpawnedFire = fire;
        
        fireManager.RecalculateNumActiveFires();
        Debug.Log("Num fires after spawn: " + fireManager.GetNumActiveFires());
    }

    private void TrySpawnFire()
    {
        fireManager.RecalculateNumActiveFires();
        
        if (fireManager.GetNumActiveFires() == 0)
        {
            Debug.Log("SPAWNING FIRE...");
            SpawnFire();
        }
        else
        {
            Debug.Log("NO SPAWN FIRE - Fire already exists");
        }
    }

    private void TrySpawnFireWrapper()
    {
        // wait for initial delay
        // yield return new WaitForSeconds(_initialFireSpawnDelay);
    }
    
    public int GetNumInitialFires()
    {
        // var currentRoomTransform = currentRoom.transform;
        var fireParentTransform = fireParent.transform;
        
        // if it is NOT a spawned fire
        return fireParentTransform.GetTransformCountPredicate(c => 
            !fireParentTransform.name.StartsWith(Constants.SpawnedFireObjectName) && c.gameObject.activeSelf);
    }

    public int GetNumSpawnFires()
    {
        // return currentRoom.transform.GetChildCountActive();
        // var currentRoomTransform = currentRoom.transform;
        var fireParentTransform = fireParent.transform;
        
        // if it is a spawned fire
        return fireParentTransform.GetTransformCountPredicate(c => 
            fireParentTransform.name.StartsWith(Constants.SpawnedFireObjectName) && c.gameObject.activeSelf);
    }
    
    private void UpdateCurrentRoom(GameObject newRoom)
    {
        currentRoom = newRoom;
        
        // update current room controller
        _currentRoomControllerOld = currentRoom.GetComponent<RoomControllerOld>();
        
        
        fireParent = _currentRoomControllerOld.fireParent;
        
        // update list of fire positions and list of unlocked rooms
    }

    private void UpdateFireLocations(RoomController roomController)
    {
        _roomFireLocationPositionsRandomList
            .AddRange(roomController.GetRoomFireLocationsList());
        _roomFireLocationPositionsRandomList = _roomFireLocationPositionsRandomList.Shuffle();
    }
}
