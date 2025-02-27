using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EscapeTrigger : MonoBehaviour
{
    private GameObject epilogueScreen, hud;
    private GameManager gameManager;

    private void Start() {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }
    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag(Constants.PlayerTag))
        {
            Debug.Log("Entered escape pod...");
            gameManager.PlayEscPodLaunch();
            gameManager.EndGame();
        }
    }
}
