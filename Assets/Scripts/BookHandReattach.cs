using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BookHandReattach : MonoBehaviour
{
    private XRGrabInteractable grab;
    private IXRSelectInteractor previousInteractor;

    void Awake()
    {
        grab = GetComponent<XRGrabInteractable>();
        grab.selectEntered.AddListener(OnGrabbed);
        grab.selectExited.AddListener(OnReleased);
    }

    private void OnDestroy()
    {
        grab.selectEntered.RemoveListener(OnGrabbed);
        grab.selectExited.RemoveListener(OnReleased);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        previousInteractor = args.interactorObject;
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        previousInteractor = null;
    }

    void OnEnable()
    {
        if (previousInteractor != null)
        {
            // Wait a moment after scene loads before trying to reattach
            StartCoroutine(ReattachToHand());
        }
    }

    private IEnumerator ReattachToHand()
    {
        yield return new WaitForSeconds(0.2f); // Wait for scene to finish loading

        if (grab != null && previousInteractor != null)
        {
            Debug.Log("Reattaching book to hand...");
            grab.interactionManager.SelectEnter(previousInteractor, grab);
        }
    }
}
