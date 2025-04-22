using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;


public class SceneChanger : MonoBehaviour
{

 [SerializeField] private string targetSceneName = "Test"; // Destination scene
    private bool hasChangedScene = false;

    private void OnEnable()
    {
        GetComponent<XRGrabInteractable>().selectEntered.AddListener(OnGrabbed);
    }

    private void OnDisable()
    {
        GetComponent<XRGrabInteractable>().selectEntered.RemoveListener(OnGrabbed);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        if (hasChangedScene) return;
        hasChangedScene = true;

        GameObject xrRig = GameObject.Find("XR Origin (XR Rig)");
        GameObject bookshelf = GameObject.Find("bookshelf");

        // Persist important objects across scenes
        DontDestroyOnLoad(gameObject); // the book
        if (xrRig != null) DontDestroyOnLoad(xrRig);
        if (bookshelf != null) DontDestroyOnLoad(bookshelf);

        // Switch to Test scene
        SceneManager.LoadScene(targetSceneName);
    }
}
