using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit;

public class ChainPickUp : MonoBehaviour
{
    public XRGrabInteractable chainInteractable;


    void Start()
    {

        // Add an event listener so that when it's picked up (selected), onChainPickup is called
        if (chainInteractable != null)
        {
            chainInteractable.selectEntered.AddListener(OnChainPickup);
        }
    }

    private void OnChainPickup(SelectEnterEventArgs args)
    {
        //When chain is pickeded up, begins fading out and load into outroscene
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

