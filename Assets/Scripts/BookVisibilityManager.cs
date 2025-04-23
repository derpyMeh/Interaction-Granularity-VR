using UnityEngine;
using System.Collections.Generic;

public class BookVisibilityManager : MonoBehaviour
{
    public static BookVisibilityManager Instance;

    [Header("Assign all book GameObjects here")]
    [SerializeField] private List<GameObject> allBooks = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// Only shows the book with the matching ID, hides all others.
    public void ShowOnlyBook(string allowedID)
    {
        Debug.Log("ShowOnlyBook called with ID: " + allowedID);

        foreach (var book in allBooks)
        {
            if (book == null) continue;

            var id = book.GetComponent<BookID>();
            bool isMatch = id != null && id.bookID == allowedID;

            Debug.Log($"Book {book.name} (ID: {id?.bookID}) match: {isMatch}");

            book.SetActive(isMatch);
        }
    }

    /// Re-enables all books when returning to the interaction scene.
    public void ShowAllBooks()
    {
        Debug.Log("ShowAllBooks called — restoring all books");

        foreach (var book in allBooks)
        {
            if (book != null)
                book.SetActive(true);
        }
    }
}
