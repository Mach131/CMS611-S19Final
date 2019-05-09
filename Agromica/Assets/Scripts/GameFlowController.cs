using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public int initialPlotsAvailable;
    public int quotaFailureCost;
    [Header("Leave empty if last scene")]
    public string nextScene;

    [Header("Popup Prefabs")]
    public GameObject failedQuotaMessage;
    public GameObject passedQuotaMessage;
    public GameObject sceneFailDebtMessage;
    public GameObject sceneFailQuotaMessage;
    public GameObject scenePassNormalMessage;
    public GameObject scenePassFinalMessage;

    [Header("Scene References")]
    public Player playerObject;

    public Dictionary<string, Crop> nameToCrop;
    public Dictionary<int, Quota> turnToQuota;

    public Plot lastPlotToClick;

    public int currentTurn;
    private Player player;
    private Market market;
    private Bank bank;

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
        public int plots = 1;
        public int debt = 0;
        public int money = 0;
        public int plotPrice = 10;
        public int failureCost = 10;
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
        bool quotaTurn = false;
        bool failedQuota = false;
        if (turnToQuota.ContainsKey(currentTurn))
        {
            quotaTurn = true;

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
                //Interest is added after this is. So it is 10% more.
                player.currentDebt += quotaFailureCost;
                player.updateInventory();
                // TODO: temp penalty for failing quota
                if (currentTurn < numberOfRounds)
                {
                    Instantiate(failedQuotaMessage, new Vector3(0, 0, 0), Quaternion.identity);
                }
            }
            else
            {
                // Only removes if the quota is reached
                foreach (Quota.Requirement req in currentQuota.cropRequirements) {
                    string reqCrop = req.cropName;
                    player.cropInventory[reqCrop] -= req.requiredAmount;
                }
                if (currentTurn < numberOfRounds)
                {
                    Instantiate(passedQuotaMessage, new Vector3(0, 0, 0), Quaternion.identity);
                }
            }
        }

        //win condition
        if (quotaTurn && currentTurn >= numberOfRounds)
        {
            Debug.Log("all rounds finished");
            //TODO: win condition, making sure u haven't already lost
            if (!failedQuota && player.currentMoney >= player.currentDebt)
            {
                Debug.Log("success!!!");
                if (!nextScene.Equals(""))
                {
                    GameObject message = Instantiate(scenePassNormalMessage, new Vector3(0, 0, 0), Quaternion.identity);
                    message.GetComponentInChildren<SceneTransition>().targetSceneName = nextScene;
                } else
                {
                    Instantiate(scenePassFinalMessage, new Vector3(0, 0, 0), Quaternion.identity);
                }
            }
            else
            {
                //show appropriate fail condition
                if (failedQuota)
                {
                    Debug.Log("failed last quota...");
                    Instantiate(sceneFailQuotaMessage, new Vector3(0, 0, 0), Quaternion.identity);
                } else
                {
                    Debug.Log("too much debt...");
                    Instantiate(sceneFailDebtMessage, new Vector3(0, 0, 0), Quaternion.identity);
                }
            }
        }
        else
        {

            //growing
            Plot[] plots = FindObjectsOfType<Plot>();
            foreach (Plot plt in plots)
            {
                plt.passTurn();
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


        player = FindObjectOfType<Player>();
        market = FindObjectOfType<Market>();
        bank = FindObjectOfType<Bank>();

        if (loadFromFile)
        {
            LoadScenario();
        }
        GameObject.Find("Turn Button").GetComponent<Button>().onClick.AddListener(() => updateTurn());
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

        Plot[] plots = FindObjectsOfType<Plot>();
        for (int i = 0; i < initialPlotsAvailable; i++)
        {
            player.currentMoney += Plot.plotPrice;
            plots[i].buyPlot();
        }

    }

    /// <summary>
    /// Allows the scenario data to be loaded from a text file (in JSON format, matching the structure of the ScenarioDataLoader class).
    /// </summary>
    private void LoadScenario()
    {
        ScenarioDataLoader loadedData = JsonUtility.FromJson<ScenarioDataLoader>(scenarioData.text);
        numberOfRounds = loadedData.rounds;
        availableCrops = new List<Crop>(loadedData.crops);
        initialPlotsAvailable = loadedData.plots;
        player.currentMoney = loadedData.money;
        player.currentDebt = loadedData.debt;
        Plot.plotPrice = loadedData.plotPrice;
        quotaFailureCost = loadedData.failureCost;
        quotas = new List<Quota>(loadedData.quotas);
    }
}
