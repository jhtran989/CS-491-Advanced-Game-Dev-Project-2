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
    public float timeLastWatered = 0;

    private void Start()
    {
        _fireExtinguisher = fireExtinguisherObject.GetComponent<FireExtinguisher>();
        _collisionEvents = new List<ParticleCollisionEvent>();
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

        timeLastWatered = Time.time;
        
        WaterFireInteract?.Invoke(fire);
    }
}
