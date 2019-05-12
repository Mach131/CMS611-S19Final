using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a crop iin a plot that has not yet fully grown.
/// </summary>
public class Seed : MonoBehaviour
{
    public string cropType;

    private int growthTurnsRemaining;
    private List<Sprite> plotSprites;
    private int totalGrowthTurns;

    public int harvestAmount;

    /// <summary>
    /// Sets up the seed based on the crop it grows into. Call this before using any other methods.
    /// </summary>
    /// <param name="cropType">The crop type that this seed grows into; must be listed in GameFlowController's available crops</param>
    public void Initialize(string cropType)
    {
        this.cropType = cropType;
        GameFlowController mainController = FindObjectOfType<GameFlowController>();
        GameFlowController.Crop cropInfo = mainController.cropLookup(cropType);

        totalGrowthTurns = cropInfo.turnsToGrow;
        growthTurnsRemaining = totalGrowthTurns;
        harvestAmount = cropInfo.harvestAmount;

        plotSprites = new List<Sprite>();
        foreach (string spritePath in cropInfo.plotGrowthSpritePaths)
        {
            plotSprites.Add(Resources.Load<Sprite>(spritePath));
        }
        if (plotSprites.Count != totalGrowthTurns + 1)
        {
            Debug.LogWarning("There may be an incorrect number of sprites for this plant");
        }
    }

    /// <summary>
    /// Simulate the seed growing for one turn. Make sure to call Initialize before this.
    /// </summary>
    /// <returns>True if the seed is done growing, false otherwise</returns>
    public bool passTurn()
    {
        growthTurnsRemaining -= 1;
        return isDoneGrowing();
    }

    /// <summary>
    /// Check if the seed is done growing.
    /// </summary>
    /// <returns>True if the seed is done growing, false otherwise</returns>
    public bool isDoneGrowing()
    {
        return growthTurnsRemaining <= 0;
    }

    public int timeLeft()
    {
        return growthTurnsRemaining;
    }

    /// <summary>
    /// Gets the current sprite that the plot showld use to represent the growth state.
    /// </summary>
    /// <returns>The sprite corresponding to this plant's growth progress</returns>
    public Sprite currentPlotSprite()
    {
        int spriteIndex = (int) Mathf.Clamp(totalGrowthTurns - growthTurnsRemaining, 0, plotSprites.Count - 1);
        return plotSprites[spriteIndex];
    }
}
