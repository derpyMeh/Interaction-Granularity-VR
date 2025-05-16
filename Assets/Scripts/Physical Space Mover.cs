using UnityEngine;

public class PhysicalSpaceMover : MonoBehaviour
{
    public Transform xrCamera; 
    public CapsuleCollider capsuleCol; 
    

    // Update is called once per frame
    void Update()
    {
        Vector3 headsetPosition = xrCamera.localPosition;

        // Move the character controller horizontally to follow the headset
        Vector3 newPosition = new Vector3(headsetPosition.x, 0, headsetPosition.z);
        capsuleCol.center = new Vector3(headsetPosition.x, capsuleCol.height / 2f, headsetPosition.z);

    }
}
