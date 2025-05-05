using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SlotLogic : MonoBehaviour
{
    public Transform slotPosition; // The position where the object should be placed
    public string ingotTag = "Ingot"; // Tag to check if the object is allowed to be placed

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object has the correct tag and is interactable
        if (other.CompareTag(ingotTag) && other.GetComponent<XRGrabInteractable>() != null)
        {
            PlaceInSlot(other.transform);
            Debug.Log("placed slot");
        }
    }

    private void PlaceInSlot(Transform objectTransform)
    {
        // Optionally disable the physics and interactions for the object when placed
        XRGrabInteractable grabInteractable = objectTransform.GetComponent<XRGrabInteractable>();
        Rigidbody objectRigidbody = objectTransform.GetComponent<Rigidbody>();

        // Stop the object from moving freely once placed in the slot
        if (objectRigidbody != null)
        {
            objectRigidbody.isKinematic = true; // Disable physics
        }

        // Snap the object into the slot position
        objectTransform.position = slotPosition.position;
        objectTransform.rotation = slotPosition.rotation; // Optional: align rotation as well

        // Optionally disable interaction after placement
        if (grabInteractable != null)
        {
            // Set the interaction layers to nothing (disable interaction)
            grabInteractable.interactionLayers = 0;
        } // Disable interaction
    }
}
