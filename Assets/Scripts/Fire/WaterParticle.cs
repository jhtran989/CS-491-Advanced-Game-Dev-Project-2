using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterParticle : MonoBehaviour
{
    public delegate void WaterDelegateInteract(Fire fire);
    public static WaterDelegateInteract WaterFireInteract;

    [SerializeField]
    private ParticleSystem steamParticleSystem;

    private List<ParticleCollisionEvent> _collisionEvents;

    [SerializeField]
    private GameObject fireExtinguisherObject;

    private FireExtinguisher _fireExtinguisher;
    
    // FIXME: same logic as in Fire script...abstract?
    private float _timeLastWatered;
    private float _steamStopDelay;

    public float TimeLastWatered
    {
        get => _timeLastWatered;
        set => _timeLastWatered = value;
    }

    private void Awake()
    {
        _timeLastWatered = 0;
        _steamStopDelay = 1.0f;
        fireExtinguisherObject = gameObject.transform.parent.gameObject;
    }

    private void Start()
    {
        _fireExtinguisher = fireExtinguisherObject.GetComponent<FireExtinguisher>();
        _collisionEvents = new List<ParticleCollisionEvent>();
        
        // periodically check if steam should be turned off
        InvokeRepeating("SteamStop", 0f, 1.0f); 
    }

    // private void OnParticleTrigger()
    // {
    //     // particles
    //     List<ParticleSystem.Particle> enter = new List<ParticleSystem.Particle>();
    //
    //     // get
    //     int numEnter = steamParticleSystem.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, enter);
    //
    //     if (numEnter == 0)
    //     {
    //         _fireExtinguisher.steamFireCheck = false;
    //         SteamFireNone?.Invoke();
    //     }
    //     else
    //     {
    //         _fireExtinguisher.steamFireCheck = true;
    //         SteamFireInteract?.Invoke();
    //     }
    // }

    private void OnParticleCollision(GameObject other)
    {
        // int numCollisionEvents = steamParticleSystem.GetCollisionEvents(other, _collisionEvents);
        // Debug.Log("num collisions: " + numCollisionEvents);
        
        // layer already specified in Inspector
        var fire = other.GetComponent<Fire>();

        Debug.Log("Water collided with fire...");
        Debug.Log("Current fire: " + fire);

        WaterFireInteract?.Invoke(fire);
        
        _timeLastWatered = Time.time;
    }
    
    private bool CheckSteamStopDelay()
    {
        // Debug.Log("current time: " + Time.time);
        
        var timeNotWatered = Time.time - _timeLastWatered;
    
        return timeNotWatered >= _steamStopDelay;
    }
    
    private void SteamStop()
    {
        if (CheckSteamStopDelay())
        {
            _fireExtinguisher.DisableSteam();
        }
    }
}
