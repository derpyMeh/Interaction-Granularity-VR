using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SceneOrbBehavior : MonoBehaviour
{
    public introCutsceneController controller;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerHand") || other.CompareTag("MainCamera"))
        {
            Debug.Log("Grabbed orb");
            string targetScene = controller.GetSelectedSceneName();
            controller.FadeAndLoadScene(targetScene);
        }
    }
}
