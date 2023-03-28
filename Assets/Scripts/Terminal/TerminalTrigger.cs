using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: check out World Space Canvas (UI?)

public class TerminalTrigger : MonoBehaviour
{
    public GameObject terminalCamera;

    public delegate void TerminalTriggerDelegate();
    public static TerminalTriggerDelegate TerminalTriggerPlayerEnter;
    
    private void OnTriggerStay(Collider other) {
        if (Input.GetMouseButtonDown(0)) {
            terminalCamera.SetActive(true);
            
            // TODO: need to disable player movement
            TerminalTriggerPlayerEnter?.Invoke();
        }
    }
}
