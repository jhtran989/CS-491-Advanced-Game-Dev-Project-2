using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalDoorManager : MonoBehaviour
{
    [Space, Header("HUD")] 
    public HUDManager hudManager;
    
    [Space, Header("Fire")] 
    public FireManager fireManager;

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

    public void SetFirePresent()
    {
        // set fire present to true for updated oxygen...
        oxygen.firePresent = true;
        
        // update number of fires
        fireManager.RecalculateNumActiveFires();
    }
}
