using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Sets up a transition to another scene.
/// </summary>
public class SceneTransition : MonoBehaviour
{
    public string targetSceneName;

    private SceneTransitionFader transitionFader;
    private bool transitionActivated;

    private void Start()
    {
        transitionFader = FindObjectOfType<SceneTransitionFader>();
        transitionActivated = false;
    }

    /// <summary>
    /// Allows a button press to activate the scene transition.
    /// </summary>
    public void onSceneTransitionPress()
    {
        if (!transitionActivated)
        {
            StartCoroutine(goToTargetScene());
            transitionActivated = true;
        }
    }

    /// <summary>
    /// Has the scene fade to black, then transitions to the target scene.
    /// </summary>
    /// <returns></returns>
    private IEnumerator goToTargetScene()
    {
        if (transitionFader != null)
        {
            yield return transitionFader.activateTransitionFade();
        }
        SceneManager.LoadScene(targetSceneName);
    }
}
