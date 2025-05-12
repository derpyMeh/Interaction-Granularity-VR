using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapLoader : MonoBehaviour
{
    [SerializeField] private string interactionSceneName = "Forge";

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
