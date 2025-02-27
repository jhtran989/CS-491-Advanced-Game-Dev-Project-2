using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalInteractable : AbstractInteractable, IInteractable
{
    public GameObject terminalCamera;

    public delegate void TerminalTriggerDelegate();
    public static TerminalTriggerDelegate TerminalTriggerPlayerEnter;

    public FireExtinguisherInteractableComponents fireExtinguisherInteractableComponents;

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
        // FIXME FINAL: remove constraint when not holding fire extinguisher
        // interactable only when not holding fire extinguisher (so can still interact while fire is present...)
        // if (!fireExtinguisherInteractableComponents.isHoldingFireExtinguisher)
        // {
        //     terminalCamera.SetActive(true);
        //     
        //     // TODO: need to disable player movement
        //     // TODO: need to lock the player camera (or disable camera)
        //     TerminalTriggerPlayerEnter?.Invoke();
        // }
        
        terminalCamera.SetActive(true);
            
        // TODO: need to disable player movement
        // TODO: need to lock the player camera (or disable camera)
        TerminalTriggerPlayerEnter?.Invoke();
    }
}
