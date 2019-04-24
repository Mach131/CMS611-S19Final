using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents one of the player's farms, which contains several plots to grow things in.
/// </summary>
public class Farm : MonoBehaviour
{
    public int initialNumberOfPlots;
    public int maxNumberOfPlots;
    public float offset = 0.8f;
    [Header("Reference to plot prefab")]
    public GameObject plotPrefabObject;

    public int plotPrice = 10;

    private List<Plot> plots;
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<Player>();

        plots = new List<Plot>();
        for (int i = 0; i < initialNumberOfPlots; i ++)
        {
            addPlot();
        }
    }

    /////Public functions
    
    /// <summary>
    /// Simulate the seeds of all plots in this farm growing for one turn. Currently does not harvest anything automatically.
    /// </summary>
    public void passTurn()
    {
        foreach (Plot plot in plots)
        {
            plot.passTurn();
            //this returns true if something is done growing in the plot; could use this for auto-harvest later
        }
    }

    /// <summary>
    /// Adds a new plot to this farm, if space is available.
    /// </summary>
    /// <returns>Whether the addition of a new plot was successful</returns>
    public bool addPlot()
    {
        int plotIndex = plots.Count;
        if (plotIndex < maxNumberOfPlots)
        {
            if (plotIndex < 3)
            {
                GameObject newPlot = Instantiate(plotPrefabObject, new Vector3(plotIndex * 2 + offset, -2), Quaternion.identity, transform);
                plots.Add(newPlot.GetComponent<Plot>());
            }
            else
            {
                GameObject newPlot = Instantiate(plotPrefabObject, new Vector3(-1 * (plotIndex - 3) * 2 - 2 * offset, -2), Quaternion.identity, transform);
                plots.Add(newPlot.GetComponent<Plot>());
            }

            return true;
        }
        // TODO: Make the buy plot button not clickable
        return false;
    }

    public void buyPlot()
    {
        if (player.currentMoney >= plotPrice)
        {
            if (addPlot()) 
            { 
                player.currentMoney -= plotPrice;
                // return true;
            }
        }
        // return false;
    }
}
