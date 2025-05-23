using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SceneOrbBehavior : MonoBehaviour
{
    public introCutsceneController controller; // Reference to the cutscene controller that holds information about the next scene

    private void OnTriggerEnter(Collider other) // Called automatically by Unity when another collider enters this object's trigger collider
    {
        if (other.CompareTag("PlayerHand") || other.CompareTag("MainCamera")) // Check if the collider belongs to the player's hand or headset camera
        {
            Debug.Log("Grabbed orb"); // Log interaction for debugging
            string targetScene = controller.GetSelectedSceneName(); // Ask the controller which scene should be loaded next
            CameraEffectController cam = FindObjectOfType<CameraEffectController>(); // Find the camera fade controller in the scene
            if (cam != null)
            {
                cam.FadeToScene(targetScene, 5f); // Trigger a fade to the selected scene over 5 seconds
            } 
        }
    }
}
