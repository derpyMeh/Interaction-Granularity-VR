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

    private void ActiveObj()
    {
        objToActivate.SetActive(true);
        Debug.Log("✅ Activated: " + objToActivate.name);

        foreach (GameObject obj in ingredientsList)
        {
            Debug.Log("✅ Inactivated: " + obj.name);
            obj.SetActive(false);

        }



    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(coalTag))
        {
            coalInFurnace = true;
            Debug.Log("Coal in Furnace");
            fireStart.SetActive(true);
            fireSoundStarter.isLit = true; 

        }

        if (SceneManager.GetActiveScene().name == "Forge 1" || SceneManager.GetActiveScene().name == "Forge")
        {

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
        else if(SceneManager.GetActiveScene().name == "Forge 2")
        {
            if (other.CompareTag(ingredientTag) && coalInFurnace)
            {
                GameObject ingredientObj = other.gameObject;
                Debug.Log("Ingredient " +  ingredientObj.name + " added");
                if (!ingredientsList.Contains(ingredientObj))
                {
                    ingredientsList.Add(ingredientObj);

                    if (ingredientsList.Count >= 6)
                    {
                        ActiveObj();
                        Debug.Log("Bar Spawned");
                    }
                }

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(ingredientTag))
        {
            ingredientsList.Remove(other.gameObject);
            Debug.Log("Removed: " + other.gameObject.name);
        }
    }
}
