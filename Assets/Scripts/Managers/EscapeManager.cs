using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeManager : MonoBehaviour
{
    public GameObject door;
    public GameObject trigger;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.SetEscapeObjects(door, trigger);
    }
}
