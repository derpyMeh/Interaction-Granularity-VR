using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;


public class HammeringMetal : MonoBehaviour
{
    private string hammerTag = "Hammer";
    private string ingotTag = "Ingot";

    private Rigidbody hammerRB;

    private bool hammerInside = false;
    private bool ingotPlaced = false;

    public GameObject chainObj;
    public GameObject ingotObj;
    public XRGrabInteractable interactor;
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

        // Only call ForgeTest method if velocity exceeds threshold and both the ingot and hammer are inside the collider (IngotPlaced and HammerInside are true)
        if (velocity >= velocityThreshold && ingotPlaced && hammerInside)
        {
            ForgeTest();
        }

       
    }


    //
    private void OnTriggerEnter(Collider other)
    {
        // If the object has the ingot tag and is an XR Grab Interactable, place it in the slot
        if (other.CompareTag(ingotTag) && other.GetComponent<XRGrabInteractable>() != null)
        {
            PlaceInSlot(other.transform);
        }

        // If the object is tagged as a hammer, mark that the hammer is inside the trigger zone
        if (other.CompareTag(hammerTag))
        {
            hammerInside = true;
        }

        // If the object has the Ingot tag, mark it as placed
        if (other.CompareTag(ingotTag))
        {
            ingotPlaced = true;
        }
    }

    
    //Similar to previous just reversed for removal of the objects 
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
        //Disable the physics and interactions for the object when placed
        XRGrabInteractable grabInteractable = objectTransform.GetComponent<XRGrabInteractable>();
        Rigidbody objectRigidbody = objectTransform.GetComponent<Rigidbody>();

        // Stop the object from moving freely once placed in the slot
        if (objectRigidbody != null)
        {
            objectRigidbody.isKinematic = true; // Disable physics
        }

        // Snap the object into the slot position
        objectTransform.position = slotPosition.position;

      
    }
    private void ForgeTest()
    {
        //If ingot and hammer are inside, set the chain object as active and destroy the ingot object
        if (ingotPlaced && hammerInside)
        {
            chainObj.SetActive(true);
            Destroy(ingotObj);
        }

    }
}
