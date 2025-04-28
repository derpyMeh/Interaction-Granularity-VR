using UnityEngine;

public class ShelfSceneChange : MonoBehaviour
{
    [SerializeField] private string bookTag = "Book";

    private float sceneStartTime;
    private float triggerEnterDelay = 1f;
    private static float lastSwitchTime = -999f;
    private const float cooldown = 1.5f;


    private void Start()
    {
        sceneStartTime = Time.time;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag(bookTag)) return;

        // Only proceed if the book is currently held
        if (BookGrabTracker.currentlyHeldBook != other.gameObject)
        {
            Debug.Log("Book exited, but it's not being held — ignoring.");
            return;
        }

        if (Time.time - lastSwitchTime < cooldown) return;

        var bookSceneRef = other.GetComponent<BookSceneReference>();
        var bookID = other.GetComponent<BookID>();
        if (bookSceneRef == null || bookID == null) return;

        lastSwitchTime = Time.time;

        Debug.Log($"Book '{bookID.bookID}' exited shelf — loading scene: {bookSceneRef.targetSceneName}");

        // When book is picked up and scene is about to change
        BookVisibilityManager.Instance?.ShowOnlyBook(bookID.bookID);

        // Load the corresponding book scene
        var controller = FindObjectOfType<BookSceneController>();
        controller?.LoadBookScene(bookSceneRef.targetSceneName);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Time.time - sceneStartTime < triggerEnterDelay) return;
        if (!other.CompareTag(bookTag)) return;

        var bookSceneRef = other.GetComponent<BookSceneReference>();
        // Only switch back if it's the currently held book returning
        if (BookGrabTracker.currentlyHeldBook != other.gameObject)
        {
            Debug.Log("Book entered, but it's not the active/held one — ignoring.");
            return;
        }

        if (Time.time - lastSwitchTime < cooldown)
        {
            Debug.Log("Cooldown still active — skipping return.");
            return;
        }

        lastSwitchTime = Time.time;

        Debug.Log("Book returned to shelf — unloading book scene");

        // When returning to interaction scene
        BookVisibilityManager.Instance?.ShowAllBooks();

        ///Reload the Interaction Scene
        var controller = FindObjectOfType<BookSceneController>();
        controller?.ReturnToInteractionScene(bookSceneRef.targetSceneName);
    }
}
