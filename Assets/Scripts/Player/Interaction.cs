using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{
   public Transform interactionPoint;
   public float interactionRadius = 0.5f;
   bool active = false;

   private void Update() {
    RaycastHit hit;
    active = Physics.Raycast(interactionPoint.position, interactionPoint.TransformDirection(Vector3.forward), out hit, interactionRadius);

    if (active && Input.GetKeyDown(KeyCode.E)) {
      Debug.Log("Interaction");
    }
   }
}
