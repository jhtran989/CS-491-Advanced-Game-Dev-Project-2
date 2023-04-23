using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Serialization;

public class Fire : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] 
    private float currentIntensity = 1.0f;

    public float CurrentIntensity
    {
        get => currentIntensity;
        set => currentIntensity = value;
    }

    private float[] _startIntensityArray = Array.Empty<float>();

    [FormerlySerializedAs("firePS")] [SerializeField] 
    private ParticleSystem[] firePSArray = Array.Empty<ParticleSystem>();

    private float timeLastWatered = 0;

    [SerializeField]
    private float fireRegenDelay = 2.5f;

    [SerializeField] 
    private float fireRegenRatePerSecond;

    private bool _isLit = true;
    
    // need bool for unlocking door
    public bool unlockDoor = false;
    
    // need delegate to stop the increased oxygen drain when fire is put out (when still within range when put out)
    public delegate void FirePutOutDelegate();
    public static FirePutOutDelegate FirePutOut;
    
    /***********************************/

    // FIXME: hard to put stuff in Inspector from a prefab...
    // public FireManager fireManager;

    private void Awake()
    {
        fireRegenRatePerSecond = 0.5f;
        _startIntensityArray = new float[firePSArray.Length];

        for (int i = 0; i < firePSArray.Length; i++)
        {
            _startIntensityArray[i] = firePSArray[i].emission.rateOverTime.constant;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RegenFire();
    }
    
    private void ChangeIntensity()
    {
        // FIXME: normal for loop faster than for each loop...
        for (int i = 0; i < firePSArray.Length; i++)
        {
            var emmission = firePSArray[i].emission;
            emmission.rateOverTime = currentIntensity * _startIntensityArray[i];
        }
    }

    public bool TryExtinguish(float extinguishAmount)
    {
        timeLastWatered = Time.time;
        currentIntensity -= extinguishAmount;
        
        ChangeIntensity();
        
        // // update if the fire is put out
        // isLit = currentIntensity > 0;
        //
        // // if the fire was put out (not lit anymore)
        // return !isLit;

        // encapsulated logic and disable fire
        if (currentIntensity <= 0)
        {
            PutOutFire();
            return true;
        }

        return false;
    }

    private void RegenFire()
    {
        var timeNotWatered = Time.time - timeLastWatered;
        
        // only regen fire if it is still lit (not already put out)
        if (_isLit && currentIntensity < 1.0f && timeNotWatered >= fireRegenDelay)
        {
            currentIntensity += fireRegenRatePerSecond * Time.deltaTime;
            ChangeIntensity();
        }
    }

    private void PutOutFire()
    {
        _isLit = false;
        
        // unlock the door (so it can be opened, doesn't automatically open the door)
        unlockDoor = true;
        DoorController.unlockDoor?.Invoke();
        
        // run events when fire is put out
        FirePutOut?.Invoke();
        
        // FIXME: need to disable entire game object
        // enabled = false;
        gameObject.SetActive(false);
        
        // TODO: fix with room manager
        // update num fires
        // FireManager.updateNumFires?.Invoke();
        RoomManager.updateNumFires?.Invoke();
        
        // change the lights back to normal if there are no fires left
        RoomController.updateNormalLight?.Invoke();
    }
}
