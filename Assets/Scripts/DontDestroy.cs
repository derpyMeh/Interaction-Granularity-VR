using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    public List<GameObject> objectPersist = new List<GameObject>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        {
            foreach (GameObject go in objectPersist)
            {
                DontDestroyOnLoad(go);
            }


        }
    }
}