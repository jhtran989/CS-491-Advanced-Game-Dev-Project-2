using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour {

    public GameObject HUD, PauseScreen;
    private bool isPaused;

    private GameManager gm;

    private void Start() {
        gm = GameManager.instance;
        isPaused = false;
    }

    private void Update() {
        if (Input.GetKeyDown("Escape")) {
            if (isPaused) ResumeButton();
            else PauseButton();
        }
    }

    public void PlayButton() {
        gm.PlayGame();
    }

    public void QuitButton() {
        gm.QuitGame();
    }

    public void ResumeButton() {
        HUD.GetComponent<Renderer>().enabled = true;
        PauseScreen.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;  
        isPaused = false;
    }

    public void PauseButton() {
        HUD.GetComponent<Renderer>().enabled = false;
        PauseScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;  
        isPaused = true;
    }

}