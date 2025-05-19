using UnityEngine;
using UnityEngine.SceneManagement;

public class FireSoundPlayer : MonoBehaviour
{
    public bool isLit = false; // Set this to true when the forge is lit

    private AudioSource audio;

    void Start()
    {
        audio = GetComponent<AudioSource>();
        UpdateFireSound();
    }

    void Update()
    {
        UpdateFireSound(); // You can remove this line if you control `isLit` via another script
        if (SceneManager.GetActiveScene().name == "Forge 1" || SceneManager.GetActiveScene().name == "Forge" || SceneManager.GetActiveScene().name == "Forge Level 1" || SceneManager.GetActiveScene().name == "Forge Level 2")
        {
            isLit = true;
        }
    }

    private void UpdateFireSound()
    {
        if (isLit && !audio.isPlaying)
        {
            audio.Play();
        }
        else if (!isLit && audio.isPlaying)
        {
            audio.Stop();
        }
    }
}
