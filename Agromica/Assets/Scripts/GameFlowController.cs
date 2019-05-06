using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the flow of the game, particularly initialization and the changing of state between turns.
/// </summary>
public class GameFlowController : MonoBehaviour
{
    [Header("Scenario Data Files")]
    public bool loadFromFile;
    public TextAsset scenarioData;
    [Header("Scenario Data")]
    public int numberOfRounds;
    public List<Crop> availableCrops;
    public List<Quota> quotas;
    [Header("Scene References")]
    public Player playerObject;

    public Dictionary<string, Crop> nameToCrop;
    public Dictionary<int, Quota> turnToQuota;

    public int currentTurn;
    private Player player;
    private Market market;
    private Bank bank;

    public GameObject failedQuotaMessage;
    public GameObject passedQuotaMessage;

    /////Helper Classes

    /// <summary>
    /// Represents a crop, containing data that the market needs to be initialized
    /// </summary>
    [System.Serializable]
    public class Crop
    {
        public string cropName;
        public int turnsToGrow;

        //path of icon sprite under resources folder; e.g. "Sprites/taroIcon"
        public string iconResourcePath;

        //market variables
        public float baseSupply;
        public float baseDemand;
        public float mVarC1;
        public float mVarC2;
        public float mVarS;
        public float mVarD;
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
            public string cropName;
            public int requiredAmount;
        }
    }

    /// <summary>
    /// Allows the information contained in a JSON scenario data file to be loaded into the game.
    /// </summary>
    [System.Serializable]
    private class ScenarioDataLoader
    {
        public int rounds = 0;
        public Crop[] crops = new Crop[0];
        public Quota[] quotas = new Quota[0];
    }



    /////Public Methods

    /// <summary>
    /// Get the crop information object associated with the given crop name.
    /// </summary>
    /// <param name="cropName">The name of the desired crop</param>
    /// <returns>The informational object associated with that crop</returns>
    public Crop cropLookup(string cropName)
    {
        return nameToCrop[cropName];
    }

    /// <summary>
    /// Perform the game updates that occur between turns, particularly growing crops and filling quotas.
    /// </summary>
    public void updateTurn()
    {
        //quotas
        if (turnToQuota.ContainsKey(currentTurn))
        {
            bool failedQuota = false;

            Quota currentQuota = turnToQuota[currentTurn];
            // Checks if the quota can be reached
            foreach (Quota.Requirement req in currentQuota.cropRequirements)
            {
                string reqCrop = req.cropName;
                if (!(player.cropInventory[reqCrop] >= req.requiredAmount))
                {
                    failedQuota = true;
                }
            }

            if (failedQuota)
            {
                Debug.Log("failed quota");
                player.currentDebt += 500;
                player.updateInventory();
                // TODO: temp penalty for failing quota
                Instantiate(failedQuotaMessage, new Vector3(0, 0, 0), Quaternion.identity);
            }
            else
            {
                // Only removes if the quota is reached
                foreach (Quota.Requirement req in currentQuota.cropRequirements) {
                    string reqCrop = req.cropName;
                    player.cropInventory[reqCrop] -= req.requiredAmount;
                }
                Instantiate(passedQuotaMessage, new Vector3(0, 0, 0), Quaternion.identity);
            }
        }

        //win condition
        if (currentTurn >= numberOfRounds)
        {
            Debug.Log("all rounds finished");
            //TODO: win condition, making sure u haven't already lost
        }
        else
        {

            //growing
            Farm[] farms = FindObjectsOfType<Farm>();
            foreach (Farm farm in farms)
            {
                farm.passTurn();
            }
            
            if (market != null)
            {
                market.passTurn();
            }

            currentTurn += 1;
        }

        player.updateInventory();
        bank.CompoundInterest(currentTurn);
    }

    /////Private Methods
    
    /// <summary>
    /// Initializes references; prevents missing references from inactive UI elements
    /// </summary>
    private void Awake()
    {
        if (loadFromFile)
        {
            LoadScenario();
        }

        player = FindObjectOfType<Player>();
        market = FindObjectOfType<Market>();
        bank = FindObjectOfType<Bank>();
    }

    /// <summary>
    /// Initialize the scenario
    /// </summary>
    private void Start()
    {
        nameToCrop = new Dictionary<string, Crop>();
        turnToQuota = new Dictionary<int, Quota>();

        //make dictionaries from public lists to facilitate other methods
        //use crop list to make dict, then seeds can use it to figure out growth time
        foreach (Crop crop in availableCrops)
        {
            nameToCrop.Add(crop.cropName, crop);
            //initialize player inventory while we're at it
            player.cropInventory.Add(crop.cropName, 0);
        }
        //use quota list to make dict so that it can easily check when something is due
        foreach (Quota quota in quotas) {
            turnToQuota.Add(quota.turnNumber, quota);
        }
        
        currentTurn = 0;

    }

    /// <summary>
    /// Allows the scenario data to be loaded from a text file (in JSON format, matching the structure of the ScenarioDataLoader class).
    /// </summary>
    private void LoadScenario()
    {
        ScenarioDataLoader loadedData = JsonUtility.FromJson<ScenarioDataLoader>(scenarioData.text);
        numberOfRounds = loadedData.rounds;
        availableCrops = new List<Crop>(loadedData.crops);
        quotas = new List<Quota>(loadedData.quotas);
    }
}
