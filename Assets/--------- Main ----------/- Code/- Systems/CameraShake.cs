using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour
{
    private Vector3 originalPos;
    private Quaternion originalRot;
    private Coroutine shakeCoroutine;

    /// <summary>
    /// Triggers the camera shake effect.
    /// </summary>
    /// <param name="duration">How long the shake lasts (in seconds).</param>
    /// <param name="magnitude">How strong the shake is.</param>
    /// <param name="rotate">If true, adds subtle rotation shake.</param>
    public void Shake(float duration = 0.5f, float magnitude = 0.3f, bool rotate = true)
    {
        if (shakeCoroutine != null)
            StopCoroutine(shakeCoroutine);

        shakeCoroutine = StartCoroutine(ShakeRoutine(duration, magnitude, rotate));
    }

    private IEnumerator ShakeRoutine(float duration, float magnitude, bool rotate)
    {
        originalPos = transform.localPosition;
        originalRot = transform.localRotation;

        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float percentComplete = elapsed / duration;

            // Fade out the intensity over time
            float currentMagnitude = magnitude * (1f - percentComplete);

            // Smooth position shake using Perlin noise
            float x = (Mathf.PerlinNoise(Time.time * 20f, 0f) - 0.5f) * 2f * currentMagnitude;
            float y = (Mathf.PerlinNoise(0f, Time.time * 20f) - 0.5f) * 2f * currentMagnitude;

            transform.localPosition = originalPos + new Vector3(x, y, 0f);

            // Optional rotation shake for extra impact
            if (rotate)
            {
                float rotZ = (Mathf.PerlinNoise(Time.time * 25f, 1f) - 0.5f) * 2f * currentMagnitude * 10f;
                transform.localRotation = originalRot * Quaternion.Euler(0f, 0f, rotZ);
            }

            yield return null;
        }

        // Reset to original state
        transform.localPosition = originalPos;
        transform.localRotation = originalRot;
    }
}
