using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Allows information from the Market to be accessed through a MarketMenu prefab.
/// </summary>
public class MarketMenuInterface : MonoBehaviour
{
    /////This script is currently very hardcoded for the sake of getting a functional integration; when crop UI is generalized,
    ///this will probably have to be updated a lot as well

    public Transform marketMenu;

    //TODO: derive from game flow controller instead
    private string[] cropList = new string[] { "Red", "Blue" };

    private Dictionary<string, Text> buyText;
    private Dictionary<string, Text> sellText;
    private Market market;

    /////Public Methods

    /// <summary>
    /// Updates the price text with the current market prices.
    /// </summary>
    public void updatePriceText()
    {
        foreach (string crop in cropList)
        {
            //TODO: may change to floats eventually

            buyText[crop].text = "Price: " + Mathf.CeilToInt(market.getBuyPrice(crop));
            sellText[crop].text = "Price: " + Mathf.CeilToInt(market.getSellPrice(crop));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        market = GetComponent<Market>();

        buyText = new Dictionary<string, Text>();
        sellText = new Dictionary<string, Text>();

        //search for references to buttons and text based on crops
        foreach (string crop in cropList)
        {
            Transform buyButton = marketMenu.Find(crop + "Buy");
            Transform sellButton = marketMenu.Find(crop + "Sell");

            //buttons
            buyButton.GetComponent<Button>().onClick.AddListener(() => market.buyCrop(crop, 1));
            sellButton.GetComponent<Button>().onClick.AddListener(() => market.sellCrop(crop, 1));

            //text
            buyText.Add(crop, buyButton.Find("Price").GetComponent<Text>());
            sellText.Add(crop, sellButton.Find("Price").GetComponent<Text>());
        }

        updatePriceText();
    }
}
