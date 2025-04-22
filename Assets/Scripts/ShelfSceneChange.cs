using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShelfSceneChange : MonoBehaviour
{
    [SerializeField] private string bookTag = "Book";
    [SerializeField] private string mainSceneName = "Interaction Scene";
    [SerializeField] private string bookSceneName = "Test";
    private bool isChangingScene = false;

    private void OnTriggerExit(Collider other)
    {
        if (isChangingScene || !other.CompareTag(bookTag)) return;

        isChangingScene = true;
        Debug.Log("Book removed from shelf — loading book scene!");

        PreparePersistentObjects(other.gameObject);
        SceneManager.LoadScene(bookSceneName);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isChangingScene || !other.CompareTag(bookTag)) return;

        isChangingScene = true;
        Debug.Log("Book returned to shelf — going back to main scene!");

        PreparePersistentObjects(other.gameObject);
        SceneManager.LoadScene(mainSceneName);
    }

    private void PreparePersistentObjects(GameObject book)
    {
        DontDestroyOnLoad(book);

        GameObject xrRig = GameObject.Find("XR Origin (XR Rig)");
        GameObject bookshelf = GameObject.Find("bookshelf");

        if (xrRig != null) DontDestroyOnLoad(xrRig);
        if (bookshelf != null) DontDestroyOnLoad(bookshelf);
    }
}
