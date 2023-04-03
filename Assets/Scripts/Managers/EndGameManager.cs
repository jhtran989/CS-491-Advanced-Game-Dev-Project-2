using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameManager : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Additive);
            SceneManager.UnloadScene("Terminal");
            SceneManager.UnloadScene("Station");
            SceneManager.UnloadScene("Fire");
            SceneManager.UnloadScene("Doors");
            SceneManager.LoadScene("Terminal", LoadSceneMode.Additive);
            SceneManager.LoadScene("Station", LoadSceneMode.Additive);
            SceneManager.LoadScene("Fire", LoadSceneMode.Additive);
            SceneManager.LoadScene("Doors", LoadSceneMode.Additive);
        } else if (Input.GetKeyDown(KeyCode.Escape)) {
            Application.Quit();
        }
    }
}
