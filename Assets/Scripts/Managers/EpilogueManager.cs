using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EpilogueManager : MonoBehaviour
{
    public TextMeshProUGUI missionReportTMP;
    public TextMeshProUGUI codeLogTMP;
    private GameManager gameManager;
    public RawImage epilogueImage;
    public Texture[] epilogueImages;
    private string missionReport = "";
    private string codeLog = "";

    private void Start() {
        gameManager = GameManager.instance;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;  
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
            if (playerCodes[2] != "5297" || playerCodes[0] != "LUNA") {
                // invalid dock or pod code
                missionReport += "No escape pod launched.<br><br>"
                            + "Space station consumed by flames.<br><br>"
                            + "No survivors.<br>";
                epilogueImage.texture = epilogueImages[1];    
            } else {
                switch(playerCodes[1]) {
                    case "4896":
                        // earth nav code
                        missionReport += "Escape pod launched.<br><br>"
                                    + "Destination: Earch.<br><br>"
                                    + "One survivor.<br>";
                        epilogueImage.texture = epilogueImages[2];
                        break;
                    case "3951":
                        // mars nav code
                        missionReport += "Escape pod launched.<br><br>"
                                    + "Destination: Mars.<br><br>"
                                    + "No survivors.<br>";
                        epilogueImage.texture = epilogueImages[3];
                        break;
                    case "3491":
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
