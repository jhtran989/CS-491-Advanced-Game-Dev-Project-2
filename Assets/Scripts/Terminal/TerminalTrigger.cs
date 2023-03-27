using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalTrigger : MonoBehaviour
{
    public GameObject terminalCamera;
    private void OnTriggerStay(Collider other) {
        if (Input.GetMouseButtonDown(0)) {
            terminalCamera.SetActive(true);
        }
    }
}
