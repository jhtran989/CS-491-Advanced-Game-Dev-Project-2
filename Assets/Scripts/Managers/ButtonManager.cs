using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour {

    private GameManager gm;

    private void Start() {
        gm = GameManager.instance;
    }

    public void PlayButton() {
        gm.PlayGame();
    }

    public void QuitButton() {
        gm.QuitGame();
    }

}