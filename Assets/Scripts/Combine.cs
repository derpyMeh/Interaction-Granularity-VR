using System.Collections.Generic;
using UnityEngine;

public class Combine : MonoBehaviour
{
    [SerializeField] List<GameObject> ingredientsList = new List<GameObject>();
    private string ingredientTag = "Ingredient";
    private string coalTag = "Coal";

    private bool coalInFurnace = false;
    public GameObject objToActivate;
    FireSoundPlayer fireSoundStarter;
    public ParticleSystem fireStart;       

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
            fireSoundStarter = GetComponent<FireSoundPlayer>();
            fireStart.Play();
            fireSoundStarter.isLit = true; 

        }

        if (other.CompareTag(ingredientTag) && coalInFurnace)
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

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(ingredientTag))
        {
            ingredientsList.Remove(other.gameObject);
            Debug.Log("Removed: " + other.gameObject.name);
        }
    }
}
