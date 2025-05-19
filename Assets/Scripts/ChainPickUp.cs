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
        CameraEffectController cam = FindObjectOfType<CameraEffectController>();
        if (cam != null)
        {
            Debug.Log("Beginning Fade");
            cam.FadeToScene("OutroCutscene", 5f);
        }

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

