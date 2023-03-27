using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TerminalController : MonoBehaviour
{
    public TextMeshProUGUI codeText;
    private string playerCode = "";
    private bool[] letterIsUsed = new bool[10];

    void Update()
    {
        if (playerCode.Length < 4) {
            if (Input.GetKeyDown("c")) {
                AddToCode('C');
            } else if (Input.GetKeyDown("o")) {
                AddToCode('O');
            } else if (Input.GetKeyDown("m")) {
                AddToCode('M');
            } else if (Input.GetKeyDown("r")) {
                AddToCode('R');
            } else if (Input.GetKeyDown("k")) {
                AddToCode('K');
            } else if (Input.GetKeyDown("a")) {
                AddToCode('A');
            } else if (Input.GetKeyDown("t")) {
                AddToCode('T');
            } else if (Input.GetKeyDown("x")) {
                AddToCode('X');
            } else if (Input.GetKeyDown("d")) {
                AddToCode('D');
            }
        } else {
            if (Input.GetKeyDown("return")) {
                gameObject.SetActive(false);
            }
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
            case 'c':
                return 0;
            case 'o':
                return 1;
            case 'm':
                return 2;
            case 'r':
                return 3;
            case 'k':
                return 4;
            case 'a':
                return 5;
            case 't':
                return 6;
            case  'x':
                return 7;
            case 'd':
                return 8;
            default:
                return 9;
        }
    }
}
