using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows the background music to be stopped and started. There should only be one object with this script, and
/// it will stay around between scene transitions; other objects with this script will destroy themselves on awakening.
/// </summary>
public class BackgroundMusicController : MonoBehaviour
{
    public float fadeOutSeconds = 1;

    private static BackgroundMusicController controllerInstance;

    private AudioSource audioSource;
    private float baseVolume;
    private Coroutine fadeCoroutine;

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
        baseVolume = audioSource.volume;
        fadeCoroutine = null;
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
        else if (fadeCoroutine != null)
        {
            Debug.Log("caught");
            StopCoroutine(fadeCoroutine);
            audioSource.volume = baseVolume;
        }
    }

    /// <summary>
    /// Stops playing the background music, if it is currently playing.
    /// </summary>
    public void stopMusic()
    {
        if (audioSource.isPlaying)
        {
            fadeCoroutine = StartCoroutine(fadeOutMusic());
        }
    }

    private IEnumerator fadeOutMusic()
    {
        while (audioSource.volume > 0)
        {
            audioSource.volume -= (baseVolume/fadeOutSeconds) * Time.deltaTime;
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = baseVolume;
        fadeCoroutine = null;
    }
}
