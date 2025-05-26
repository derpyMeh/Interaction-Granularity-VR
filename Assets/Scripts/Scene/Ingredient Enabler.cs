using Mono.Cecil;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactors;

public class IngredientEnabler : MonoBehaviour
{
    public XRDirectInteractor Interactor;
    public GameObject xrOrig;
    public GameObject xrIntMan;
    public List<string> ingredientListNames = new List<string>();
    private List<GameObject> allIngredientList = new List<GameObject>();
    public GameObject poofObj;
    public ParticleSystem poofParticle;



    void Awake()
    {
        //Ensures several important objects arent destroyed when a new scene loads. 
        //Runs on Awake instead of start since bootStrap runs on Awake.
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(xrOrig);
        DontDestroyOnLoad(xrIntMan);
        DontDestroyOnLoad(poofObj);


    }

    // Update is called once per frame
    void Update()
    {
        //Only run this logic if:
        //The current scene is "Forge 2"
        //There are ingredients stored in the ingredient list
        //Fewer than or equal to 6 ingredients have been added to the allIngredientList
        if (SceneManager.GetActiveScene().name == "Forge 2" && ingredientListNames.Count > 0 && allIngredientList.Count <= 6)
        {
            // Get the current scene reference
            Scene currentScene = SceneManager.GetActiveScene();

            // Get ALL GameObjects in the project, including inactive ones
            GameObject[] allIngredients = Resources.FindObjectsOfTypeAll<GameObject>();

            //Filter the GameObjects:
            //Must belong to the current scene
            //Must be tagged as "Ingredient"
            //If they are, add them to the ingredient list  
            foreach (GameObject go in allIngredients)
            {
                if (go.scene == currentScene && go.CompareTag("Ingredient"))
                {
                    allIngredientList.Add(go);
                }
            }

            //Clear the allIngredients array manually to free up memory
            for (int i = 0; i < allIngredients.Length; i++)
            {
                allIngredients[i] = null;
            }

            //Loop through ingredients that belong to the scene
            foreach (GameObject obj in allIngredientList)
            {
                //If the object is in the saved ingredient list and is currently inactive, activate it
                if (ingredientListNames.Contains(obj.name) && !obj.activeSelf)
                {
                    obj.SetActive(true);
                }
            }
        }
    }


    private void OnEnable()
    {
        //Subscribe to the selectEntered event so that OnGrab is called when an object is grabbed
        //This will call OnGrab() whenever an interactable is grabbed
        Interactor.selectEntered.AddListener(OnGrab);
    }

    private void OnDisable()
    {
        //Unsubscribe from the selectEntered event to avoid memory leaks or unexpected behavior
        Interactor.selectEntered.RemoveListener(OnGrab);
    }

    //Called when an interactable object is grabbed
    private void OnGrab(SelectEnterEventArgs args)
    {
        //Get the GameObject that was grabbed
        GameObject grabbedObj = args.interactableObject.transform.gameObject;

        //Check if the grabbed object is tagged as "Ingredient" and currently in the correct scene
        if (grabbedObj.CompareTag("Ingredient") && SceneManager.GetActiveScene().name == "Ingredients world")
        {
            //Check if the object hasn't already been added to the list
            if (!ingredientListNames.Contains(grabbedObj.name))
            {
                //Move the particle effect to the grabbed object's position and play it
                poofObj.transform.position = grabbedObj.transform.position;
                poofParticle.Play();

                //Add the ingredient's name to the tracking list
                ingredientListNames.Add(grabbedObj.name);

                // Deactivate the object after picking it up (if it's still active)
                if (!grabbedObj.activeInHierarchy == false)
                {   
                    grabbedObj.SetActive(false);
                }
            }
        }
    }
}
