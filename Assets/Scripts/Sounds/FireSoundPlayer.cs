using UnityEngine;
using UnityEngine.SceneManagement;

public class FireSoundPlayer : MonoBehaviour
{
    public bool isLit = false; // Public flag to control if the forge is lit (can be set from other scripts)

    private AudioSource audio; // Reference to the AudioSource component

    void Start()
    {
        audio = GetComponent<AudioSource>(); //Get the audiosource on same GameObject.
        UpdateFireSound(); // Set the correct sound state on start
    }

    void Update()
    {
        UpdateFireSound(); // Automatically turn on fire sound if we're in a "Forge" scene
        if (SceneManager.GetActiveScene().name == "Forge 1" || SceneManager.GetActiveScene().name == "Forge" || SceneManager.GetActiveScene().name == "Forge Level 1" || SceneManager.GetActiveScene().name == "Forge Level 2")
        {
            isLit = true; // Mark the forge as lit in these scenes
        } 
    }

    private void UpdateFireSound() // Plays or stops the audio depending on whether the forge is lit
    { 
        if (isLit && !audio.isPlaying) // If the forge is lit but the sound isn't playing, start playing
        {
            audio.Play();
        }
        else if (!isLit && audio.isPlaying)  // If the forge isn't lit but the sound is playing, stop it
        {
            audio.Stop();
        }
    }
}
