using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUDManager : MonoBehaviour
{
    public GameObject DoorPowerBarObject;
    public GameObject OxygenObject;
    
    private void Awake()
    {
        var tempTransform = gameObject.transform;
        DoorPowerBarObject = tempTransform.Find(Constants.DoorPowerBarObjectName).gameObject;
        OxygenObject = tempTransform.Find(Constants.OxygenObjectName).gameObject;
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
