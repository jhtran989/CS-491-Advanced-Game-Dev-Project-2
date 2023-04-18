using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpawn : MonoBehaviour
{
    public GameObject firePrefab;
    public GameObject fireParent;
    public GameObject currentRoom;
    private GameObject currentSpawnedFire;

    public FireManager fireManager;

    private float _fireSpawnDelay;
    
    public delegate void UpdateCurrentRoomDelegate(GameObject newRoom);
    public static UpdateCurrentRoomDelegate updateCurrentRoom;
    
    private void Awake()
    {
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
        // test spawn fire
        Vector3 position = new Vector3(0, 0, 0);
        GameObject fire = Instantiate(firePrefab, position, Quaternion.identity, currentRoom.transform);
        // GameObject fire = Instantiate(firePrefab, position, Quaternion.identity, fireParent.transform);
        
        // can't put it under current room to make math easier, need to keep track of all current fires...
        // get relative position of current room to fire parent
        // Vector3 fireParentPosition = fireParent.transform.position;
        // Vector3 currentRoomPosition = currentRoom.transform.position;
        // Vector3 relativePositionFireParentToCurrentRoom = currentRoomPosition - fireParentPosition;
        
        fire.transform.localPosition = new Vector3(0, 0, -1);
        // Vector3 displacementVector = new Vector3(0, 0, -1);
        // fire.transform.localPosition = relativePositionFireParentToCurrentRoom + displacementVector;
        
        Debug.Log("Spawned fire location: " + fire.transform.position);
        // fire.transform.position = currentRoom.transform.position;

        fire.name = currentRoom.name + "SpawnedFire";
        currentSpawnedFire = fire;

        // start the coroutine to spawn fires in current room every few seconds

        fireManager.RecalculateNumActiveFires();
        Debug.Log("Num fires after spawn: " + fireManager.GetNumActiveFires());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void SpawnFire()
    {
        if (fireManager.GetNumActiveFires() == 0)
        {
            fireManager.RecalculateNumActiveFires();
        }
    }

    public int GetNumSpawnFires()
    {
        return currentRoom.transform.GetChildCountActive();
    }

    private void UpdateCurrentRoom(GameObject newRoom)
    {
        currentRoom = newRoom;
    }
}
