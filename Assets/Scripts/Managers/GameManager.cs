using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private static string terminalCode = "";

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void SaveTerminalCode(string code) 
    {
        terminalCode = code;
    }

    public string GetCode() {
        return terminalCode;
    }

}
