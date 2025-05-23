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
        //sets the start position of the player
        lastPosition = playerObj.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 deltaPosition = playerObj.transform.position - lastPosition;

        // Calculate the velocity 
        float velocity = deltaPosition.magnitude / Time.deltaTime;

        // Update the last position for the next frame
        lastPosition = playerObj.transform.position;
        if (velocity > 0.1f) // small threshold to avoid false triggers
        {
            footstepTimer -= Time.deltaTime;

            //footstep sound plays whenever the player
            if (footstepTimer <= 0f)
            {
                footstepSource.Play();
                footstepTimer = footstepInterval;
            }
        }
        //Stop footstep sound if player isn't moving.
        else
        {
            footstepSource.Stop();
        }
    }
}
