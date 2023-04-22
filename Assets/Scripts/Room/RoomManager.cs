using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    private List<RoomController> _unlockedRoomsList;

    public List<RoomController> UnlockedRoomsList => _unlockedRoomsList;

    private void Awake()
    {
        _unlockedRoomsList = new List<RoomController>();
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
        if (!_unlockedRoomsList.Contains(roomController))
        {
            _unlockedRoomsList.Add(roomController);
        }
    }
}
