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

    // Class for mapping an image to a specific timestamp
    [System.Serializable]
    public class ImageCue
    {
        public float timestamp; // When to show this image (in seconds)
        public Sprite image; // The sprite to display at this time
    }
    // Class for mapping a subtitle to a specific timestamp
    [System.Serializable]
    public class SubtitleCue
    {
        public float timestamp;// When to show this subtitle (in seconds)
        [TextArea] public string subtitle; // Subtitle text to show
    }

    [Header("Cutscene Data")]
    public List<ImageCue> imageCues = new List<ImageCue>(); // Timeline of image changes
    public List<SubtitleCue> subtitleCues = new List<SubtitleCue>(); // Timeline of subtitle updates

    [Header("Settings")]
    public float fadeDuration = 1.5f; // Duration for fading the canvas in/out
    public float cutsceneDelay = 5f; // Delay before the cutscene starts

    private void Start()
    {
        screenCanvasGroup.alpha = 0f;  // Start fully invisible
        StartCoroutine(DelayedCutsceneStart()); // Wait, then begin cutscene
    }

    private IEnumerator DelayedCutsceneStart()     // Waits a few seconds before starting the cutscene
    {
        yield return new WaitForSeconds(cutsceneDelay); // Initial delay before showing anything

        subtitleText.text = ""; // Clear subtitles at start


        if (imageCues.Count > 0)  // If there's at least one image cue, show the first image immediately (but invisible)
        { 
            screenImage.sprite = imageCues[0].image;
            screenImage.canvasRenderer.SetAlpha(0f); // Set to invisible for fade-in
        }

        yield return StartCoroutine(FadeInScreen()); // Fade screen in

        StartCoroutine(PlayCutscene());
    }

    private IEnumerator FadeInScreen()  // Fades the entire UI canvas in from black
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            screenCanvasGroup.alpha = Mathf.Lerp(0f, 1f, timer / fadeDuration); // Smooth alpha transition
            timer += Time.deltaTime;
            yield return null;
        }
         
        screenCanvasGroup.alpha = 1f; // Ensure it's fully visible at the end
    } 

    private IEnumerator PlayCutscene()  // Plays the voiceover and starts the parallel image and subtitle playback
    {
        voiceoverSource.Play(); // Start voiceover audio

        StartCoroutine(PlayImageCues());  // Play image transitions in parallel
        StartCoroutine(PlaySubtitleCues()); // Play subtitle transitions in parallel

        yield return new WaitForSeconds(voiceoverSource.clip.length + 2f);  // Wait for the voiceover to finish + 2 seconds of buffer
        subtitleText.text = "";  // Clear subtitle
        screenImage.CrossFadeAlpha(0f, fadeDuration, false); // Fade image out
        yield return new WaitForSeconds(fadeDuration);
        screenImage.sprite = null; // Remove image reference
    }

    private IEnumerator PlayImageCues()  // Displays each image at the correct time
    {
        float lastTime = 0f; // Track previous cue time to calculate waiting duration

        for (int i = 0; i < imageCues.Count; i++)
        {
            var cue = imageCues[i];
            float wait = cue.timestamp - lastTime;
            if (wait > 0f) yield return new WaitForSeconds(wait); // Wait for the right moment
            lastTime = cue.timestamp;

            screenImage.sprite = cue.image; // Set image
            screenImage.canvasRenderer.SetAlpha(0f); // Make invisible
            screenImage.CrossFadeAlpha(1f, fadeDuration, false);

            Debug.Log($"Showing image {i} at {cue.timestamp}s");
        }
    }

    private IEnumerator PlaySubtitleCues()   // Displays each subtitle at the correct time
    {
        float lastTime = 0f;

        for (int i = 0; i < subtitleCues.Count; i++)
        {
            var cue = subtitleCues[i];
            float wait = cue.timestamp - lastTime;
            if (wait > 0f) yield return new WaitForSeconds(wait); // Wait until the cue’s time
            lastTime = cue.timestamp;

            subtitleText.text = cue.subtitle;  // Show subtitle

            Debug.Log($"Subtitle {i} at {cue.timestamp}s: {cue.subtitle}");
        }

        yield return new WaitForSeconds(6f); // Let the last subtitle linger for 6 seconds before clearing
        subtitleText.text = "";
    }
}
