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

    public void goToTargetScene()
    {
        SceneManager.LoadScene(targetSceneName);
    }
}
