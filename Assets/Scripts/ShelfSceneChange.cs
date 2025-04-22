using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShelfSceneChange : MonoBehaviour
{
    [SerializeField] private string bookTag = "Book";
    [SerializeField] private string mainSceneName = "Interaction Scene";

    private bool isChangingScene = false;
    private bool bookIsInShelf = true;

    private void OnTriggerExit(Collider other)
    {
        if (isChangingScene || !other.CompareTag(bookTag)) return;

        if (bookIsInShelf)
        {
            bookIsInShelf = false;
            isChangingScene = true;

            var bookSceneRef = other.GetComponent<BookSceneReference>();
            if (bookSceneRef == null)
            {
                Debug.LogWarning("Book is missing a BookSceneReference component!");
                return;
            }

            string targetScene = bookSceneRef.targetSceneName;

            Debug.Log("Book exited shelf — loading scene: " + targetScene);
            PreparePersistentObjects(other.gameObject);
            SceneManager.LoadScene(targetScene);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isChangingScene || !other.CompareTag(bookTag)) return;

        if (!bookIsInShelf)
        {
            bookIsInShelf = true;
            isChangingScene = true;

            Debug.Log("Book returned to shelf — returning to main scene");
            PreparePersistentObjects(other.gameObject);
            SceneManager.LoadScene(mainSceneName);
        }
    }

    private void PreparePersistentObjects(GameObject book)
    {
        DontDestroyOnLoad(book);

        GameObject bookshelf = GameObject.Find("bookshelf");
        if (bookshelf != null) DontDestroyOnLoad(bookshelf);

        // Uncomment if needed:
        // GameObject xrRig = GameObject.Find("XR Origin (XR Rig)");
        // if (xrRig != null) DontDestroyOnLoad(xrRig);
    }
}