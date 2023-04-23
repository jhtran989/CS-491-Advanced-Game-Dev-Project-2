using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EpilogueManager : MonoBehaviour
{
    public static TextMeshProUGUI missionReportTMP, codeLogTMP;
    public GameManager gameManager;
    RawImage epilogueImage;
    public Texture[] epilogueImages;
    private string missionReport = "";
    private string codeLog = "";

    private void Start() {
        missionReportTMP = GameObject.Find("MissionReport").GetComponent<TextMeshProUGUI>();
        codeLogTMP = GameObject.Find("CodeLog").GetComponent<TextMeshProUGUI>();
        GenerateMissionReport();
    }

    public void GenerateMissionReport() {
        string[] playerCodes = gameManager.GetCodes();
        bool oxygenDepleted = gameManager.IsOxygenDepleted();
        // 0: pod, 1: nav, 2: dock
        foreach (string code in playerCodes) {
            if (code == "") codeLog += "----<br>";
            else codeLog += code + "<br>";
        }

        if (oxygenDepleted) {
            // oxygen supply depleted
            missionReport += "No escape pod launched.<br><br>"
                        + "Oxygen supply depleted.<br><br>"
                        + "No survivors.<br>";
            epilogueImage.texture = epilogueImages[0];
        } else {
            if (playerCodes[0] != "5297") {
                // invalid dock pod code
                missionReport += "No escape pod launched.<br><br>"
                            + "Space station consumed by flames.<br><br>"
                            + "No survivors.<br>";
                epilogueImage.texture = epilogueImages[1];    
            } else {
                switch(playerCodes[1]) {
                    case "3084":
                        // earth nav code
                        missionReport += "Escape pod launched.<br><br>"
                                    + "Destination: Earch.<br><br>"
                                    + "One survivor.<br>";
                        epilogueImage.texture = epilogueImages[2];
                        break;
                    case "3831":
                        // mars nav code
                        missionReport += "Escape pod launched.<br><br>"
                                    + "Destination: Mars.<br><br>"
                                    + "No survivors.<br>";
                        epilogueImage.texture = epilogueImages[3];
                        break;
                    case "3309":
                        // sun nav code
                        missionReport += "Escape pod launched.<br><br>"
                                    + "Destination: Sun.<br><br>"
                                    + "No survivors.<br>";
                        epilogueImage.texture = epilogueImages[4];
                        break;
                    default:
                        // invalid nav code
                        missionReport += "Escape pod launched.<br><br>"
                                    + "Destination: Unknown.<br><br>"
                                    + "No survivors.<br>";
                        epilogueImage.texture = epilogueImages[5];
                        break;
                }
            }
        }
        missionReportTMP.SetText(missionReport);     
        codeLogTMP.SetText(codeLog);   
        gameManager.clearRunData();
    }
}
