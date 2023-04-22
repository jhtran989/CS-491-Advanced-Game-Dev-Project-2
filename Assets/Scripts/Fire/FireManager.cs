using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireManager : MonoBehaviour
{
    public GameObject fireParent;
    private Transform _fireParentTransform;

    public FireSpawn fireSpawn;

    private int numInitialFires;
    private int numSpawnFires;

    public delegate void UpdateNumFiresDelegate();
    public static UpdateNumFiresDelegate updateNumFires;

    private void Awake()
    {
        _fireParentTransform = fireParent.transform;
        Debug.Log("Num fires: " + GetNumActiveFires());
    }

    private void OnEnable()
    {
        updateNumFires += RecalculateNumActiveFires;
    }

    private void OnDisable()
    {
        updateNumFires -= RecalculateNumActiveFires;
    }

    // Start is called before the first frame update
    void Start()
    {
        // FIXME: moved to fire spawn (random order of Start)
        // // need to initially calculate num of fires
        // RecalculateNumActiveFires();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetNumActiveFires()
    {
        return numInitialFires + numSpawnFires;
    }

    public void RecalculateNumActiveFires()
    {
        // return both initial fires and spawned fires
        // need to check if null
        
        // numInitialFires = 0;
        // numInitialFires = _fireParentTransform.GetChildCountActive();
        numInitialFires = fireSpawn.GetNumInitialFires();
        Debug.Log("num initial fires: " + numInitialFires);

        numSpawnFires = fireSpawn.GetNumSpawnFires();
        Debug.Log("num spawn fires: " + numSpawnFires);
        
        Debug.Log("num total fires: " + GetNumActiveFires());
    }

    
}
