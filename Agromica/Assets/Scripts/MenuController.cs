using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Facilitates the showing and hiding of a menu like the instructions panel
/// </summary>
public class MenuController : MonoBehaviour
{
    public GameObject menu;
    public bool visibleAtStart;

    private void Start()
    {
        menu.SetActive(visibleAtStart);
    }

    /// <summary>
    /// Updates the visibility of the menu
    /// </summary>
    /// <param name="newStatus">true if the menu should be visible, false otherwise</param>
    public void SetMenuStatus(bool newStatus)
    {
        menu.SetActive(newStatus);
    }
}
