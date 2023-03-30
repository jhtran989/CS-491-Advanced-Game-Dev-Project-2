using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)] 
    private float currentIntensity = 1.0f;

    private float startIntensity = 5.0f;

    [SerializeField] 
    private ParticleSystem firePS = null;

    // Start is called before the first frame update
    void Start()
    {
        startIntensity = firePS.emission.rateOverTime.constant;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeIntensity(); 
    }
    
    private void ChangeIntensity()
    {
        var emmission = firePS.emission;
        emmission.rateOverTime = currentIntensity * startIntensity;
    }
}
