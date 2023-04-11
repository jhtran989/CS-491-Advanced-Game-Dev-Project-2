using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FireExtinguisher : MonoBehaviour
{
    public Camera playerCamera;

    // should be some value less than 1 (since intensity is a proportion of total intensity)
    [FormerlySerializedAs("amountExtinguishedPerSecond")] [SerializeField]
    private float extinguishedPerSecond = 0.5f;

    [SerializeField] private int fireLayerMask;

    [SerializeField] private Transform raycastOrigin = null;

    [Space, Header("Steam")] [SerializeField]
    private GameObject steamObject = null;

    private Fire _currentFire;

    [Space, Header("Water Hose")] [SerializeField]
    private GameObject waterHoseObject = null;

    [SerializeField]
    private float fireRaycastDistance = 5.0f;
    
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
        // need to use Time.fixedDeltaTime instead of Time.deltaTime for FixedUpdate
        // FIXME: particle system actually uses the normal frame instead of the fixed (physics) frame...
        fire.TryExtinguish(extinguishedPerSecond * Time.deltaTime);
        
        // link it to the fire object instead (so intensity also changes proportional to how much fire is put out)
        InitialSetSteam(fire);
        steamObject.SetActive(true);
    }

    private void InitialSetSteam(Fire fire)
    {
        // get steam object for future use for a given fire
        if (_currentFire == null || _currentFire != fire)
        {
            _currentFire = fire;
            steamObject = fire.gameObject.transform.Find(Constants.SteamGameObject).gameObject;
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

    public void DisableSteam()
    {
        if (steamObject.activeSelf)
        {
            steamObject.SetActive(false);
        }
    }
}