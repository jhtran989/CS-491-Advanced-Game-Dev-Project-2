using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class EscapeTrigger : MonoBehaviour
{
    private GameObject epilogueScreen, hud;
    private EpilogueManager epilogueManager;

    private void Start() {
        epilogueScreen = GameObject.Find("EpilogueScreen");
        epilogueManager = GameObject.Find("Epilogue").GetComponent<EpilogueManager>();
        hud = GameObject.Find("HUD");
    }
    private void OnTriggerEnter(Collider other) {
        epilogueScreen.SetActive(true);
        epilogueManager.GenerateEpilogue();
        hud.SetActive(false);
        Destroy(gameObject);
    }
}
