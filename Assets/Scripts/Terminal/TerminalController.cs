using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TerminalController : MonoBehaviour
{
    public TextMeshProUGUI codeText;
    private string playerCode = "";
    public GameObject goodLight, badLight;
    public GameObject door, trigger;
    [SerializeField] private string[] correctCodes = new string[3];
    private bool[] letterIsUsed = new bool[10];
    private bool codeTested = false;
    public char[] letters = new char[9];
    public int terminalIndex;
    public bool isLaunchTerminal;
    public GameManager gameManager;
    public delegate void TerminalControllerDelegate();
    public static TerminalControllerDelegate TerminalControllerPlayerLeave;

    public bool unlockDoor = false;
    
    // need player camera to re-enable
    public GameObject playerCamera;

    void Start() {
        gameManager = GameManager.instance;
    }

    void Update()
    {
        if (playerCode.Length < 4) {
            if (Input.GetKeyDown(letters[0].ToString())) {
                AddToCode(char.ToUpper(letters[0]), 0);
            } else if (Input.GetKeyDown(letters[1].ToString())) {
                AddToCode(char.ToUpper(letters[1]), 1);
            } else if (Input.GetKeyDown(letters[2].ToString())) {
                AddToCode(char.ToUpper(letters[2]), 2);
            } else if (Input.GetKeyDown(letters[3].ToString())) {
                AddToCode(char.ToUpper(letters[3]), 3);
            } else if (Input.GetKeyDown(letters[4].ToString())) {
                AddToCode(char.ToUpper(letters[4]), 4);
            } else if (Input.GetKeyDown(letters[5].ToString())) {
                AddToCode(char.ToUpper(letters[5]), 5);
            } else if (Input.GetKeyDown(letters[6].ToString())) {
                AddToCode(char.ToUpper(letters[6]), 6);
            } else if (Input.GetKeyDown(letters[7].ToString())) {
                AddToCode(char.ToUpper(letters[7]), 7);
            } else if (Input.GetKeyDown(letters[8].ToString())) {
                AddToCode(char.ToUpper(letters[8]), 8);
            }
        } else if (Input.GetKeyDown("space") && !codeTested) {
            bool isValid = false;
            for (int i =  0;  i < correctCodes.Length; i++)  {
                if (playerCode ==  correctCodes[i]) isValid = true;
            }
            // TODO: add sound effects
            if (isValid) goodLight.SetActive(true);
            else badLight.SetActive(true);

            codeTested = true;
        }

        if (Input.GetKeyDown("return")) {
                gameObject.SetActive(false);
                gameManager.SaveTerminalCode(playerCode, terminalIndex);
                if (isLaunchTerminal) {
                    gameManager.ActivateEscapePod();
                    // door.SetActive(false);
                    // trigger.SetActive(true);
                }
                // need to unlock the constraints on Player position
                // need to enable the player camera again
                TerminalControllerPlayerLeave?.Invoke();
                // playerCamera.GetComponent<PlayerCamera>().EnableCamera();

                // TODO: unlock the door for now (maybe add other constraints later depending on code entered)
                unlockDoor = true;
                DoorController.unlockDoor?.Invoke();
        }
        
        if (Input.GetKeyDown("backspace")) {
                RemoveFromCode();
        }
    }

    private void RemoveFromCode() {
        if (playerCode.Length > 0) {
            int letterIdx = GetIndex(char.ToLower(playerCode[playerCode.Length - 1]));
            letterIsUsed[letterIdx] = false;
            playerCode = playerCode.Remove(playerCode.Length - 1);
            codeText.SetText(playerCode);
        }
    }

    private void AddToCode(char letter, int index) {
        if (!letterIsUsed[index]) {
            playerCode += letter;
            letterIsUsed[index] = true;
            codeText.SetText(playerCode);
        }
    }

    private int GetIndex(char letter) {
        for (int i = 0; i < 9; i++) {
            if (letters[i] == letter) return i;
        }
        return 9;
    }
    
    public void EnableTerminalController()
    {
        // easiest way is to enable script
        enabled = true;
    }

    private void DisableTerminalController()
    {
        // easiest way is to disable script
        enabled = false;
    }
}
