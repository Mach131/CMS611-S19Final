using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Represents a plot for seeds to grow in.
/// </summary>
public class Plot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Seed plantedSeed;
    public GameObject plotMenu;
    public GameObject buyMenu;
    public HoverToolTip toolTip;

    [Header("Reference to prefabs")]
    public GameObject seedPrefabObject;
    //public GameObject plantMenuPrefab;
    public static int plotPrice = 10;

    GameFlowController controller;

    private Player player;

    public bool unlocked = false;
    public int state = 0;
    // State 0 is empty, 1 is planted, 2 is ready to harvest. 

    // Start is called before the first frame update
    void Awake()
    {
        player = FindObjectOfType<Player>();
        controller = FindObjectOfType<GameFlowController>();

        //plotMenu = Instantiate(plantMenuPrefab, transform);
    }

    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        //Debug.Log(string.Format("Mouse now over plot {0}.", this.gameObject.name));
        if (this.state != 0)
        {
            toolTip.ShowToolTip(this);
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        //Debug.Log(string.Format("Mouse no longer over plot {0}.", this.gameObject.name));
        if (toolTip.gameObject.activeSelf)
        {
            toolTip.HideToolTip();
        }
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

        if (state == 2 && plantedSeed.isDoneGrowing())
        {
            Debug.Log(plantedSeed.harvestAmount);
            player.cropInventory[plantedSeed.cropType] += plantedSeed.harvestAmount;
            Debug.Log(plantedSeed.cropType + ": " + player.cropInventory[plantedSeed.cropType]);

            Destroy(plantedSeed.gameObject);

            Text timeLeft = findPlantText();
            timeLeft.text = "Empty";
            //Changes the state holder to empty
            state = 0;

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
        if (unlocked) 
        {
            if (state == 0)
            {
                //make seed, initialize with given type
                GameObject newPlant = Instantiate(seedPrefabObject, transform);
                plantedSeed = newPlant.GetComponent<Seed>();
                plantedSeed.Initialize(type);

                Text timeLeft = findPlantText();
                timeLeft.text = plantedSeed.timeLeft().ToString();
                //Changes state to planted
                state = 1;
            }
        }
        // Close menu
        this.plotMenu.SetActive(false);
    }

    /// <summary>
    /// Brings up a menu for this plot object.
    /// </summary>
    public void PlotMenu()
    {
        if (state == 0 && unlocked) 
        {
            foreach (SeedEntry entry in plotMenu.GetComponent<SeedScroller>().currentEntries)
            {
                Debug.Log(string.Format("Found button for {0}.", entry.cropName.text));
            }
        }
    }

    public void BuyMenu()
    {
        if (!unlocked)
        {
            Button[] buttons = buyMenu.GetComponentsInChildren<Button>();
            Button yes = null;
            foreach (Button b in buttons)
            {
                if (b.name == "Yes Button")
                    yes = b;
            }
            yes.onClick.RemoveAllListeners();
            yes.onClick.AddListener(this.buyPlot);
        }
    }


    /////Other public functions

    /// <summary>
    /// Simulate the seed in this plot growing for one turn. If there is no seed here, does nothing and returns false.
    /// </summary>
    /// <returns>True if the seed is done growing, false otherwise</returns>
    public bool passTurn()
    {
        if (state == 1)
        {

            int turns = plantedSeed.timeLeft() - 1;

            if (turns < 0)
                turns = 0;
            Text timeLeft = findPlantText();
            timeLeft.text = turns.ToString();
            if (turns == 0) 
            { 
                timeLeft.text = "Harvest";
                //state turned to ready
                state = 2;
            }


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

    public void onClick()
    {
        controller.lastPlotToClick = this;
        if (!unlocked)
        {
           this.buyMenu.transform.position = this.transform.position;
           this.buyMenu.SetActive(true);
            BuyMenu();
        }
        else if (state == 0)
        {
            this.plotMenu.transform.position = this.transform.position;
            this.plotMenu.SetActive(true);
            PlotMenu();
        }
        else
        {
            attemptHarvest();
        }
    }

    public void buyPlot()
    {
        int amount = Plot.plotPrice;
        if (!unlocked && player.currentMoney >= amount)
        {
            player.currentMoney -= amount;
            unlocked = true;
            Text timeLeft = findPlantText();
            timeLeft.text = "Empty";
        }
        else
        {
            Debug.Log("You don't have enough money to purchase this plot.");
        }
        // Close menu
        this.buyMenu.SetActive(false);
    }
}
