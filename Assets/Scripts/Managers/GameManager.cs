using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private static string[] terminalCodes = new string[1];

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void SaveTerminalCode(int terminal, string code) 
    {
        terminalCodes[terminal] = code;
    }

    public string[] GetCodes() {
        return terminalCodes;
    }

}
