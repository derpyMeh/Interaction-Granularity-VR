using UnityEngine;

public class BookSpawner : MonoBehaviour
{
    [System.Serializable]
    public class BookEntry
    {
        public string bookName;
        public GameObject prefab;
        public Transform spawnPoint;
        [HideInInspector] public GameObject instance;
    }

    [SerializeField] private BookEntry[] books;

    private void Awake()
    {
        foreach (var book in books)
        {
            if (book.instance == null)
            {
                book.instance = Instantiate(book.prefab, book.spawnPoint.position, book.spawnPoint.rotation);
                book.instance.name = book.bookName;
                DontDestroyOnLoad(book.instance);
            }
        }
    }
}
