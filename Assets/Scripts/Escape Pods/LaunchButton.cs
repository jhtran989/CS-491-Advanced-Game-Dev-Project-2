using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaunchButton : MonoBehaviour
{   
    public GameObject escapePodDoor; 
    public float duration = 1f;

    private void OnTriggerStay(Collider other) {
        if (Input.GetMouseButtonDown(0)) {
            escapePodDoor.SetActive(false);
            gameObject.GetComponent<Animator>().Play("MoveButton", 0, 0.0f);
            // TODO: Add Increased Oxygen Consumption OR Countdown Timer
        }
    }

}
