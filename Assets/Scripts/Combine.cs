using System.Collections.Generic;
using UnityEngine;

public class Combine : MonoBehaviour
{
    [SerializeField] List<GameObject> ingredientsList = new List<GameObject>();
    private string ingredientTag = "Ingredient";

    public GameObject objToActivate;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }
        

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

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(ingredientTag))
        {
            ingredientsList.Remove(other.gameObject);
            Debug.Log("Removed: " + other.gameObject.name);
        }
    }
}
