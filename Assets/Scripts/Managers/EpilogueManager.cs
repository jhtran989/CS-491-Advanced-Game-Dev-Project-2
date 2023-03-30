using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EpilogueManager : MonoBehaviour
{
    public static TextMeshPro epilogueTMP;
    public static string epilogueTxt = "";
    public static void GenerateEpilogue(string[] terminalCodes) {
        //TODO: Add epilogue for running out of oxygen
        foreach (string code in terminalCodes) {
            switch(code) {
                case "ROCK":
                    epilogueTxt += "Welcome Home! Astronaut Narrowly Escapes Certain Cosmic Death.";
                    break;
                case "ROAD":
                    epilogueTxt += "Astronaut Dead! Astronaut Launched into Sun.";
                    break;
                case "ROAM":
                    epilogueTxt += "Astronaut Dead! Astronaut Launched into Black Hole.";
                    break;
                case "ROMA":
                    epilogueTxt += "Astronaut Dead! Astronaut Left Stranded on Mars.";
                    break;
                default:
                    epilogueTxt += "Astronaut Dead! Astronaut Suffocates on Space Station.";
                    break;
            }
        }
        epilogueTMP.SetText(epilogueTxt);
    }
}
