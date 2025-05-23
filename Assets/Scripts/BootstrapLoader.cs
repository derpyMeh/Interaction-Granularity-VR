using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapLoader : MonoBehaviour
{
    public string interactionSceneName; // Name of the scene to ensure is loaded

    private void Awake()
    {
        if (!SceneManager.GetSceneByName(interactionSceneName).isLoaded)  // Check if the specified scene is already loaded 
        {
            Debug.Log($"Bootstrapping scene: {interactionSceneName}");
            SceneManager.LoadScene(interactionSceneName); // Load the interaction scene immediately (synchronously, replacing current scene)

        }
        
    }
}
