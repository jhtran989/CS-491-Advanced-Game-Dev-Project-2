using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireManager : MonoBehaviour
{
    public GameObject fireParent;
    private Transform _fireParentTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        _fireParentTransform = fireParent.transform;
        Debug.Log("Num fires: " + _fireParentTransform.GetChildCountActive());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int GetNumActiveFires()
    {
        return _fireParentTransform.GetChildCountActive();
    }
}
