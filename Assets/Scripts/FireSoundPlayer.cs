using UnityEngine;

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
