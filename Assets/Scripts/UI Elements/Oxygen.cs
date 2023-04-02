using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// TODO: change Mathf.Lerp to Mathf.SmoothStep 

public class Oxygen : MonoBehaviour
{
    public TextMeshProUGUI oxygenText;
    public Image oxygenMeter;
    public TextMeshProUGUI oxygenIndicatorText;

    public GameObject PlayerObject;
    private Rigidbody _playerRigidBody;
    private PlayerMovement _playerMovement;

    [SerializeField]
    private float speedStationaryThreshold = 0.5f;

    public float maxOxygenLevel = 100.0f;
    public float oxygenDrainPerSecond = 1.0f;
    public float lerpSpeed;
    public float lerpSpeedMultiplier = 100.0f;
    public Color[] colors;

    private float _startTime;
    [SerializeField]
    private float currentTime;

    private float _totalTime;
    
    [SerializeField]
    private float currentOxygenLevel;
    
    // oxygen rate stuff
    public float oxygenRateNormal;

    [SerializeField]
    private float oxygenRateIdleMultiplier = 0.1f;
    public float oxygenRateIdle;

    [SerializeField]
    private float oxygenRateFireMultiplier = 4.0f;
    
    public float currentOxygenRate;
    
    // flag if player is near fire, or idle
    private bool _nearFire = false;
    private bool _isIdle = true;


    // FIXME: from Start()
    void Awake()
    {
        currentOxygenLevel = maxOxygenLevel;
        _startTime = Time.time;
        _totalTime = maxOxygenLevel / oxygenDrainPerSecond;
        
        oxygenRateNormal = oxygenDrainPerSecond;
        currentOxygenRate = oxygenRateNormal;

        _playerRigidBody = PlayerObject.GetComponent<Rigidbody>();
        _playerMovement = PlayerObject.GetComponent<PlayerMovement>();
    }

    private void OnEnable()
    {
        // player movement
        _playerMovement.UpdateOxygenRateFire += UpdateOxygenRateFire;
        _playerMovement.UpdateOxygenRateNormal += UpdateOxygenRateNormal;
        _playerMovement.UpdateOxygenRateIdle += UpdateOxygenRateIdle;
        
        // fire
        Fire.FirePutOut += UpdateOxygenRateNormal;
    }

    private void OnDisable()
    {
        // player movement
        _playerMovement.UpdateOxygenRateFire -= UpdateOxygenRateFire;
        _playerMovement.UpdateOxygenRateNormal -= UpdateOxygenRateNormal;
        _playerMovement.UpdateOxygenRateIdle -= UpdateOxygenRateIdle;
        
        // fire
        Fire.FirePutOut -= UpdateOxygenRateNormal;
    }

    // Update is called once per frame
    void Update()
    {
        if (oxygenMeter.fillAmount > 0)
        {
            currentOxygenLevel -= currentOxygenRate * Time.deltaTime;

            lerpSpeed = lerpSpeedMultiplier * Time.deltaTime;
        
            OxygenMeterFiller();
            ColorChanger();
        }

        UpdateOxygenIndicator();
    }

    private void OxygenMeterFiller()
    {
        // oxygenMeter.fillAmount = Mathf.Lerp(oxygenMeter.fillAmount, 
        //     currentOxygenLevel / maxOxygenLevel, 
        //     lerpSpeed);
        
        // TODO: only need to call SmoothStep if oxygen rate changes?
        oxygenMeter.fillAmount = Mathf.SmoothStep(oxygenMeter.fillAmount, 
            currentOxygenLevel / maxOxygenLevel, 
            FindFractionTimeElapsed());
        
        // oxygenText.SetText(Math.Ceiling(oxygenMeter.fillAmount * maxOxygenLevel).ToString());
        
        oxygenText.SetText(Math.Ceiling(currentOxygenLevel).ToString()+"%");
    }

    private void ColorChanger()
    {
        Color oxygenColor = Color.Lerp(Color.red, Color.blue, currentOxygenLevel / maxOxygenLevel);

        oxygenMeter.color = oxygenColor;
        oxygenText.color = oxygenColor;
    }

    private float FindFractionTimeElapsed()
    {
        // TODO: need to find dynamic fraction (different rates...)
        // return (Time.time - _startTime) / _totalTime;
        
        return 1.0f - (currentOxygenLevel - maxOxygenLevel);
    }

    private void UpdateOxygenIndicator()
    {
        // updates to fire/normal are done on delegates (trigger on player)...
        UpdateOxygenRateIdle();
        oxygenIndicatorText.SetText(CreateOxygenIndicatorText());
    }

    private void UpdateOxygenRateFire()
    {
        currentOxygenRate = oxygenRateNormal * oxygenRateFireMultiplier;
        _nearFire = true;
    }
    
    private void UpdateOxygenRateNormal()
    {
        currentOxygenRate = oxygenRateNormal;
        _nearFire = false;
    }

    private void UpdateOxygenRateIdle()
    {
        float playerSpeed = _playerRigidBody.velocity.magnitude;

        if (playerSpeed > speedStationaryThreshold)
        {
            currentOxygenRate = oxygenRateNormal;
            _isIdle = false;
        }
        else
        {
            currentOxygenRate = oxygenRateNormal * oxygenRateIdleMultiplier;
            _isIdle = true;
        }
        
        // the fire will overwrite the oxygen rate, even if idle
        if (_nearFire)
        {
            currentOxygenRate = oxygenRateNormal * oxygenRateFireMultiplier;
        }
    }

    private string CreateOxygenIndicatorText()
    {
        var oxygenIndicatorText = "";

        if (_isIdle)
        {
            oxygenIndicatorText += "Idle\n";
        }
        else
        {
            oxygenIndicatorText += "Moving\n";
        }

        if (_nearFire)
        {
            oxygenIndicatorText += "Fire!";
        }
        else
        {
            oxygenIndicatorText += "Safe from fire...";
        }

        return oxygenIndicatorText;
    }
}
