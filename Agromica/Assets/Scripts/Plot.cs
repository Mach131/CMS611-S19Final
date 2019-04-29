using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Represents a plot for seeds to grow in.
/// </summary>
public class Plot : MonoBehaviour
{
    public bool currentlyAvailable;
    public Seed plantedSeed;
    public GameObject plotMenu;
    [Header("Reference to prefabs")]
    public GameObject seedPrefabObject;
    public GameObject plantMenuPrefab;

    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();
        currentlyAvailable = true;

        plotMenu = Instantiate(plantMenuPrefab, transform);
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
            Debug.Log(plantedSeed.cropType + ": " + player.cropInventory[plantedSeed.cropType]);

            Destroy(plantedSeed.gameObject);
            currentlyAvailable = true;

            Text timeLeft = findPlantText();
            timeLeft.text = "Empty Plot";

            player.updateInventory();

            return true;
        }

        return false;
    }

    /// <summary>
    /// Harvests a crop, but doesn't return information about its success. Meant to make things work with GUI.
    /// </summary>
    public void attemptHarvest()
    {
        Harvest();
    }

    /// <summary>
    /// Plants a seed in this plot. Does nothing if this plot is already occupied.
    /// </summary>
    /// <param name="type">The name of the crop to plant here; must be listed in GameFlowController's available crops</param>
    public void Plant(string type)
    {
        if (currentlyAvailable)
        {
            this.plotMenu.SetActive(false);

            //make seed, initialize with given type
            GameObject newPlant = Instantiate(seedPrefabObject, transform);
            plantedSeed = newPlant.GetComponent<Seed>();
            plantedSeed.Initialize(type);

            currentlyAvailable = false;

            Text timeLeft = findPlantText();
            timeLeft.text = plantedSeed.timeLeft().ToString();
        }
    }

    /// <summary>
    /// Brings up a menu for this plot object.
    /// </summary>
    public void PlotMenu()
    {
        if (currentlyAvailable) 
        {
            this.plotMenu.SetActive(true);
            this.plotMenu.GetComponent<PlantMenu>().Initialize(this);
            // Change text to Open Plot
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

            int turns = plantedSeed.timeLeft() - 1;

            if (turns < 0)
                turns = 0;
            Text timeLeft = findPlantText();
            timeLeft.text = turns.ToString();
            if (turns == 0)
                timeLeft.text = "Harvest";


            return plantedSeed.passTurn();
        }
        return false;
    }

    private Text findPlantText()
    {
        Transform[] ts = this.gameObject.GetComponentsInChildren<Transform>();
        Text timeLeft = null;
        foreach (Transform child in ts)
        {
            if (child.name is "PlantText")
                timeLeft = child.gameObject.GetComponent<Text>();

        }
        return timeLeft;
    }
}
