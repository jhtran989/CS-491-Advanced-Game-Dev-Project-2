using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour {

    public GameObject HUD, PauseScreen;
    public PlayerCamera cameraMovement;
    public Player.PlayerMovement playerMovemeent;
    private bool isPaused;

    private GameManager gm;

    private void Start() {
        gm = GameManager.instance;
        isPaused = false;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
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
        HUD.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 10f);
        // HUD.transform.position = new Vector3(0, 10.0f, 0);
        PauseScreen.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;  
        isPaused = false;
        playerMovemeent.enabled = true;
        cameraMovement.enabled = true;
    }

    public void PauseButton() {
        HUD.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 50f);
        // HUD.transform.position = new Vector3(0, 50.0f, 0);
        PauseScreen.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;  
        isPaused = true;
        playerMovemeent.enabled = false;
        cameraMovement.enabled = false;
    }

}