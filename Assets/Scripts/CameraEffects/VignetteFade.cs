using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VignetteFade : MonoBehaviour
{
    public CanvasGroup vignetteGroup; // CanvasGroup controlling vignette alpha.

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject); // Keeps this object active across scene loads
    }

    public void ShowVignette(float duration = 0.5f) // Method to fade in the vignette.
    {
        StartCoroutine(FadeVignette(0f, 1f, duration));
    }

    public void HideVignette(float duration = 0.5f) // Method to fade out the vignette
    {
        StartCoroutine(FadeVignette(1f, 0f, duration));
    }

    private IEnumerator FadeVignette(float from, float to, float duration) // Fading the vignette over time.
    {
        float timer = 0f;
        while (timer < duration)
        {
            vignetteGroup.alpha = Mathf.Lerp(from, to, timer / duration);
            timer += Time.deltaTime; // Increment time by time between frames.
            yield return null; // Wait for next frame.
        }
        vignetteGroup.alpha = to; // Ensure final alpha is set
    }

}
