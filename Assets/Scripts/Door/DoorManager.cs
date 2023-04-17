using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class DoorManager : MonoBehaviour
{
    [Space, Header("Door")]
    public Door door;
    
    // get objects that influence when a door is unlocked
    // without need of doing spatial checks...
    [Space, Header("Fire")] 
    public string fireObjectName;
    [FormerlySerializedAs("_fire")] public Fire fire;
    private bool _fireCheck = false;
    
    [Space, Header("Terminal")] 
    public string terminalObjectName;
    private GameObject _terminalGameObject;
    private TerminalTrigger _terminalTrigger;
    private TerminalController _terminalController;
    private bool _terminalCheck = false;
    
    public bool doorOptionCheck;

    private static readonly string EmptyString = Utilities.EmptyString;

    public delegate void UnlockDoorDelegate();
    public static UnlockDoorDelegate unlockDoor;

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
        door = gameObject.GetComponentInChildren<Door>();
        
        doorOptionCheck = true;
    }

    private void OnEnable()
    {
        unlockDoor += CheckUnlockDoor;
    }

    private void OnDisable()
    {
        unlockDoor -= CheckUnlockDoor;
    }

    // Start is called before the first frame update
    void Start()
    {
        // FIXME: need to check for empty string instead of null
        // get the corresponding fire and terminal (if applicable -- should be null otherwise)
        if (fireObjectName != EmptyString)
        {
            // FIXME: move to one scene to get fires that are hidden...
            // fire = GameObject.Find(fireObjectName).GetComponent<Fire>();
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
            _terminalGameObject = GameObject.Find(terminalObjectName);
            _terminalTrigger = _terminalGameObject
                .GetComponentInChildren<TerminalTrigger>(true);
            _terminalController = _terminalGameObject
                .GetComponentInChildren<TerminalController>(true);
            _terminalCheck = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
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
            return !_fireCheck || fire.unlockDoor;
        } 
        else if (doorOptionsEnum == DoorOptionsEnum.Terminal)
        {
            return !_terminalCheck || _terminalController.unlockDoor;
        }

        // default to false for testing
        return false;
    }

    /// <summary>
    /// Should be moved to each instance where one portion of unlocking the door could occur
    /// </summary>
    public void CheckUnlockDoor()
    {
        Debug.Log("Door manager, check unlock door");
        
        if (doorOptionCheck)
        {
            var unlockCondition = true;
            
            foreach (var doorOptionsEnum in Utilities.GetValues<DoorOptionsEnum>())
            {
                unlockCondition = unlockCondition && CheckDoorOptions(doorOptionsEnum);
                door.UnlockCondition = unlockCondition;
            }
            
            // FIXME: need to abstract this into rooms...
            if (_terminalCheck)
            {
                // make terminal interactable if fire was put out
                if (CheckDoorOptions(DoorOptionsEnum.Fire))
                {
                    // _terminalController.EnableTerminalController();
                    _terminalTrigger.gameObject.SetActive(true);
                }
            }

            // FIXME: move to animation logic...
            if (unlockCondition)
            {
                door.UnlockDoor();
                doorOptionCheck = false;
            }
        }
    }
}
