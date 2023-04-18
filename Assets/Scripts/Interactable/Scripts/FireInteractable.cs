using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireInteractable : MonoBehaviour, IInteractable
{
    [SerializeField] 
    private string interactText;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact(Transform interactorTransform)
    {
        // throw new System.NotImplementedException();
    }

    public string GetInteractText()
    {
        return interactText;
    }

    public Transform GetTransform()
    {
        return transform;
    }
}
