using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalDoorManager : MonoBehaviour
{
    [Space, Header("HUD")] 
    public HUDManager hudManager;

    public Oxygen oxygen;
    
    // Start is called before the first frame update
    void Start()
    {
        oxygen = hudManager.OxygenObject.GetComponent<Oxygen>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
