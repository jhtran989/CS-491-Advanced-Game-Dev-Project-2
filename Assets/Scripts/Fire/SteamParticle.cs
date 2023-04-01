using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteamParticle : MonoBehaviour
{
    public delegate void SteamDelegateNone();
    public static SteamDelegateNone SteamFireNone;
    
    // FIXME: same logic as in Fire script...abstract?
    private float steamStopDelay = 0;
    
    private WaterParticle _waterParticle;

    private void Start()
    {
        _waterParticle = gameObject.GetComponentInParent<WaterParticle>();
    }
    
    private void FixedUpdate()
    {
        // FIXME: does not update if disabled...
        
        if (CheckSteamStopDelay())
        {
            SteamFireNone?.Invoke();
        }
    }

    private bool CheckSteamStopDelay()
    {
        Debug.Log("current time: " + Time.time);
        
        var timeNotWatered = Time.time - _waterParticle.timeLastWatered;

        return timeNotWatered >= steamStopDelay;
    }
}
