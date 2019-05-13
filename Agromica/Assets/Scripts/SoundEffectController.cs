using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the playing of sound effects for things like buttons being pressed.
/// </summary>
public class SoundEffectController : MonoBehaviour
{
    public AudioClip buttonSoundEffect;
    [Header("Random variation in pitch")]
    public float lowPitchFactor = 0.9f;
    public float highPitchFactor = 1.1f;

    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// The sound for pressing a button.
    /// </summary>
    public void playButtonSound()
    {
        audioSource.pitch = Random.Range(lowPitchFactor, highPitchFactor);
        audioSource.PlayOneShot(buttonSoundEffect);
    }
}
