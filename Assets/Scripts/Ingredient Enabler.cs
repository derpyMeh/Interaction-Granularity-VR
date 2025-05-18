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



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(xrOrig);
        DontDestroyOnLoad(xrIntMan);


    }

    // Update is called once per frame
    void Update()
    {

        if (SceneManager.GetActiveScene().name == "Forge 2" && ingredientListNames.Count > 0 && allIngredientList.Count <= 6)
        {
            Scene currentScene = SceneManager.GetActiveScene();
            Debug.Log("In " + SceneManager.GetActiveScene().name + "And Found " + ingredientListNames.Count + "Ingredients");
            GameObject[] allIngredients = Resources.FindObjectsOfTypeAll<GameObject>();

            Debug.Log("Found " + allIngredients.Length + " amount of ingredients");
            foreach (GameObject go in allIngredients)
            {
                if (go.scene == currentScene && go.CompareTag("Ingredient"))
                {
                    allIngredientList.Add(go);
                }
            }

            for (int i = 0; i < allIngredients.Length; i++)
            {
                allIngredients[i] = null;
            }

            foreach (GameObject obj in allIngredientList)
            {
                Debug.Log("Found " + obj.name);
                if (ingredientListNames.Contains(obj.name) && !obj.activeSelf)
                {
                    obj.SetActive(true);
                }
            }
        }
    }


    private void OnEnable()
    {
        Interactor.selectEntered.AddListener(OnGrab);
    }

    private void OnDisable()
    {
        Interactor.selectEntered.RemoveListener(OnGrab);
    }

    private void OnGrab(SelectEnterEventArgs args)
    {
        //string grabbedObjName = args.interactableObject.transform.gameObject.name;
        GameObject grabbedObj = args.interactableObject.transform.gameObject;

        if (grabbedObj.CompareTag("Ingredient") && SceneManager.GetActiveScene().name != "Forge 2")
        {
            if (!ingredientListNames.Contains(grabbedObj.name))
            {
                ingredientListNames.Add(grabbedObj.name);
                Debug.Log("Added " + grabbedObj.name + "to list");
                if (!grabbedObj == false)
                {
                    grabbedObj.SetActive(false);
                }
            }
        }
    }
}
