using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// TODO: change Mathf.Lerp to Mathf.SmoothStep 

public class Oxygen : MonoBehaviour
{
    public TextMeshProUGUI oxygenText;
    public Image oxygenMeter;

    public float maxOxygenLevel = 100.0f;
    public float oxygenDrainPerSecond = 1.0f;
    public float lerpSpeed;
    public float lerpSpeedMultiplier = 100.0f;

    private float _startTime;
    [SerializeField]
    private float currentTime;

    private float _totalTime;
    
    [SerializeField]
    private float currentOxygenLevel;
    
    // Start is called before the first frame update
    void Start()
    {
        currentOxygenLevel = maxOxygenLevel;
        _startTime = Time.time;
        _totalTime = maxOxygenLevel / oxygenDrainPerSecond;
    }

    // Update is called once per frame
    void Update()
    {
        if (oxygenMeter.fillAmount > 0)
        {
            currentOxygenLevel -= oxygenDrainPerSecond * Time.deltaTime;

            lerpSpeed = lerpSpeedMultiplier * Time.deltaTime;
        
            OxygenMeterFiller();
            ColorChanger();
        }
    }

    private void OxygenMeterFiller()
    {
        // oxygenMeter.fillAmount = Mathf.Lerp(oxygenMeter.fillAmount, 
        //     currentOxygenLevel / maxOxygenLevel, 
        //     lerpSpeed);
        
        
        oxygenMeter.fillAmount = Mathf.SmoothStep(oxygenMeter.fillAmount, 
            currentOxygenLevel / maxOxygenLevel, 
            FindFractionTimeElapsed());
        
        // oxygenText.SetText(Math.Ceiling(oxygenMeter.fillAmount * maxOxygenLevel).ToString());
        
        oxygenText.SetText(Math.Ceiling(currentOxygenLevel).ToString());
    }

    private void ColorChanger()
    {
        Color oxygenColor = Color.Lerp(Color.red, Color.green, currentOxygenLevel / maxOxygenLevel);

        oxygenMeter.color = oxygenColor;
        oxygenText.color = oxygenColor;
    }

    private float FindFractionTimeElapsed()
    {
        return (Time.time - _startTime) / _totalTime;
    }
}
