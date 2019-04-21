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

    /// <summary>
    /// Sets up the seed based on the crop it grows into. Call this before using any other methods.
    /// </summary>
    /// <param name="cropType">The crop type that this seed grows into; must be listed in GameFlowController's available crops</param>
    public void Initialize(string cropType)
    {
        GameFlowController mainController = FindObjectOfType<GameFlowController>();
        GameFlowController.Crop cropInfo = mainController.cropLookup(cropType);
        growthTurnsRemaining = cropInfo.turnsToGrow;
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
}
