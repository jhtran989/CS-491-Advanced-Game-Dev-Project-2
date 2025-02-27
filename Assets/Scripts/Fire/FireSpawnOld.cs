using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpawnOld : MonoBehaviour
{
    public GameObject firePrefab;
    public GameObject fireParent;
    public GameObject currentRoom;
    private GameObject currentSpawnedFire;

    public FireManager fireManager;

    private float _startTime;
    private float _initialFireSpawnDelay;
    private float _fireSpawnDelay;
    
    public delegate void UpdateCurrentRoomDelegate(GameObject newRoom);
    public static UpdateCurrentRoomDelegate updateCurrentRoom;
    
    private void Awake()
    {
        _initialFireSpawnDelay = 5.0f;
        _fireSpawnDelay = 15.0f;
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
        // start the coroutine to spawn fires in current room every few seconds
        InvokeRepeating("TrySpawnFire", _initialFireSpawnDelay, _fireSpawnDelay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnFire()
    {
        // test spawn fire
        Vector3 position = new Vector3(0, 0, 0);
        GameObject fire = Instantiate(firePrefab, position, Quaternion.identity, currentRoom.transform);
        // GameObject fire = Instantiate(firePrefab, position, Quaternion.identity, fireParent.transform);
        
        // can't put it under current room to make math easier, need to keep track of all current fires...
        // Edit: split calculation into initial fires and spawned fires
        
        // get relative position of current room to fire parent
        // Vector3 fireParentPosition = fireParent.transform.position;
        // Vector3 currentRoomPosition = currentRoom.transform.position;
        // Vector3 relativePositionFireParentToCurrentRoom = currentRoomPosition - fireParentPosition;
        
        fire.transform.localPosition = new Vector3(0, 0, -1);
        // Vector3 displacementVector = new Vector3(0, 0, -1);
        // fire.transform.localPosition = relativePositionFireParentToCurrentRoom + displacementVector;
        
        Debug.Log("Spawned fire location: " + fire.transform.localPosition);
        // fire.transform.position = currentRoom.transform.position;

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
        var currentRoomTransform = currentRoom.transform;
        
        // if it is NOT a spawned fire
        // return currentRoomTransform.GetTransformCountCondition(
        //     !currentRoomTransform.name.StartsWith(Constants.SpawnedFireObjectName));

        return currentRoomTransform.GetTransformCountPredicate(c => 
            !currentRoomTransform.name.StartsWith(Constants.SpawnedFireObjectName) && c.gameObject.activeSelf);
    }

    public int GetNumSpawnFires()
    {
        // return currentRoom.transform.GetChildCountActive();
        
        var currentRoomTransform = currentRoom.transform;
        
        // if it is a spawned fire
        // return currentRoomTransform.GetTransformCountCondition(
        //     currentRoomTransform.name.StartsWith(Constants.SpawnedFireObjectName));
        
        return currentRoomTransform.GetTransformCountPredicate(c => 
            currentRoomTransform.name.StartsWith(Constants.SpawnedFireObjectName) && c.gameObject.activeSelf);
    }
    
    private void UpdateCurrentRoom(GameObject newRoom)
    {
        currentRoom = newRoom;
    }
}
