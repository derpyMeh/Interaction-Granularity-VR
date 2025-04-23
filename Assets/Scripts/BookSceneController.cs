using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BookSceneController : MonoBehaviour
{
    [SerializeField] private string interactionSceneName = "Interaction Scene";
    private string currentBookScene = "";

    public void LoadBookScene(string bookSceneName)
    {
        StartCoroutine(SwitchToBookScene(bookSceneName));
    }

    public void ReturnToInteractionScene()
    {
        if (!string.IsNullOrEmpty(currentBookScene))
        {
            SceneManager.UnloadSceneAsync(currentBookScene);
            currentBookScene = "";
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
        }
    }
}
