using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the flow of the game, particularly initialization and the changing of state between turns.
/// </summary>
public class GameFlowController : MonoBehaviour
{
    [Header("Scenario Data")]
    public int numberOfRounds;
    public List<Crop> availableCrops;
    public List<Quota> quotas;
    [Header("Scene References")]
    public Player playerObject;
    //TODO: market

    private int currentTurn;

    /// <summary>
    /// Represents a crop, containing data that the market needs to be initialized
    /// </summary>
    [System.Serializable]
    public class Crop
    {
        public string cropName;
        //TODO: stuff that the market needs to know (reactivity, starting prices)
    }

    /// <summary>
    /// Represents a quota, containing information about when it is due and what is required.
    /// </summary>
    [System.Serializable]
    public class Quota
    {
        //when the quota is due
        public int turnNumber;
        //maps the index of the crop (in availableCrops) to how many are needed
        [SerializeField]
        public List<Requirement> cropRequirements;

        [System.Serializable]
        public struct Requirement
        {
            public int cropIndex;
            public int requiredAmount;
        }
    }


    /// <summary>
    /// Initialize the scenario
    /// </summary>
    private void Start()
    {
        //TODO: send crop info to market
        currentTurn = 0;
    }
}
