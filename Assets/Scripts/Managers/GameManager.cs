using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject escapePodDoor, escapeTrigger;
    private static string[] playerCodes = {"", "", "", ""}; // 0: pod, 1: nav, 2: dock, 3: launch
    private static bool oxygenDepleted = false;

    public GameObject playerObject;

    private void Awake() {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else Destroy(gameObject);
    }

    public void EndGame() {
        SceneManager.LoadScene("EndScreen");
    }

    public void PlayGame() {
        // TODO: make sure to add all the scenes
        SceneManager.LoadScene("Main");
        SceneManager.LoadScene("Space Station", LoadSceneMode.Additive);
        SceneManager.LoadScene("Furniture", LoadSceneMode.Additive);
        clearRunData();
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void ActivateEscapePod() {
        if (playerCodes[3] != "TIME") {
            escapePodDoor.SetActive(false);
            escapeTrigger.SetActive(true);
            // TODO: play announcement
        } else {
            // TODO: play announcement
        }
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
