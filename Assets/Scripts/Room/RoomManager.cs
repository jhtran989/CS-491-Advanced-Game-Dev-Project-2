using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RoomLocations;

public class RoomManager : MonoBehaviour
{
    private GameObject _currentRoomObject;
    public RoomController initialRoomController;

    public GameObject CurrentRoomObject
    {
        get => _currentRoomObject;
        set => _currentRoomObject = value;
    }

    private List<RoomController> _unlockedRoomControllersList;

    public List<RoomController> UnlockedRoomControllersList => _unlockedRoomControllersList;
    
    private int _totalNumInitialFires;
    private int _totalNumSpawnFires;
    private int _totalNumFires;
    
    public delegate void UpdateNumFiresDelegate();
    public static UpdateNumFiresDelegate updateNumFires;

    private void Awake()
    {
        _unlockedRoomControllersList = new List<RoomController>();
    }
    
    private void OnEnable()
    {
        updateNumFires += RecalculateTotalNumActiveFires;
    }

    private void OnDisable()
    {
        updateNumFires -= RecalculateTotalNumActiveFires;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddRoom(RoomController roomController)
    {
        if (!_unlockedRoomControllersList.Contains(roomController))
        {
            _unlockedRoomControllersList.Add(roomController);
        }
    }

    public int GetTotalNumFies()
    {
        return _totalNumFires;
    }
    
    public int GetTotalNumInitialFies()
    {
        return _totalNumInitialFires;
    }
    
    public int GetTotalNumSpawnFies()
    {
        return _totalNumSpawnFires;
    }
    
    public void RecalculateTotalNumActiveFires()
    {
        var totalNumInitialFires = 0;
        var totalNumSpawnFires = 0;
        var totalNumFires = 0;
        
        foreach (var unlockedRoomController in _unlockedRoomControllersList)
        {
            unlockedRoomController.RecalculateNumActiveFires();

            totalNumInitialFires += unlockedRoomController.GetNumInitialFires();
            totalNumSpawnFires += unlockedRoomController.GetNumSpawnFires();
            totalNumFires += unlockedRoomController.GetNumActiveFires();
        }

        _totalNumInitialFires = totalNumInitialFires;
        _totalNumSpawnFires = totalNumSpawnFires;
        _totalNumFires = totalNumFires;
        
        PrintAllFireStats();
    }

    private void PrintAllFireStats()
    {
        Debug.Log("-----------------------------------------------");
        
        foreach (var unlockedRoomController in _unlockedRoomControllersList)
        {
            RoomLocationsEnum roomLocationsEnum = unlockedRoomController.roomLocationsEnum;
            Debug.Log("Room: " + roomLocationsEnum);
            Debug.Log("Initial fires: " + unlockedRoomController.GetNumInitialFires());
            Debug.Log("Spawn fires: " + unlockedRoomController.GetNumSpawnFires());
            Debug.Log("Total room fires: " + unlockedRoomController.GetNumActiveFires());
            Debug.Log("-----------------------------------------------");
        }
        
        Debug.Log("Total stats");
        Debug.Log("Initial fires: " + _totalNumInitialFires);
        Debug.Log("Spawn fires: " + _totalNumSpawnFires);
        Debug.Log("Total fires: " + _totalNumFires);
        Debug.Log("-----------------------------------------------");
    }
}
