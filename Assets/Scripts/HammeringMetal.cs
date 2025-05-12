using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;


public class HammeringMetal : MonoBehaviour
{
    private string hammerTag = "Hammer";
    private string ingotTag = "Ingot";

    private Rigidbody hammerRB;

    private bool hammerInside = false;
    private bool ingotPlaced = false;

    public GameObject chainObj;
    public GameObject ingotObj;
    public XRBaseInteractor interactor;
    [SerializeField] GameObject hammerObj;

    public Transform slotPosition;
    private Vector3 lastPosition;
    private float velocityThreshold = 5.0f;
    private float lastSwingTime = 0f;
    private float swingCd = 1.5f;

    // Start is called before the first frame update
    void Start()
    {
        lastPosition = hammerObj.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        Vector3 deltaPosition = hammerObj.transform.position - lastPosition;

        // Calculate the velocity (change in position over time)
        float velocity = deltaPosition.magnitude / Time.deltaTime;

        // Update the last position for the next frame
        lastPosition = hammerObj.transform.position;

        // Only trigger if velocity exceeds threshold
        if (velocity >= velocityThreshold)
        {
            ForgeTest();
            // Hammer is swinging with sufficient velocity
            Debug.Log("Swing detected with velocity: " + velocity);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(ingotTag) && other.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>() != null)
        {
            PlaceInSlot(other.transform);
            Debug.Log("placed slot");
        }
        if (other.CompareTag(hammerTag))
        {
            hammerInside = true;
        }
        if (other.CompareTag(ingotTag))
        {
            ingotPlaced = true;
            Debug.Log("Ingot Placed");
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(hammerTag))
        {
            hammerInside = false;
        }

        if (other.CompareTag(ingotTag))
        {
            ingotPlaced = false;
        }
    }

    private void PlaceInSlot(Transform objectTransform)
    {
        // Optionally disable the physics and interactions for the object when placed
        UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable grabInteractable = objectTransform.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>();
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
    private void ForgeTest()
    {
        Vector3 controllerVelocity = interactor.GetComponent<Rigidbody>().linearVelocity;
        Debug.Log(controllerVelocity.magnitude);

        if (Time.time - lastSwingTime > swingCd && ingotPlaced && hammerInside)
        {
            chainObj.SetActive(true);
            Destroy(ingotObj);
            Debug.Log("Chain Spawned");
        }

    }
}
