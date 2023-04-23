using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject escapePodDoor, escapeTrigger;
    private static string[] playerCodes = new string[4];
    private static bool oxygenDepleted = false;

    private void Awake() {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void EndGame() {
        SceneManager.LoadScene("EndScreen");
    }

    public void PlayGame() {
        SceneManager.LoadScene("Main");
        SceneManager.LoadScene("Space Station", LoadSceneMode.Additive);
        clearRunData();
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void ActivateEscapePod() {
        escapePodDoor.SetActive(false);
        escapeTrigger.SetActive(true);
    }

    public void SaveTerminalCode(string code, int index) 
    {
        playerCodes[index] = code;
    }

    public string[] GetCodes() {
        return playerCodes;
    }

    public void SetOxygenDepletion() {
        oxygenDepleted = true;
    }

    public bool IsOxygenDepleted() {
        return oxygenDepleted;
    }

    public void clearRunData() {
        oxygenDepleted = false;
        for (int i = 0; i < playerCodes.Length; i++) {
            playerCodes[i] = "";
        }
    }
}
