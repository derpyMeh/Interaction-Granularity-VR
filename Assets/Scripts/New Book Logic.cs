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
        GameObject target = GameObject.Find("XR Origin (XR Rig)");

        if (target != null)
        {
            playPos = target;
        }

            grabbedInteract = GetComponent<XRGrabInteractable>();
        if (grabbedInteract != null)
        {
            grabbedInteract.selectEntered.AddListener(onBookPickup);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "Forge 2")
        {
            if (!inIngWorld)
            {
                Debug.Log("Has Triggered");
                playPos.transform.position = spawnPos.transform.position;
                inIngWorld = true;
            }
        }
        else
        {
            inIngWorld = false;
        }
        if (SceneManager.GetActiveScene().name != "Ingredients world")
        {
            if (!hasTriggered)
            {
                Debug.Log("Has Triggered");
                playPos.transform.position = spawnPos.transform.position;
                hasTriggered = true;
            }
        }
        else
        {
            hasTriggered = false;
        }
    }
        

    private void onBookPickup(SelectEnterEventArgs args)
    {
        if (SceneManager.GetActiveScene().name == "Forge 2")
        {

            CameraEffectController cam = FindObjectOfType<CameraEffectController>();
            if (cam != null)
            {
                cam.FadeToScene("Ingredients world", 2f);
            }
            inIngWorld = true;
            hasTriggered = false;
        }
        else
        {
            CameraEffectController cam = FindObjectOfType<CameraEffectController>();
            if (cam != null)
            {
                cam.FadeToScene("Forge 2", 2f);
            }
            inIngWorld = false;
            hasTriggered = false;
        }
    }


}
