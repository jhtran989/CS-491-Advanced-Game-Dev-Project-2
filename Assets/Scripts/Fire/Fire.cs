using System;
using System.Collections;
using System.Collections.Generic;
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
    
    [FormerlySerializedAs("fireRegenRate")] [SerializeField]
    private float fireRegenRatePerSecond = 0.1f;

    private bool isLit = true;

    // Start is called before the first frame update
    void Start()
    {
        _startIntensityArray = new float[firePSArray.Length];

        for (int i = 0; i < firePSArray.Length; i++)
        {
            _startIntensityArray[i] = firePSArray[i].emission.rateOverTime.constant;
        }
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
            ExtinguishFire();
            return true;
        }

        return false;
    }

    private void RegenFire()
    {
        var timeNotWatered = Time.time - timeLastWatered;
        
        // only regen fire if it is still lit (not already put out)
        if (isLit && currentIntensity < 1.0f && timeNotWatered >= fireRegenDelay)
        {
            currentIntensity += fireRegenRatePerSecond * Time.deltaTime;
            ChangeIntensity();
        }
    }

    private void ExtinguishFire()
    {
        isLit = false;
        enabled = false;
    }
}
