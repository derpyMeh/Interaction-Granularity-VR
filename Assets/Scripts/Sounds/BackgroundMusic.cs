using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    private static BackgroundMusic instance; // Static reference to ensure a single instance

    void Awake()
    {
        if (instance == null) // Check if an instance already exists
        {
            instance = this;  // Set this object as the instance
            DontDestroyOnLoad(gameObject); // Prevent this object from being destroyed on scene load
        } 
        else
        {
            Destroy(gameObject); // Destroy any duplicate music object
        }
    }
}
