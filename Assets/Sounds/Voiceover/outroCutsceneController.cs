using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class outroCutsceneController : MonoBehaviour
{
    [Header("UI References")]
    public CanvasGroup screenCanvasGroup;     // Assign your screen CanvasGroup
    public Image screenImage;                 // The image that displays cutscene visuals
    public TextMeshProUGUI subtitleText;      // TMP Text for subtitles
    public AudioSource voiceoverSource;       // AudioSource with your outro voiceover

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

    [Header("Cutscene Data")]
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

        // Optional: transition to next scene
        // SceneManager.LoadScene("CreditsScene");
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

            Debug.Log($"Showing image {i} at {cue.timestamp}s");
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

            Debug.Log($"Subtitle {i} at {cue.timestamp}s: {cue.subtitle}");
        }

        // Show the last subtitle a bit longer before clearing it
        yield return new WaitForSeconds(6f);
        subtitleText.text = "";
    }
}
