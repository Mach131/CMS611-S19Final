using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Responsible for having the screen fade to and from black when moving between scenes
/// </summary>
public class SceneTransitionFader : MonoBehaviour
{
    public bool startingFade;
    public float fadeDuration = 1;

    private Image image;
    private bool isFading;
    private float currentAlpha;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        isFading = false;
        currentAlpha = startingFade ? 1 : 0;
        updateAlpha();

        StartCoroutine(fadeFromBlack(fadeDuration));
    }
    
    /// <summary>
    /// Initiates the fade to black for transitioning to a new scene.
    /// </summary>
    /// <returns>The coroutine associated with the fade.</returns>
    public Coroutine activateTransitionFade()
    {
        return StartCoroutine(fadeToBlack(fadeDuration));
    }

    /// <summary>
    /// Causes the black overlay to decrease in opacity over a period of time. If it is already in the process of
    /// of fading, waits until the previous fade is done.
    /// </summary>
    /// <param name="overSeconds">How long in seconds to take to fade out. Must be positive.</param>
    /// <returns>Time to wait between updates</returns>
    private IEnumerator fadeFromBlack(float overSeconds)
    {
        //Wait for any other fading to complete (pls no concurrency tho)
        while (isFading)
        {
            yield return null;
        }
        isFading = true;

        float totalDelta = currentAlpha/overSeconds;
        
        while (currentAlpha > 0)
        {
            updateAlpha();
            float currentDelta = totalDelta * Time.deltaTime;
            currentAlpha -= currentDelta;
            yield return null;
        }

        currentAlpha = 0;
        updateAlpha();
        isFading = false;
    }

    /// <summary>
    /// Causes the black overlay to increase in opacity over a period of time. If it is already in the process of
    /// of fading, waits until the previous fade is done.
    /// </summary>
    /// <param name="overSeconds">How long in seconds to take to fade in. Must be positive.</param>
    /// <returns>Time to wait between updates</returns>
    private IEnumerator fadeToBlack(float overSeconds)
    {
        //Wait for any other fading to complete (pls no concurrency tho)
        while (isFading)
        {
            yield return null;
        }
        isFading = true;

        float totalDelta = (1 - currentAlpha) / overSeconds;

        while (currentAlpha < 1)
        {
            updateAlpha();
            float currentDelta = totalDelta * Time.deltaTime;
            currentAlpha += currentDelta;
            yield return null;
        }

        currentAlpha = 1;
        updateAlpha();
        isFading = false;
    }

    /// <summary>
    /// Updates the alpha of the attached image to the currentAlpha.
    /// </summary>
    private void updateAlpha()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, currentAlpha);
    }
}
