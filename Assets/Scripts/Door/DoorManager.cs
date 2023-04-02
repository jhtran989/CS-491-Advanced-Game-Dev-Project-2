using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    [Space, Header("Door")]
    private Door _door;
    
    // get objects that influence when a door is unlocked
    // without need of doing spatial checks...
    [Space, Header("Fire")] 
    public string fireObjectName;
    private Fire _fire;
    private bool _fireCheck = false;
    
    [Space, Header("Terminal")] 
    public string terminalObjectName;
    private TerminalController _terminalController;
    private bool _terminalCheck = false;
    
    private bool _doorOptionCheck = true;

    private static readonly string EmptyString = Utilities.EmptyString;

    /******************************************************************/
    
    private enum DoorOptionsEnum
    {
        Fire,
        Terminal
    }
    
    private class DoorOptions
    {
        
    }

    private Dictionary<DoorOptionsEnum, List<DoorOptions>> doorOptionsDict = 
        new Dictionary<DoorOptionsEnum, List<DoorOptions>>();

    /******************************************************************/

    private void Awake()
    {
        // returns only the first component found - only the DoorCenterFrame should have it
        _door = gameObject.GetComponentInChildren<Door>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // FIXME: need to check for empty string instead of null
        // get the corresponding fire and terminal (if applicable -- should be null otherwise)
        if (fireObjectName != EmptyString)
        {
            _fire = GameObject.Find(fireObjectName).GetComponent<Fire>();
            _fireCheck = true;
        }

        if (terminalObjectName != EmptyString)
        {
            // Debug.Log("Terminal: " + terminalObjectName);
            // if (terminalObjectName == "")
            // {
            //     Debug.Log("Empty terminal, NOT NULL");
            // }
            
            // IMPORTANT: need to include inactive in search
            _terminalController = GameObject.Find(terminalObjectName)
                .GetComponentInChildren<TerminalController>(true);
            _terminalCheck = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_doorOptionCheck)
        {
            var unlockCondition = true;
            
            foreach (var doorOptionsEnum in Utilities.GetValues<DoorOptionsEnum>())
            {
                unlockCondition = unlockCondition && CheckDoorOptions(doorOptionsEnum);
            }

            if (unlockCondition)
            {
                _door.UnlockDoor();
                _doorOptionCheck = false;
            }
        }
    }

    /// <summary>
    ///
    /// logic is dependent on the corresponding check (if is not null) and if so, check the unlock condition
    /// 
    /// </summary>
    /// <param name="doorOptionsEnum"></param>
    /// <returns></returns>
    private bool CheckDoorOptions(DoorOptionsEnum doorOptionsEnum)
    {
        if (doorOptionsEnum == DoorOptionsEnum.Fire)
        {
            return !_fireCheck || _fire.unlockDoor;
        } 
        else if (doorOptionsEnum == DoorOptionsEnum.Terminal)
        {
            return !_terminalCheck || _terminalController.unlockDoor;
        }

        // default to false for testing
        return false;
    }
}
