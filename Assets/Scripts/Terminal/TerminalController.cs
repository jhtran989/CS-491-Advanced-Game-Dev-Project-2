using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TerminalController : MonoBehaviour
{
    public TextMeshProUGUI codeText;
    private string playerCode = "";
    private bool[] letterIsUsed = new bool[10];
    public GameManager gameManager;
    public delegate void TerminalControllerDelegate();
    public static TerminalControllerDelegate TerminalControllerPlayerLeave;

    public bool unlockDoor = false;

    void Update()
    {
        if (playerCode.Length < 4) {
            if (Input.GetKeyDown("d")) {
                AddToCode('D');
            } else if (Input.GetKeyDown("r")) {
                AddToCode('R');
            } else if (Input.GetKeyDown("a")) {
                AddToCode('A');
            } else if (Input.GetKeyDown("e")) {
                AddToCode('E');
            } else if (Input.GetKeyDown("t")) {
                AddToCode('T');
            } else if (Input.GetKeyDown("b")) {
                AddToCode('B');
            } else if (Input.GetKeyDown("i")) {
                AddToCode('I');
            } else if (Input.GetKeyDown("s")) {
                AddToCode('S');
            } else if (Input.GetKeyDown("k")) {
                AddToCode('K');
            }
        } 

        if (Input.GetKeyDown("return")) {
                gameObject.SetActive(false);
                GameObject.Find("GameManager").GetComponent<GameManager>().SaveTerminalCode(playerCode);
                
                // need to unlock the constraints on Player position
                TerminalControllerPlayerLeave?.Invoke();
                
                // TODO: unlock the door for now (maybe add other constraints later depending on code entered)
                unlockDoor = true;
                DoorManager.unlockDoor?.Invoke();
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

    private void AddToCode(char letter) {
        int letterIdx = GetIndex(char.ToLower(letter));
        if (!letterIsUsed[letterIdx]) {
            playerCode += letter;
            letterIsUsed[letterIdx] = true;
            codeText.SetText(playerCode);
        }
    }

    private int GetIndex(char letter) {
        switch (letter) {
            case 'd':
                return 0;
            case 'r':
                return 1;
            case 'a':
                return 2;
            case 'e':
                return 3;
            case 't':
                return 4;
            case 'b':
                return 5;
            case 'i':
                return 6;
            case  's':
                return 7;
            case 'k':
                return 8;
            default:
                return 9;
        }
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
