using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamParticle : MonoBehaviour
{
    public delegate void SteamDelegateNone();
    public static SteamDelegateNone SteamFireNone;
    
    // FIXME: same logic as in Fire script...abstract?
    private float _steamStopDelay;
    
    private WaterParticle _waterParticle;

    private void Awake()
    {
        _steamStopDelay = 2.0f;
    }

    private void Start()
    {
        // FIXME: should be placed in the fire (parent) now
        _waterParticle = gameObject.GetComponentInChildren<WaterParticle>();
        
        InvokeRepeating("SteamStop", 0, 1);
    }
    
    // private void FixedUpdate()
    // {
    //     // FIXME: does not update if disabled...
    //     
    //     if (CheckSteamStopDelay())
    //     {
    //         SteamFireNone?.Invoke();
    //     }
    // }
    //
    private bool CheckSteamStopDelay()
    {
        // Debug.Log("current time: " + Time.time);
        
        var timeNotWatered = Time.time - _waterParticle.TimeLastWatered;
    
        return timeNotWatered >= _steamStopDelay;
    }

    private void SteamStop()
    {
        if (CheckSteamStopDelay())
        {
            SteamFireNone?.Invoke();
        }
    }
}
