using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class introCutsceneController : MonoBehaviour
{
    [Header("UI References")]
    public CanvasGroup screenCanvasGroup; // Controls UI fade in/out
    public Image screenImage; // Displays cutscene images
    public TextMeshProUGUI subtitleText; // Subtitle text
    public AudioSource voiceoverSource; // Audio source for voiceover
    public GameObject orb; // Object spawned at end of cutscene

    // Struct to pair an image with a specific time in the audio
    [System.Serializable]
    public class ImageCue
    {
        public float timestamp;
        public Sprite image;
    }

    // Struct to pair a subtitle line with a specific time
    [System.Serializable]
    public class SubtitleCue
    {
        public float timestamp;
        [TextArea] public string subtitle;
    }

    // Lists of cues for cutscene
    [Header("Cutscene Data")]
    public List<ImageCue> imageCues = new List<ImageCue>();
    public List<SubtitleCue> subtitleCues = new List<SubtitleCue>();

    // Fade and timing
    [Header("Fade & Timing Settings")]
    public float fadeDuration = 1.5f;
    public float cutsceneDelay = 5f;

    // Orb spawn and scene transition
    [Header("Orb & Scene Transition")]
    public GameObject orbPrefab;
    public Transform orbSpawnPoint;

    // Next scene choices for cutscene outcome
    [Tooltip("Scene for Level 1")]
    public SceneAsset level1Scene;
    [Tooltip("Scene for Level 2")]
    public SceneAsset level2Scene;
    [Tooltip("Scene for Level 3")]
    public SceneAsset level3Scene;

    public enum FirstLevelToPlay   // Which level to load after cutscene
    {
        Level1,
        Level2,
        Level3
    }

    [Header("First level to play:")]
    public FirstLevelToPlay firstLevel = FirstLevelToPlay.Level1;

    private void Start()
    {
        screenCanvasGroup.alpha = 0f;  // Ensure screen starts invisible
        StartCoroutine(DelayedCutsceneStart()); // Begin delayed start
    }

    private IEnumerator DelayedCutsceneStart()
    {
        yield return new WaitForSeconds(cutsceneDelay); // Wait before starting

        subtitleText.text = "";

        if (imageCues.Count > 0)
        {
            screenImage.sprite = imageCues[0].image;
            screenImage.canvasRenderer.SetAlpha(0f);
        }

        yield return StartCoroutine(FadeInScreen());  // Fade screen in

        StartCoroutine(PlayCutscene()); // Begin playing cues
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
        voiceoverSource.Play(); // Start voiceover

        StartCoroutine(PlayImageCues()); // Play images
        StartCoroutine(PlaySubtitleCues()); // Play subtitles
         
        yield return new WaitForSeconds(voiceoverSource.clip.length + 2f); // Wait for voice
        subtitleText.text = "";
        screenImage.CrossFadeAlpha(0f, fadeDuration, false); // Fade out image
        yield return new WaitForSeconds(fadeDuration);
        screenImage.sprite = null;

        SpawnSceneOrb(); // Activate transition orb
    }

    private IEnumerator PlayImageCues()
    {
        float lastTime = 0f; // Tracks time of the last cue to calculate wait time

        for (int i = 0; i < imageCues.Count; i++)
        {
            var cue = imageCues[i]; // Get the current image cue
            float wait = cue.timestamp - lastTime; // Calculate how long to wait before showing this cue
            if (wait > 0f) yield return new WaitForSeconds(wait); // Wait for the right time to show the image
            lastTime = cue.timestamp; // Update the last cue time

            screenImage.sprite = cue.image; // Set the new image
            screenImage.canvasRenderer.SetAlpha(0f); // Immediately make it invisible
            screenImage.CrossFadeAlpha(1f, fadeDuration, false); // Fade it in over `fadeDuration` seconds
        }
    }

    private IEnumerator PlaySubtitleCues()
    {
        float lastTime = 0f;

        for (int i = 0; i < subtitleCues.Count; i++)
        {
            var cue = subtitleCues[i];
            float wait = cue.timestamp - lastTime;
            if (wait > 0f) yield return new WaitForSeconds(wait);  // Wait for calculated time
            lastTime = cue.timestamp;

            subtitleText.text = cue.subtitle; // Display the subtitle
        }

        float endTime = voiceoverSource.clip.length; // Total length of the voiceover audio
        float lastSubtitleTime = subtitleCues[^1].timestamp; // Timestamp of the last subtitle
        float remaining = Mathf.Max(endTime - lastSubtitleTime, 2f); // Wait until voiceover ends, minimum 2 sec
        yield return new WaitForSeconds(remaining); // Let subtitle linger for remaining time

        subtitleText.text = ""; // Clear subtitle text
    }

    private void SpawnSceneOrb()
    {
        if (orbPrefab == null || orbSpawnPoint == null) // Check for null references before trying to spawn the orb
        {
            Debug.LogWarning("Orb prefab or spawn point not set.");
            return; // Exit early if we can't spawn the orb
        }

        orb.SetActive(true); // Activate the orb object in the scene

        var behavior = orb.GetComponent<SceneOrbBehavior>(); // Try to get the SceneOrbBehavior component from the orb GameObject
        if (behavior != null)
        {
            behavior.controller = this;
        }
    }

    public string GetSelectedSceneName()
    {
#if UNITY_EDITOR 
        SceneAsset selectedScene = firstLevel switch // Select the SceneAsset object based on the enum selection
        {
            FirstLevelToPlay.Level1 => level1Scene,
            FirstLevelToPlay.Level2 => level2Scene,
            FirstLevelToPlay.Level3 => level3Scene,
            _ => level1Scene // Default fallback
        };

        if (selectedScene == null) // Handle null case if no scene is assigned
        {
            Debug.LogWarning("Selected SceneAsset is null.");
            return "";
        }

        string path = AssetDatabase.GetAssetPath(selectedScene); // Get the file path of the selected SceneAsset from the editor asset database
        string sceneName = System.IO.Path.GetFileNameWithoutExtension(path); // Extract the file name (scene name) without the file extension
        return sceneName;
#else
        Debug.LogWarning("SceneAsset references only work in the editor. Please assign scene name strings for builds.");
        return "";
#endif
    }

    public void FadeAndLoadScene(string sceneName)
    {
        StartCoroutine(FadeOutAndLoad(sceneName)); // Begin the fade out and scene loading process
}

    private IEnumerator FadeOutAndLoad(string sceneName)  // Fade out the screen by gradually lowering the CanvasGroup alpha
    {
        float timer = 0f; 

        while (timer < fadeDuration)
        {
            screenCanvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        screenCanvasGroup.alpha = 0f; // Ensure it's fully invisible at the end

        if (!string.IsNullOrEmpty(sceneName))  // If a valid scene name is given, load the scene
            SceneManager.LoadScene(sceneName);// Switch to the new scene
    }
}
