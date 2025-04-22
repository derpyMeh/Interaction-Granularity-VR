using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersistentBook : MonoBehaviour
{
 private static Dictionary<string, GameObject> bookInstances = new Dictionary<string, GameObject>();

    [SerializeField] private string bookID = "DefaultBook"; // Set per book in Inspector

    private void Awake()
    {
        if (bookInstances.ContainsKey(bookID))
        {
            Debug.Log("Duplicate book for ID '" + bookID + "' — destroying");
            Destroy(gameObject);
        }
        else
        {
            bookInstances[bookID] = this.gameObject;
            DontDestroyOnLoad(gameObject);
        }
    }
}
