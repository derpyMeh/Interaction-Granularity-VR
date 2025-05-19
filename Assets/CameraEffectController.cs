using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraEffectController : MonoBehaviour
{
    public CanvasGroup fadeGroup;     // assign the CanvasGroup on FadeImg
    public GameObject fadeImgObject;  // assign the FadeImg GameObject

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject); // optional if you want this to persist
        fadeImgObject.SetActive(false);     // hide on startup
    }

    public void FadeToScene(string sceneName, float fadeDuration = 1f)
    {
        StartCoroutine(FadeOutAndLoad(sceneName, fadeDuration));
    }

    private IEnumerator FadeOutAndLoad(string sceneName, float fadeDuration)
    {
        fadeImgObject.SetActive(true); // enable black image
        yield return Fade(0f, 1f, fadeDuration); // fade to black

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        while (!op.isDone) yield return null;

        yield return new WaitForSeconds(0.1f); // safety delay
        yield return Fade(1f, 0f, fadeDuration); // fade back in

        fadeImgObject.SetActive(false); // hide again
    }

    private IEnumerator Fade(float from, float to, float duration)
    {
        float timer = 0f;
        fadeGroup.alpha = from;

        while (timer < duration)
        {
            fadeGroup.alpha = Mathf.Lerp(from, to, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        fadeGroup.alpha = to;
    }
}
