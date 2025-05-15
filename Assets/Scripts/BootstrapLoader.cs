using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapLoader : MonoBehaviour
{
    public string interactionSceneName;

    private void Awake()
    {
        // Load Interaction Scene if not already loaded
        if (!SceneManager.GetSceneByName(interactionSceneName).isLoaded)
        {
            Debug.Log($"Bootstrapping scene: {interactionSceneName}");
            SceneManager.LoadSceneAsync(interactionSceneName, LoadSceneMode.Additive);

        }
    }
}
