using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public AnimationCurve curve;
    public bool doShake;
    public float duration = 1f;

    private void Update() {
        if (doShake) {
            doShake = false;
            StartCoroutine(Shake());
        }
    }

    IEnumerator Shake() {
        Vector3 originalPosition = transform.position;
        float elapsedTime = 0f;

        while (elapsedTime < duration) {
            elapsedTime += Time.deltaTime;
            float strength = curve.Evaluate(elapsedTime / duration);
            transform.position = originalPosition + Random.insideUnitSphere * strength;
            yield return null;
        }

        transform.position = originalPosition;        
    }
}