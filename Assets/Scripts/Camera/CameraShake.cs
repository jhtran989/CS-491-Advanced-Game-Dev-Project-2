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

            // TODO: Lock player movement, shake can sometimes separate player & camera

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