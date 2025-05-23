using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraEffectController : MonoBehaviour
{
    public CanvasGroup fadeGroup;     // CanvasGroup used for fading (controlling alpha on fade)
    public GameObject fadeImgObject;  // UI plane that is black = fade overlay.
    public GameObject screenEffectObj; 
    public GameObject xrOrig;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(screenEffectObj);
        DontDestroyOnLoad(xrOrig);
        fadeImgObject.SetActive(false);     // Hide fade on startup
    }

    public void FadeToScene(string sceneName, float fadeDuration = 1f) // Trigger fade when changing scene.
    {
        StartCoroutine(FadeOutAndLoad(sceneName, fadeDuration));
    }

    private IEnumerator FadeOutAndLoad(string sceneName, float fadeDuration) // Handle fade out, scene load and fade in.
    {

        fadeImgObject.SetActive(true); // Enable black image overlay
        yield return Fade(0f, 1f, fadeDuration); // Fade from transparent to black.

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName); // Load scene in background, prevent stuttering/freeze
        while (!op.isDone) yield return null;

        yield return new WaitForSeconds(0.1f); // safety delay
        yield return Fade(1f, 0f, fadeDuration); // fade back in

        fadeImgObject.SetActive(false); // hide fade again
    }

    private IEnumerator Fade(float from, float to, float duration) // Controls fade time.
    {
        float timer = 0f;
        fadeGroup.alpha = from;

        while (timer < duration) // Fade during duration.
        {
            fadeGroup.alpha = Mathf.Lerp(from, to, timer / duration);
            timer += Time.deltaTime; // Increment timer by frame time.
            yield return null; // Wait for next frame.
        }

        fadeGroup.alpha = to; // Ensure final alpha is set.
    }
}
