using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FireExtinguisher : MonoBehaviour
{
    public Camera playerCamera;

    // should be some value less than 1 (since intensity is a proportion of total intensity)
    [SerializeField]
    private float extinguishedPerSecond;

    [SerializeField] private int fireLayerMask;

    [SerializeField] private Transform raycastOrigin = null;

    [Space, Header("Steam")] [SerializeField]
    private GameObject steamObject = null;

    private Fire _currentFire;

    [Space, Header("Water Hose")] [SerializeField]
    private GameObject waterHoseObject = null;

    [SerializeField]
    private float fireRaycastDistance = 5.0f;

    public bool fireExtinguished;
    
    /****************************************************/

    [FormerlySerializedAs("interactableComponents")] public FireExtinguisherInteractableComponents fireExtinguisherInteractableComponents;
    
    // anonymous function to check Raycast
    private bool IsRaycastingSomething(out RaycastHit hit) => Physics.Raycast(raycastOrigin.position,
        raycastOrigin.forward, out hit, fireRaycastDistance, fireLayerMask);

    public bool steamFireCheck = false;

    private bool IsRaycastingFire(out Fire fire)
    {
        fire = null;

        return IsRaycastingSomething(out RaycastHit hit) && hit.collider.TryGetComponent(out fire);
    }

    private void Awake()
    {
        fireLayerMask = LayerMask.GetMask(Constants.FireLayer);

        extinguishedPerSecond = 0.8f;

        fireExtinguished = false;
    }

    private void OnEnable()
    {
        SteamParticle.SteamFireNone += DisableSteam;
        WaterParticle.WaterFireInteract += ExtinguishFire;
    }

    private void OnDisable()
    {
        SteamParticle.SteamFireNone -= DisableSteam;
        WaterParticle.WaterFireInteract -= ExtinguishFire;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!waterHoseObject)
        {
            Debug.LogError(
                "Please place a WATER HOSE on the Extinguisher's steamObject field or rewrite the Extinguisher script.",
                this);
        }
    }

    private void FixedUpdate()
    {
        // mouse hold instead of Raycast...
        SetWaterOnInput();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void ExtinguishFire(Fire fire)
    {
        // FIXME: stop after extinguished
        // FIXME: need to reset fireExtinguished...
        
        // get the updated fire and steam
        InitialSetSteam(fire);
        
        // Debug.Log("Current Fire: " + _currentFire);
        // Debug.Log("Input fire: " + fire);
        
        if (!fireExtinguished)
        {
            // need to use Time.fixedDeltaTime instead of Time.deltaTime for physics
            fireExtinguished = fire.TryExtinguish(extinguishedPerSecond * Time.fixedDeltaTime);
        
            // link it to the fire object instead (so intensity also changes proportional to how much fire is put out)
            steamObject.SetActive(true);
        }
    }

    private void InitialSetSteam(Fire fire)
    {
        // get steam object for future use for a given fire
        if (_currentFire == null || _currentFire != fire)
        {
            _currentFire = fire;
            steamObject = fire.gameObject.transform.Find(Constants.SteamGameObjectName).gameObject;
            
            // reset if fire extinguished
            fireExtinguished = false;
        }
    }

    private void EnableWaterFireCondition(Fire fire)
    {
        // activate the water hose
        waterHoseObject.SetActive(fire.CurrentIntensity > 0.0f);
    }
    
    private void EnableWater()
    {
        // activate the water hose
        waterHoseObject.SetActive(true);
    }

    private void DisableWater()
    {
        if (waterHoseObject.activeSelf)
        {
            waterHoseObject.SetActive(false);
        }
    }

    private void SetWaterOnInput()
    {
        // TODO: only activate when holding fire extinguisher
        if (fireExtinguisherInteractableComponents.isHoldingFireExtinguisher)
        {
            // for Right mouse button (pressed down)
            if (Input.GetMouseButton(1))
            {
                EnableWater();
            }
            else
            {
                DisableWater();
            }
        }
    }

    public void DisableSteam()
    {
        if (steamObject.activeSelf)
        {
            steamObject.SetActive(false);
        }
    }
}