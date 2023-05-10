using System;
using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.Assertions;
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
    public AudioClip[] sfx; // 0: opening, 1; fire, 2: short fire, 3: esc run, 4: esc pod sound
    public GameObject playerObject;
    public bool firstRun = true;
    
    private void Awake() {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else Destroy(gameObject);

        UpdatePlayerObject();
    }

    public void UpdatePlayerObject()
    {
        Debug.Log("Generating game manager...");
        Debug.Log("main scene loaded " + SoundManager.IsSceneLoaded(Constants.MainSceneName));
        
        if (SoundManager.IsSceneLoaded(Constants.MainSceneName) && playerObject == null)
        {
            var player1 = GameObject.Find(Constants.PlayerContainerName);
            Assert.IsNotNull(player1);
            
            var player2 = player1.GetComponentInChildren<PlayerMovement>();
            Assert.IsNotNull(player2);
            
            playerObject = GameObject.Find(Constants.PlayerContainerName).GetComponentInChildren<PlayerMovement>()
                .gameObject;
        }
    }

    private void Start()
    {
        
    }

    public void EndGame() {
        SceneManager.LoadScene("EndScreen");
        firstRun = false;
    }

    public void PlayGame() {
        // TODO: make sure to add all the scenes
        SceneManager.LoadScene("Main");
        SceneManager.LoadScene("Space Station", LoadSceneMode.Additive);
        SceneManager.LoadScene("Furniture", LoadSceneMode.Additive);
        clearRunData();
        if (firstRun) {
            audio.PlayOneShot(sfx[0]);
        }
        secondaryAudio.PlayOneShot(sfx[4]);
        
        // update the music when loading
        SoundManager.instance.PlayStartingSound();
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void PlayFireAlert() {
        if (!fireTutorialComplete & firstRun) {
            audio.PlayOneShot(sfx[1]);
            fireTutorialComplete = true;
        } else {
            audio.PlayOneShot(sfx[2]);
        }
    }

    public void PlayEscPodLaunch() {
        secondaryAudio.PlayOneShot(sfx[4]);
    }

    public void ActivateEscapePod() {
        if (playerCodes[3] == "TIME") {
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

    public void SetEscapeObjects(GameObject door, GameObject trigger) {
        escapePodDoor = door;
        escapeTrigger = trigger;
    }
}
