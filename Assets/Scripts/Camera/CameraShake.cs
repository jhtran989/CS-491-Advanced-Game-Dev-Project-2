using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public AnimationCurve curve;
    public Player.PlayerMovement playerMovement;
    private PlayerCamera camera;
    public bool doShake;
    public float duration = 1f;

    private void Start() {
        camera = GetComponent<PlayerCamera>();
    }

    private void Update() {
        if (doShake) {
            doShake = false;
            StartCoroutine(Shake());
        }
    }

    IEnumerator Shake() {
        Vector3 originalPosition = transform.position;
        float elapsedTime = 0f;
        playerMovement.enabled = false;
        camera.AllowLook(false);
        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / duration);
            transform.position = originalPosition + Random.insideUnitSphere * strength;
            yield return null;
        }
        playerMovement.enabled = true;
        camera.AllowLook(true);
        transform.position = originalPosition;        
    }
}