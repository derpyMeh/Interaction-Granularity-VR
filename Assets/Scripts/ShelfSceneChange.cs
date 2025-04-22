using UnityEngine;
using UnityEngine.SceneManagement;

public class ShelfSceneChange : MonoBehaviour
{
    [SerializeField] private string bookTag = "Book";
    [SerializeField] private string mainSceneName = "Interaction Scene";

    private static bool justSwitchedScene = false; // survives across scene reload
    private static float lastSwitchTime = -999f;
    private const float cooldown = 1.5f;

    private GameObject lastBookEntered;
    private float sceneStartTime;
    private float triggerEnterDelay = 1f; // or tweak this as needed

    private void Start()
{
    sceneStartTime = Time.time;
}

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(bookTag)) return;
        if (justSwitchedScene && Time.time - lastSwitchTime < cooldown) return;

        var bookSceneRef = other.GetComponent<BookSceneReference>();
        if (bookSceneRef == null) return;

        justSwitchedScene = true;
        lastSwitchTime = Time.time;

        Debug.Log("Book exited shelf — loading scene: " + bookSceneRef.targetSceneName);
        PreparePersistentObjects(other.gameObject);
        SceneManager.LoadScene(bookSceneRef.targetSceneName);
    }

    private void OnTriggerEnter(Collider other)
    {
    if (!other.CompareTag(bookTag)) return;

    if (BookGrabTracker.currentlyHeldBook != other.gameObject)
    {
        Debug.Log("Book entered, but it's not the active/held one.");
        return;
    }

    if (Time.time - sceneStartTime < triggerEnterDelay)
    {
        Debug.Log("Entered too early after scene load");
        return;
    }

    if (justSwitchedScene && Time.time - lastSwitchTime < cooldown)
    {
        Debug.Log("Cooldown still active");
        return;
    }

    justSwitchedScene = true;
    lastSwitchTime = Time.time;

    Debug.Log("Book returned — loading main scene");
    PreparePersistentObjects(other.gameObject);
    SceneManager.LoadScene(mainSceneName);
    }

    private void PreparePersistentObjects(GameObject book)
    {
        DontDestroyOnLoad(book);

        GameObject bookshelf = GameObject.Find("bookshelf");
        if (bookshelf != null) DontDestroyOnLoad(bookshelf);

        // Optional:
        // GameObject xrRig = GameObject.Find("XR Origin (XR Rig)");
        // if (xrRig != null) DontDestroyOnLoad(xrRig);
    }
}
