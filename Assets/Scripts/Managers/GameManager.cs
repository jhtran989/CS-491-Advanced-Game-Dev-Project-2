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
    private bool fireTutorialComplete = false;
    public AudioSource audio;
    public AudioSource secondaryAudio;
    public AudioClip[] sfx;
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
        audio.PlayOneShot(sfx[0]);
        secondaryAudio.PlayOneShot(sfx[4]);
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void PlayFireAlert() {
        if (!fireTutorialComplete) {
            audio.PlayOneShot(sfx[1]);
            fireTutorialComplete = true;
        } else {
        //    audio.PlayOneShot(sfx[2]);
        }
    }

    public void PlayEscPodLaunch() {
        audio.PlayOneShot(sfx[4]);
    }

    public void ActivateEscapePod() {
        if (playerCodes[3] != "TIME") {
            escapePodDoor.SetActive(false);
            escapeTrigger.SetActive(true);
            audio.PlayOneShot(sfx[3]);
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
