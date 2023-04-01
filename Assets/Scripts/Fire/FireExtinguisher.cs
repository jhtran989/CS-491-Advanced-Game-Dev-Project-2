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
    private float extinguishedPerSecond = 0.1f;

    [SerializeField] private int fireLayerMask;

    [SerializeField] private Transform raycastOrigin = null;

    [Space, Header("Steam")] [SerializeField]
    private GameObject steamObject = null;

    [Space, Header("Water Hose")] [SerializeField]
    private GameObject waterHoseObject = null;

    // anonymous function to check Raycast
    private bool IsRaycastingSomething(out RaycastHit hit) => Physics.Raycast(raycastOrigin.position,
        raycastOrigin.forward, out hit, 100f, fireLayerMask);

    private bool IsRaycastingFire(out Fire fire)
    {
        fire = null;

        return IsRaycastingSomething(out RaycastHit hit) && hit.collider.TryGetComponent(out fire);
    }

    private void Awake()
    {
        fireLayerMask = LayerMask.GetMask(Constants.FireLayer);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (!steamObject)
        {
            Debug.LogError(
                "Please place a STEAM PARTICLE SYSTEM on the Extinguisher's steamObject field or rewrite the Extinguisher script.",
                this);
        }

        if (!waterHoseObject)
        {
            Debug.LogError(
                "Please place a WATER HOSE on the Extinguisher's steamObject field or rewrite the Extinguisher script.",
                this);
        }
    }

    private void FixedUpdate()
    {
        // TODO: abstract...
        // TODO: add layerMask instead of using collider.TryGetComponent()...
        // Transform playerCameraTransform = playerCamera.transform;
        // if (Physics.Raycast(playerCameraTransform.position, playerCameraTransform.forward, out RaycastHit raycastHit, 100.0f, fireLayerMask) && raycastHit.collider.TryGetComponent(out Fire fire))
        // {
        //     // FIXME: Debug.Log() equivalent to print()...
        //     // FIXME: actually, print() does not work if the class doesn't inherit from MonoBehavior...
        //     Debug.Log(raycastHit.collider.name);
        //     
        //     // need to use Time.fixedDeltaTime instead of Time.deltaTime for FixedUpdate
        //     fire.TryExtinguish(extinguishedPerSecond * Time.fixedTime);
        // }

        if (IsRaycastingFire(out Fire fire))
        {
            ExtinguishFire(fire);
        }
        else
        {
            if (steamObject.activeSelf)
            {
                steamObject.SetActive(false);
            }
            
            if (waterHoseObject.activeSelf)
            {
                waterHoseObject.SetActive(false);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void ExtinguishFire(Fire fire)
    {
        // need to use Time.fixedDeltaTime instead of Time.deltaTime for FixedUpdate
        fire.TryExtinguish(extinguishedPerSecond * Time.fixedDeltaTime);

        // activate the water hose
        waterHoseObject.SetActive(fire.CurrentIntensity > 0.0f);

        // spawn the steam at the same place as the fire
        steamObject.transform.position = fire.transform.position;
        steamObject.SetActive(fire.CurrentIntensity > 0.0f);
    }
}