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
    public CanvasGroup screenCanvasGroup;
    public Image screenImage;
    public TextMeshProUGUI subtitleText;
    public AudioSource voiceoverSource;
    public GameObject orb;

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

    [Header("Fade & Timing Settings")]
    public float fadeDuration = 1.5f;
    public float cutsceneDelay = 5f;

    [Header("Orb & Scene Transition")]
    public GameObject orbPrefab;
    public Transform orbSpawnPoint;

    [Tooltip("Scene for Level 1")]
    public SceneAsset level1Scene;
    [Tooltip("Scene for Level 2")]
    public SceneAsset level2Scene;
    [Tooltip("Scene for Level 3")]
    public SceneAsset level3Scene;

    public enum FirstLevelToPlay
    {
        Level1,
        Level2,
        Level3
    }

    [Header("First level to play:")]
    public FirstLevelToPlay firstLevel = FirstLevelToPlay.Level1;

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

        SpawnSceneOrb();
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

        float endTime = voiceoverSource.clip.length;
        float lastSubtitleTime = subtitleCues[^1].timestamp;
        float remaining = Mathf.Max(endTime - lastSubtitleTime, 2f);
        yield return new WaitForSeconds(remaining);

        subtitleText.text = "";
    }

    private void SpawnSceneOrb()
    {
        if (orbPrefab == null || orbSpawnPoint == null)
        {
            Debug.LogWarning("Orb prefab or spawn point not set.");
            return;
        }

        orb.SetActive(true);
        var behavior = orb.GetComponent<SceneOrbBehavior>();
        if (behavior != null)
        {
            behavior.controller = this;
        }
    }

    public string GetSelectedSceneName()
    {
#if UNITY_EDITOR
        SceneAsset selectedScene = firstLevel switch
        {
            FirstLevelToPlay.Level1 => level1Scene,
            FirstLevelToPlay.Level2 => level2Scene,
            FirstLevelToPlay.Level3 => level3Scene,
            _ => level1Scene
        };

        if (selectedScene == null)
        {
            Debug.LogWarning("Selected SceneAsset is null.");
            return "";
        }

        string path = AssetDatabase.GetAssetPath(selectedScene);
        string sceneName = System.IO.Path.GetFileNameWithoutExtension(path);
        return sceneName;
#else
        Debug.LogWarning("SceneAsset references only work in the editor. Please assign scene name strings for builds.");
        return "";
#endif
    }

    public void FadeAndLoadScene(string sceneName)
    {
        StartCoroutine(FadeOutAndLoad(sceneName));
    }

    private IEnumerator FadeOutAndLoad(string sceneName)
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            screenCanvasGroup.alpha = Mathf.Lerp(1f, 0f, timer / fadeDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        screenCanvasGroup.alpha = 0f;

        if (!string.IsNullOrEmpty(sceneName))
            SceneManager.LoadScene(sceneName);
    }
}
