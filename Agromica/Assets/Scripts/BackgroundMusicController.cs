using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows the background music to be stopped and started. There should only be one object with this script, and
/// it will stay around between scene transitions; other objects with this script will destroy themselves on awakening.
/// </summary>
public class BackgroundMusicController : MonoBehaviour
{
    private static BackgroundMusicController controllerInstance;

    private AudioSource audioSource;

    private void Awake()
    {
        if (controllerInstance == null)
        {
            controllerInstance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Starts playing the background music, if it isn't already playing.
    /// </summary>
    public void startMusic()
    {
        if (!audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    /// <summary>
    /// Stops playing the background music, if it is currently playing.
    /// </summary>
    public void stopMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}
