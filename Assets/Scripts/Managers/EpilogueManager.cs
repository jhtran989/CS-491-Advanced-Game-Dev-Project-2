using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EpilogueManager : MonoBehaviour
{
    public static TextMeshProUGUI epilogueTMP;
    public GameManager gameManager;
    public GameObject epilogueScreen, player;
    public static string epilogueTxt = "";

    private void Start() {
        epilogueTMP = GameObject.Find("Epilogue").GetComponent<TextMeshProUGUI>();
        epilogueScreen.SetActive(false);
    }

    public void GenerateEpilogue() {
        epilogueScreen.SetActive(true);
        player.SetActive(false);
        //TODO: Add epilogue for running out of oxygen
        string code = gameManager.GetCode(); 
        switch(code) {
                case "DIRT":
                    epilogueTxt += "Mission Report<br>_____________<br><br>"
                                + "Space station destroyed following cosmic event.<br>"
                                + "Escape pod was deployed with navigation code: DIRT<br>"
                                + "Safely returned to Earth with one survivor.<br><br>"
                                + "[ x ] Success<br>[   ] Failure";
                    break;
                case "DIET":
                    epilogueTxt += "Mission Report<br>_____________<br><br>"
                                + "Space station destroyed following cosmic event.<br>"
                                + "Escape pod was deployed with navigation code: DIET<br>"
                                + "Collided with sun. No survivors.<br><br>"
                                + "[   ] Success<br>[ x ] Failure";
                    break;
                case "DART":
                    epilogueTxt += "Mission Report<br>_____________<br><br>"
                                + "Space station destroyed following cosmic event.<br>"
                                + "Escape pod was deployed with navigation code: DART<br>"
                                + "Collided with sun. No survivors.<br><br>"
                                + "[   ] Success<br>[ x ] Failure";
                    break;
                case "DARK":
                    epilogueTxt += "Mission Report<br>_____________<br><br>"
                                + "Space station destroyed following cosmic event.<br>"
                                + "Escape pod was deployed with navigation code: DARK<br>"
                                + "Collided with sun. No survivors.<br><br>"
                                + "[   ] Success<br>[ x ] Failure";
                    break;
                case "":
                    epilogueTxt += "Mission Report<br>_____________<br><br>"
                                + "Space station destroyed following cosmic event.<br>"
                                + "Escape pod remained with station. No navigation code entered."
                                + "<br>No survivors.<br><br>"
                                + "[   ] Success<br>[ x ] Failure";
                    break;
                default:
                    epilogueTxt += "Mission Report<br>_____________<br><br>"
                                + "Space station destroyed following cosmic event.<br>"
                                + "Escape pod was deployed with navigation code: "
                                + code
                                + "<br>Whereabout unknown. No survivors likely.<br><br>"
                                + "[   ] Success<br>[ x ] Failure";
                    break;
            }
        epilogueTMP.SetText(epilogueTxt);
    }
}
