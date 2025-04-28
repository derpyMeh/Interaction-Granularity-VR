using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using System.Collections;

public class BookSceneController : MonoBehaviour
{
    [SerializeField] private string interactionSceneName = "Interaction Scene";
    List<string> loadedScenes = new List<string>();

    //[SerializeField] private string mainSceneName = "Main";

    private string currentBookScene = "";

    public void LoadBookScene(string bookSceneName)
    {
        StartCoroutine(SwitchToBookScene(bookSceneName));
    }

    public void ReturnToInteractionScene(string bookSceneName)
    {
        if (!string.IsNullOrEmpty(bookSceneName))
        {
            if (loadedScenes.Contains(bookSceneName))
            {
                Debug.Log($"Unloading book scene: {bookSceneName}");
                SceneManager.UnloadSceneAsync(bookSceneName);
                loadedScenes.Remove(bookSceneName);

                if (currentBookScene == bookSceneName)
                {
                    currentBookScene = "";
                }
            }

        }

        if (!SceneManager.GetSceneByName(interactionSceneName).isLoaded)
        {
            Debug.Log("Reloading Interaction Scene...");
            SceneManager.LoadSceneAsync(interactionSceneName, LoadSceneMode.Additive);
        }
    }


    private IEnumerator SwitchToBookScene(string newBookScene)
    {
        Debug.Log("Loading book scene: " + newBookScene);

        if (SceneManager.GetSceneByName(interactionSceneName).isLoaded)
        {
            SceneManager.UnloadSceneAsync(interactionSceneName);
        }

        AsyncOperation op = SceneManager.LoadSceneAsync(newBookScene, LoadSceneMode.Additive);
        while (!op.isDone)
            yield return null;

        Scene loadedScene = SceneManager.GetSceneByName(newBookScene);
        if (loadedScene.IsValid())
        {
            SceneManager.SetActiveScene(loadedScene);
            currentBookScene = newBookScene;

            if (!loadedScenes.Contains(newBookScene))
            {
                loadedScenes.Add(newBookScene); // Save the newly loaded scene
            }

        }
    }
}
