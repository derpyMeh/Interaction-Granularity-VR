using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class NewBookLogic : MonoBehaviour
{
    public GameObject spawnPos;
    public GameObject playPos;
    private bool inIngWorld = false;
    private bool hasTriggered = false;
    private XRGrabInteractable grabbedInteract;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Attempt to find the XR Rig in the scene by its name
        GameObject target = GameObject.Find("XR Origin (XR Rig)");

        //If the XR Rig was found, assign it to the playPos variable
        if (target != null)
        {
            playPos = target;
        }

        //Get the XRGrabInteractable component attached to this GameObject
        grabbedInteract = GetComponent<XRGrabInteractable>();

        //If the component exists, subscribe to the selectEntered event
        //This ensures the onBookPickup method is called when the object is grabbed
        if (grabbedInteract != null)
        {
            grabbedInteract.selectEntered.AddListener(onBookPickup);
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Check if the current scene is NOT "Forge 2"
        if (SceneManager.GetActiveScene().name != "Forge 2")
        {
            //If not in IngWorld, set the players position to the spawn position
            if (!inIngWorld)
            {
                playPos.transform.position = spawnPos.transform.position;
                inIngWorld = true;
            }
        }
        else
        {
            //Reset the trigger flag when we’re back in "Forge 2"
            inIngWorld = false;
        }

        //Check if the current scene is NOT "Ingredients world"
        if (SceneManager.GetActiveScene().name != "Ingredients world")
        {
            //If hasTriggered is not true, set the player position to the spawn position
            if (!hasTriggered)
            {
                playPos.transform.position = spawnPos.transform.position;
                hasTriggered = true;
            }
        }
        else
        {
            //Reset the triggerflag once back in Ingredients World
            hasTriggered = false;
        }
    }
        


    private void onBookPickup(SelectEnterEventArgs args)
    {
        //Disable interaction on the doorhandle object to prevent it from being picked up again
        grabbedInteract.interactionLayers = 0;

        //Check if the current scene is forge 2
        if (SceneManager.GetActiveScene().name == "Forge 2")
        {

            //If in "Forge 2", fade and transition to "Ingredients world"
            CameraEffectController cam = FindObjectOfType<CameraEffectController>();
            if (cam != null)
            {
                cam.FadeToScene("Ingredients world", 2f);
            }

            //Mark Entering ingredient world scene
            inIngWorld = true;
            hasTriggered = false;
        }
        else
        {
            //If in any other scene, fade then transition to "Forge 2"
            CameraEffectController cam = FindObjectOfType<CameraEffectController>();
            if (cam != null)
            {
                cam.FadeToScene("Forge 2", 2f);
            }

            //Mark leaving ingredients world scene
            inIngWorld = false;
            hasTriggered = false;
        }
    }


}
