using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class introCutsceneController : MonoBehaviour
{
    [Header("UI References")]
    public CanvasGroup screenCanvasGroup;
    public Image screenImage;
    public TextMeshProUGUI subtitleText;
    public AudioSource voiceoverSource;

    [System.Serializable]
    public class ImageCue
    {
        public float timestamp;
        public Sprite image;
    }

    [System.Serializable]
    public class SubtitleCue
    {
        public float timestamp;
        [TextArea] public string subtitle;
    }

    public List<ImageCue> imageCues = new List<ImageCue>();
    public List<SubtitleCue> subtitleCues = new List<SubtitleCue>();

    [Header("Settings")]
    public float fadeDuration = 1.5f;
    public float cutsceneDelay = 5f;

    private void Start()
    {
        screenCanvasGroup.alpha = 0f;
        StartCoroutine(DelayedCutsceneStart());
    }

    private IEnumerator DelayedCutsceneStart()
    {
        yield return new WaitForSeconds(cutsceneDelay);

        subtitleText.text = "";

        if (imageCues.Count > 0)
        {
            screenImage.sprite = imageCues[0].image;
            screenImage.canvasRenderer.SetAlpha(0f);
        }

        yield return StartCoroutine(FadeInScreen());
        StartCoroutine(PlayCutscene());
    }

    private IEnumerator FadeInScreen()
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            screenCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        screenCanvasGroup.alpha = 1f;
    }

    private IEnumerator PlayCutscene()
    {
        voiceoverSource.Play();

        StartCoroutine(PlayImageCues());
        StartCoroutine(PlaySubtitleCues());

        yield return new WaitForSeconds(voiceoverSource.clip.length + 2f);
        subtitleText.text = "";
        screenImage.CrossFadeAlpha(0f, fadeDuration, false);
        yield return new WaitForSeconds(fadeDuration);
        screenImage.sprite = null;
    }

    private IEnumerator PlayImageCues()
    {
        float lastTime = 0f;

        for (int i = 0; i < imageCues.Count; i++)
        {
            var cue = imageCues[i];
            float wait = cue.timestamp - lastTime;
            if (wait > 0f) yield return new WaitForSeconds(wait);
            lastTime = cue.timestamp;

            screenImage.sprite = cue.image;
            screenImage.canvasRenderer.SetAlpha(0f);
            screenImage.CrossFadeAlpha(1f, fadeDuration, false);
        }
    }

    private IEnumerator PlaySubtitleCues()
    {
        float lastTime = 0f;

        for (int i = 0; i < subtitleCues.Count; i++)
        {
            var cue = subtitleCues[i];
            float wait = cue.timestamp - lastTime;
            if (wait > 0f) yield return new WaitForSeconds(wait);
            lastTime = cue.timestamp;

            subtitleText.text = cue.subtitle;
        }

        yield return new WaitForSeconds(2f); // show final subtitle for x seconds
        subtitleText.text = "";

    }
}
