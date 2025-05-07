using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BookGrabTracker : MonoBehaviour
{
  public static GameObject currentlyHeldBook = null;

    private UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grab;

    private void Awake()
    {
        grab = GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
    }

    private void OnEnable()
    {
        grab.selectEntered.AddListener(OnGrabbed);
        grab.selectExited.AddListener(OnReleased);
    }

    private void OnDisable()
    {
        grab.selectEntered.RemoveListener(OnGrabbed);
        grab.selectExited.RemoveListener(OnReleased);
    }

    private void OnGrabbed(SelectEnterEventArgs args)
    {
        currentlyHeldBook = this.gameObject;

        // Notify manager
        BookShelfGroupManager.Instance?.OnBookGrabbed(this.gameObject);
    }

    private void OnReleased(SelectExitEventArgs args)
    {
        currentlyHeldBook = null;

        // Notify manager
        BookShelfGroupManager.Instance?.OnBookReleased(this.gameObject);
    }

}
