using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ReturnBook : MonoBehaviour
{
      [SerializeField] private string returnSceneName = "Interaction Scene";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Book"))
        {
            SceneManager.LoadScene(returnSceneName);
        }
    }
}
