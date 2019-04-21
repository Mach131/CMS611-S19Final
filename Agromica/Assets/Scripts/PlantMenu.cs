using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Helps control a menu allowing players to decide what to plant.
/// </summary>
public class PlantMenu : MonoBehaviour
{
    private List<string> availableSeeds;
    public List<Button> seedButtonsRedBlue; //should be private when buttons are generated

    // Start is called before the first frame update
    void Start()
    {
        availableSeeds = new List<string>();
        foreach(GameFlowController.Crop crop in FindObjectOfType<GameFlowController>().availableCrops)
        {
            availableSeeds.Add(crop.cropName);
        }

        //todo: generate buttons based on crop list
    }

    /// <summary>
    /// Initializes the plot by connecting the functionality of the buttons to a plot.
    /// </summary>
    /// <param name="forPlot">The plot to connect to</param>
    public void Initialize(Plot forPlot)
    {
        //hardcoded for testing
        seedButtonsRedBlue[0].onClick.AddListener(() => forPlot.Plant("Red"));
        seedButtonsRedBlue[1].onClick.AddListener(() => forPlot.Plant("Blue"));
    }
}
