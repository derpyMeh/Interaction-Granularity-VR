using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;

public class ChainPickUp : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public XRGrabInteractable chainInteractable;

    void Start()
    {
        if (chainInteractable != null)
        {
            chainInteractable.selectEntered.AddListener(OnChainPickup);
        }
    }

    private void OnChainPickup(SelectEnterEventArgs args)
    {
        SceneManager.LoadScene("OutroCutscene");
    }

    private void OnDestroy()
    {
        // Always clean up your listeners!
        if (chainInteractable != null)
        {
            chainInteractable.selectEntered.RemoveListener(OnChainPickup);
        }
    }
}

