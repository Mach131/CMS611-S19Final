using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Provides an easier way to set up buttons to play the appropriate sound when clicked.
/// </summary>
public class AttachButtonSound : MonoBehaviour
{
    private void Start()
    {
        SoundEffectController sfxController = FindObjectOfType<SoundEffectController>();
        GetComponent<Button>().onClick.AddListener(() => sfxController.playButtonSound());
    }
}
