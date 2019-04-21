﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a plot for seeds to grow in.
/// </summary>
public class Plot : MonoBehaviour
{
    public bool currentlyAvailable;
    public Seed plantedSeed;
    public Canvas plotMenu;
    [Header("Reference to seed prefab")]
    public GameObject seedPrefabObject;

    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        currentlyAvailable = true;
    }

    /////Public UI functions

    /// <summary>
    /// Harvest a crop that has finished growing in this plot. Fails if there is no seed growing here, or if it is not done growing.
    /// When successful, this function will delete the seed growing here and accordingly modify the player's inventory.
    /// </summary>
    /// <returns>True if harvesting was successful, false otherwise</returns>
    public bool Harvest()
    {
        // Should be an onClick function
        // Needs to be related to self

        if (!currentlyAvailable && plantedSeed.isDoneGrowing())
        {
            player.cropInventory[plantedSeed.cropType] += 1;

            Destroy(plantedSeed.gameObject);
            currentlyAvailable = true;

            return true;
        }

        return false;
    }

    /// <summary>
    /// Plants a seed in this plot. Does nothing if this plot is already occupied.
    /// </summary>
    /// <param name="type">The name of the crop to plant here; must be listed in GameFlowController's available crops</param>
    public void Plant(string type)
    {
        if (currentlyAvailable)
        {
            this.plotMenu.gameObject.SetActive(false);

            //make seed, initialize with given type
            GameObject newPlant = Instantiate(seedPrefabObject, transform);
            plantedSeed = newPlant.GetComponent<Seed>();
            plantedSeed.Initialize(type);

            currentlyAvailable = false;
        }
    }

    /// <summary>
    /// Brings up a menu for this plot object.
    /// </summary>
    /// <param name="plotMenu">The menu to bring up</param>
    public void PlotMenu(Canvas plotMenu)
    {
        Debug.Log("I have been clicked");
        if (currentlyAvailable) 
        {
            this.plotMenu = plotMenu;
            this.plotMenu.gameObject.SetActive(true); 
        }
    }


    /////Other public functions

    /// <summary>
    /// Simulate the seed in this plot growing for one turn. If there is no seed here, does nothing and returns false.
    /// </summary>
    /// <returns>True if the seed is done growing, false otherwise</returns>
    public bool passTurn()
    {
        if (!currentlyAvailable)
        {
            return plantedSeed.passTurn();
        }
        return false;
    }
}
