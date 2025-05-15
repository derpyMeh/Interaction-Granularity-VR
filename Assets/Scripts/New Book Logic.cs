using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class NewBookLogic : MonoBehaviour
{
    private XRGrabInteractable grabbedInteract;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        grabbedInteract = GetComponent<XRGrabInteractable>();
        if (grabbedInteract != null)
        {
            grabbedInteract.selectEntered.AddListener(onBookPickup);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void onBookPickup(SelectEnterEventArgs args)
    {
        if (SceneManager.GetActiveScene().name == "Forge 2")
        {
            SceneManager.LoadScene("Ingredients world");
        }
        else
        {
            SceneManager.LoadScene("Forge 2");
        }
    }

    private void OnEnable()
    {

       
    }
}
