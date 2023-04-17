using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpawn : MonoBehaviour
{
    public GameObject firePrefab;
    public GameObject fireParent;
    public GameObject currentRoom;
    
    private void Awake()
    {
        // test spawn fire
        Vector3 position = new Vector3(0, 0, 0);
        GameObject fire = Instantiate(firePrefab, position, Quaternion.identity, fireParent.transform);
        fire.transform.position = currentRoom.transform.position;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
