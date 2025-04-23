using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class BookShelfGroupManager : MonoBehaviour
{
    public List<GameObject> allBooks; // drag all your book objects in here

    public static BookShelfGroupManager Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void OnBookGrabbed(GameObject selectedBook)
    {
        foreach (GameObject book in allBooks)
        {
            if (book == selectedBook) continue;

            Rigidbody rb = book.GetComponent<Rigidbody>();
            Collider col = book.GetComponent<Collider>();

            if (rb != null)
            {
                rb.isKinematic = true;
                rb.detectCollisions = false;
            }

            if (col != null)
                col.enabled = false;
        }
    }

    public void OnBookReleased(GameObject selectedBook)
    {
        foreach (GameObject book in allBooks)
        {
            if (book == selectedBook) continue;

            Rigidbody rb = book.GetComponent<Rigidbody>();
            Collider col = book.GetComponent<Collider>();

            if (rb != null)
            {
                rb.isKinematic = false;
                rb.detectCollisions = true;
            }

            if (col != null)
                col.enabled = true;
        }
    }
}
