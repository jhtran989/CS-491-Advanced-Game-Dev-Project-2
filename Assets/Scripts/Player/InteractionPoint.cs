using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionPoint : MonoBehaviour
{
    // FIXME: editing interaction point is too much work...
    // FIXME: SCRATCH THIS
    // TODO: get sense of separate rooms and get list of objects in room to enable/disable interactions...

    private void OnEnable()
    {
        Fire.FirePutOut += EnableInteractionPoint;
    }

    private void OnDisable()
    {
        Fire.FirePutOut -= EnableInteractionPoint;
    }

    private void EnableInteractionPoint()
    {
        gameObject.SetActive(true);
    }

    private void DisableInteractionPoint()
    {
        gameObject.SetActive(false);
    }
}
