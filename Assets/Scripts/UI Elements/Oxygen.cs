using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

// TODO: change Mathf.Lerp to Mathf.SmoothStep 

public class Oxygen : MonoBehaviour
{
    public TextMeshProUGUI oxygenText;
    public Image oxygenMeter;
    public TextMeshProUGUI oxygenIndicatorText;

    public GameObject playerObject;
    private Rigidbody _playerRigidBody;
    private PlayerMovement _playerMovement;

    public CameraShake cameraShake;
    private float _oxygenDrainTimeDelay;

    [Space, Header("Managers")]
    public FireManager fireManager;
    public RoomManager roomManager;

    [Space, Header("Oxygen Stuff")]
    [SerializeField]
    private float speedStationaryThreshold = 0.5f;

    private float maxOxygenLevel;
    private float oxygenDrainPerSecond;
    private float lerpSpeed;
    private float lerpSpeedMultiplier;
    public Color[] colors;

    private float _startTime;
    private float _currentTime;
    private float _totalTime;
    
    [SerializeField]
    private float currentOxygenLevel;

    private float _currentOxygenLevelPercent;
    
    // oxygen rate stuff
    public float oxygenRateNormal;

    [SerializeField]
    private float oxygenRateIdleMultiplier;
    public float oxygenRateIdle;

    [SerializeField]
    private float oxygenRateFireMultiplier;
    
    [SerializeField]
    private float oxygenRateTooCloseFireMultiplier;
    
    public float currentOxygenRate;
    
    // FIXME: change logic for if a fire is present, regardless of distance
    // TODO: only make ONE fire present at any time
    // flag if player is near fire, or idle
    private bool _nearFire;
    
    [FormerlySerializedAs("_firePresent")] 
    public bool firePresent;
    
    private bool _isIdle;
    
    /****************/
    
    // Colors
    private Color _initialColor;
    private Color _finalColor;
    
    /****************/


    // FIXME: from Start()
    void Awake()
    {
        // FIXME FINAL: modify max oxygen
        maxOxygenLevel = 600.0f;
        oxygenDrainPerSecond = 1.0f;
        lerpSpeedMultiplier = 100.0f;
        
        // update current oxygen levels
        currentOxygenLevel = maxOxygenLevel;
        UpdateCurrentOxygenLevelPercent();
        
        _startTime = Time.time;
        _totalTime = maxOxygenLevel / oxygenDrainPerSecond;

        oxygenRateIdleMultiplier = 0.1f;
        oxygenRateFireMultiplier = 2.0f;
        oxygenRateTooCloseFireMultiplier = 4.0f;

        oxygenRateNormal = oxygenDrainPerSecond;
        // assume fire already spawned...
        currentOxygenRate = oxygenRateFireMultiplier;
        
        _nearFire = false;
        firePresent = true;
        _isIdle = true;

        _startTime = Time.time;
        
        // set colors
        // blue
        if (!ColorUtility.TryParseHtmlString("#0046FF", out _initialColor))
        {
            Debug.LogError("Invalid color for INITIAL...");
        }
        
        // red
        if (!ColorUtility.TryParseHtmlString("#C61F5F", out _finalColor))
        {
            Debug.LogError("Invalid color for FINAL...");
        }
    }

    private void OnEnable()
    {
        // player movement
        _playerMovement = playerObject.GetComponent<PlayerMovement>();
        _playerMovement.UpdateOxygenRateFire += UpdateOxygenRateFire;
        _playerMovement.UpdateOxygenRateExit += UpdateOxygenRateExit;
        _playerMovement.UpdateOxygenRateIdle += UpdateOxygenRateIdle;
        
        // fire
        Fire.FirePutOut += UpdateOxygenRateNormal;
    }

    private void OnDisable()
    {
        // player movement
        _playerMovement.UpdateOxygenRateFire -= UpdateOxygenRateFire;
        _playerMovement.UpdateOxygenRateExit -= UpdateOxygenRateExit;
        _playerMovement.UpdateOxygenRateIdle -= UpdateOxygenRateIdle;
        
        // fire
        Fire.FirePutOut -= UpdateOxygenRateNormal;
    }

    private void Start()
    {
        // need to get components from other objects in Start
        _playerRigidBody = playerObject.GetComponent<Rigidbody>();

        _oxygenDrainTimeDelay = cameraShake.duration;
    }

    // Update is called once per frame
    void Update()
    {
        _currentTime = Time.time;

        // TODO: don't drain until AFTER the shaking (now with initial fire drain)
        if (_currentTime - _startTime > _oxygenDrainTimeDelay)
        {
            if (oxygenMeter.fillAmount > 0)
            {
                currentOxygenLevel -= currentOxygenRate * Time.deltaTime;

                lerpSpeed = lerpSpeedMultiplier * Time.deltaTime;
                
                // need to update current oxygen level
                UpdateCurrentOxygenLevelPercent();
        
                OxygenMeterFiller();
                ColorChanger();
            }

            UpdateOxygenIndicator();
        }

        if (currentOxygenLevel <= 0) {
            GameManager gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            gameManager.SetOxygenDepletion();
            gameManager.EndGame();
        }

        // FIXME: change location to be near the player...
        // when low oxygen
        // FIXME FINAL: forgot to change the beeping for LOW oxygen
        // 99.9
        if (_currentOxygenLevelPercent < 30.0f)
        {
            SoundManager.instance.PlaySoundEffect(SoundTypesEnum.LowOxygen);
        }

    }

    private void UpdateCurrentOxygenLevelPercent()
    {
        _currentOxygenLevelPercent = (currentOxygenLevel / maxOxygenLevel) * 100.0f;
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
        
        // FIXME: need to use percent and not level (currentOxygenLevel)
        oxygenText.SetText(Math.Ceiling(_currentOxygenLevelPercent).ToString()+"%");
    }

    private void ColorChanger()
    {
        // TODO: switch to custom Color32 (uses 0-255 values) or HEX (html) above in Awake()
        // Color oxygenColor = Color.Lerp(Color.red, Color.blue, 
        //     currentOxygenLevel / maxOxygenLevel);
        Color oxygenColor = Color.Lerp(_finalColor, _initialColor, 
            currentOxygenLevel / maxOxygenLevel);

        oxygenMeter.color = oxygenColor;
        //oxygenText.color = oxygenColor;
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
        currentOxygenRate = oxygenRateNormal * oxygenRateTooCloseFireMultiplier;
        _nearFire = true;
    }
    
    private void UpdateOxygenRateNormal()
    {
        currentOxygenRate = oxygenRateNormal;
        firePresent = false;
        _nearFire = false;
    }
    
    private void UpdateOxygenRateExit()
    {
        // when exiting collider of fire
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
        
        // TODO: get from room manager instead (fireManager.GetNumActiveFires() > 0)
        // set fire present depending on how many fires are present (greater than 0)
        if (roomManager.GetTotalNumFies() > 0)
        {
            firePresent = true;
        }
        else
        {
            firePresent = false;
        }
        
        // the fire will overwrite the oxygen rate, even if idle
        if (_nearFire)
        {
            currentOxygenRate = oxygenRateNormal * oxygenRateTooCloseFireMultiplier;
        }
        else if (firePresent)
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

        // changed from _nearFire\
        if (_nearFire)
        {
            oxygenIndicatorText += "Too close to fire!";
        }
        else if (firePresent)
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
