using UnityEngine;

public class FootstepsLogic : MonoBehaviour
{
    public GameObject playerObj;
    public AudioSource footstepSource;
    private Vector3 lastPosition;

    public float footstepInterval = 0.5f; // Time between steps
    private float footstepTimer = 0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lastPosition = playerObj.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 deltaPosition = playerObj.transform.position - lastPosition;

        // Calculate the velocity (change in position over time)
        float velocity = deltaPosition.magnitude / Time.deltaTime;

        // Update the last position for the next frame
        lastPosition = playerObj.transform.position;
        if (velocity > 0.1f) // small threshold to avoid false triggers
        {
            footstepTimer -= Time.deltaTime;

            if (footstepTimer <= 0f)
            {
                footstepSource.Play();
                footstepTimer = footstepInterval;
            }
        }
        else
        {
            footstepTimer = 0f; // reset timer when not moving
        }
    }
}
