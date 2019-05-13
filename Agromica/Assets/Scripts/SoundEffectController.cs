using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the playing of sound effects for things like buttons being pressed.
/// </summary>
public class SoundEffectController : MonoBehaviour
{
    public AudioClip buttonSoundEffect;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = FindObjectOfType<AudioSource>();
    }

    /// <summary>
    /// The sound for pressing a button.
    /// </summary>
    public void playButtonSound()
    {
        audioSource.PlayOneShot(buttonSoundEffect);
    }
}
