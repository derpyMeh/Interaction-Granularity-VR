using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Combine : MonoBehaviour
{
    [SerializeField] List<GameObject> ingredientsList = new List<GameObject>();
    private string ingredientTag = "Ingredient";
    private string coalTag = "Coal";

    private bool coalInFurnace = false;
    public GameObject objToActivate;
    public FireSoundPlayer fireSoundStarter;
    public GameObject fireStart;

    // Activates the target object and deactivates all other objects in the ingredientsList
    private void ActiveObj()
    {
        objToActivate.SetActive(true);

        foreach (GameObject obj in ingredientsList)
        {
            obj.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // If the object that entered has the "coal" tag, mark the furnace as lit
        if (other.CompareTag(coalTag))
        {
            coalInFurnace = true;

            // Activate visual fire and mark the fire sound trigger
            fireStart.SetActive(true);
            fireSoundStarter.isLit = true; 
        }

        // Check which scene the player is in
        if (SceneManager.GetActiveScene().name == "Forge 1" ||
            SceneManager.GetActiveScene().name == "Forge" ||
            SceneManager.GetActiveScene().name == "Forge Level 1" ||
            SceneManager.GetActiveScene().name == "Forge Level 2")
        {
            //Check if object in collider has the ingredient tag, if it does, add it to the ingredient list if it hasn't already been, then call the ActiveObj method
            if (other.CompareTag(ingredientTag))
            {
                GameObject ingredientObj = other.gameObject;
                if (!ingredientsList.Contains(ingredientObj))
                {
                    ingredientsList.Add(ingredientObj);

                    if (ingredientsList.Count >= 6)
                    {
                        ActiveObj();
                    }
                }

            }
        }

        // For Forge 2 and Forge Level 3, only allow ingredient collection if the furnace is lit
        else if (SceneManager.GetActiveScene().name == "Forge 2" ||
            SceneManager.GetActiveScene().name == "Forge Level 3")
        {
            if (other.CompareTag(ingredientTag) && coalInFurnace)
            {
                GameObject ingredientObj = other.gameObject;

                // Only add the ingredient if it’s not already in the list
                if (!ingredientsList.Contains(ingredientObj))
                {
                    ingredientsList.Add(ingredientObj);

                    // Activate the object once enough ingredients are collected
                    if (ingredientsList.Count >= 6)
                    {
                        ActiveObj();
                    }
                }

            }
        }
    }

    //If object leaves collider, remove it from the ingredient list
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(ingredientTag))
        {
            ingredientsList.Remove(other.gameObject);
        }
    }
}
