using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class VignetteFade : MonoBehaviour
{
    public CanvasGroup vignetteGroup;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    public void ShowVignette(float duration = 0.5f)
    {
        StartCoroutine(FadeVignette(0f, 1f, duration));
    }

    public void HideVignette(float duration = 0.5f)
    {
        StartCoroutine(FadeVignette(1f, 0f, duration));
    }

    public void FadeToScene(string sceneName, float fadeDuration = 1f)
    {
        StartCoroutine(FadeOutAndLoad(sceneName, fadeDuration));
    }

    private IEnumerator FadeVignette(float from, float to, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            vignetteGroup.alpha = Mathf.Lerp(from, to, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
        vignetteGroup.alpha = to;
    }

    private IEnumerator FadeOutAndLoad(string sceneName, float fadeDuration)
    {
        yield return FadeVignetteAndWait(0f, 1f, fadeDuration); // fade to black

        AsyncOperation op = SceneManager.LoadSceneAsync(sceneName);
        while (!op.isDone) yield return null;

        yield return new WaitForSeconds(0.2f); // brief pause to avoid flicker
        yield return FadeVignetteAndWait(1f, 0f, fadeDuration); // fade back in
    }

    private IEnumerator FadeVignetteAndWait(float from, float to, float duration)
    {
        float timer = 0f;
        while (timer < duration)
        {
            vignetteGroup.alpha = Mathf.Lerp(from, to, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }
        vignetteGroup.alpha = to;
    }
}
