using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Represents the market, tracking the current prices of crops and the variables influencing them.
/// Currently heavily assumes that prices are not meant to change until the end of a turn.
/// </summary>
public class Market : MonoBehaviour
{
    public static float buyPriceFactor = 1.1f;
    public static float supplyResetFactor = 0.2f;
    public Image marketPanel;
    private MarketList marketMenuList;

    private Dictionary<string, CropMarketData> cropToData;
    private Player player;

    /// <summary>
    /// Contains relevant market information for a single crop, and provides ways to update it
    /// </summary>
    private class CropMarketData
    {
        private float baseSupply;
        private float baseDemand;
        private float c1;
        private float c2;
        private float s;
        private float d;

        private int timesBought;
        private int timesSold;

        private float currentSupply;
        private float currentDemand;
        private float currentBuyPrice;
        private float currentSellPrice;

        /////Constructor

        /// <summary>
        /// Initializes market data for a crop.
        /// </summary>
        /// <param name="baseSupply">The base supply for the crop</param>
        /// <param name="baseDemand">The base demand for the crop</param>
        /// <param name="c1">The scaling factor for the crop price</param>
        /// <param name="c2">Offset for supply/demand price factor</param>
        /// <param name="s">Scaling factor for supply changes</param>
        /// <param name="d">Scaling factor for demand changes</param>
        public CropMarketData(float baseSupply, float baseDemand, float c1, float c2, float s, float d)
        {
            this.baseSupply = baseSupply;
            this.currentSupply = baseSupply;
            this.baseDemand = baseDemand;
            this.currentDemand = baseDemand;

            this.c1 = c1;
            this.c2 = c2;
            this.s = s;
            this.d = d;

            this.timesBought = 0;
            this.timesSold = 0;

            updatePrices();
        }

        /////Getter Methods
        
        /// <summary>
        /// Get the current buy price of the crop.
        /// </summary>
        /// <returns>Current cost of buying crop</returns>
        public float getBuyPrice()
        {
            return currentBuyPrice;
        }

        /// <summary>
        /// Get the current sell price of the crop.
        /// </summary>
        /// <returns>Current value of selling crop</returns>
        public float getSellPrice()
        {
            return currentSellPrice;
        }


        /////Public Methods
        
        /// <summary>
        /// Update the supply and demand of the crop after having purchased some of it. Does not update the price.
        /// </summary>
        /// <param name="amount">The amount of the crop that was bought</param>
        public void cropBuyUpdate(int amount)
        {
            this.currentDemand += this.d * amount;

            timesBought += 1;
        }

        /// <summary>
        /// Update the supply and demand of the crop after having sold some of it. Does not update the price.
        /// </summary>
        /// <param name="amount">The amount of the crop that was sold</param>
        public void cropSellUpdate(int amount)
        {
            this.currentSupply += this.s * amount;

            timesSold += 1;
        }

        /// <summary>
        /// Performs updates to market data that occur between turns, including updating the price.
        /// </summary>
        public void marketTurnUpdate()
        {
            this.currentSupply += (this.baseSupply - this.currentSupply) * Market.supplyResetFactor;

            updatePrices();
        }


        /////Private Methods

        /// <summary>
        /// Recalculates the current buy and sell price of the crop.
        /// </summary>
        private void updatePrices()
        {
            float priceDenominator = Mathf.Max((this.c2 - this.currentDemand + this.currentSupply), 0.001f);
            currentSellPrice = this.c1 / priceDenominator;
            currentBuyPrice = currentSellPrice * Market.buyPriceFactor;
        }
    }



    /////Public Methods

    /// <summary>
    /// Get the current buy price of a crop.
    /// </summary>
    /// <param name="cropName">The name of the crop; must be listed by GameFlowController</param>
    /// <returns>The crop's current buy price</returns>
    public float getBuyPrice(string cropName)
    {
        return cropToData[cropName].getBuyPrice();
    }

    /// <summary>
    /// Get the current sell pricce of a crop.
    /// </summary>
    /// <param name="cropName">The name of the crop; must be listed by GameFlowController</param>
    /// <returns>The crop's current sell price</returns>
    public float getSellPrice(string cropName)
    {
        return cropToData[cropName].getSellPrice();
    }

    //TODO: use floats for money things maybe

    /// <summary>
    /// Buys crops from the market, updating the player's inventory and funds accordingly.
    /// Does nothing if the player cannot afford the purchase.
    /// </summary>
    /// <param name="cropName">The name of the crop; must be listed by GameFlowController</param>
    /// <param name="amount">The amount of the crop to buy</param>
    public void buyCrop(string cropName, int amount)
    {
        int purchasePrice = Mathf.CeilToInt(getBuyPrice(cropName)) * amount;
        if (player.currentMoney >= purchasePrice)
        {
            //take away money
            player.currentMoney -= purchasePrice;

            //update player's inventory (add amount of cropname)
            player.cropInventory[cropName] += amount;
            player.updateInventory();

            //update market
            cropToData[cropName].cropBuyUpdate(amount);
        }
        else
        {
            Debug.Log("not enough money");
        }
    }

    /// <summary>
    /// Sells crops to the market, updating the player's inventory and funds accordingly.
    /// Does nothing if the player does not have enough crops.
    /// </summary>
    /// <param name="cropName">The name of the crop; must be listed by GameFlowController</param>
    /// <param name="amount">The amount of the crop to sell</param>
    public void sellCrop(string cropName, int amount)
    {
        if (player.cropInventory[cropName] >= amount)
        {
            //update player's inventory (remove amount of cropname)
            player.cropInventory[cropName] -= amount;
            player.updateInventory();

            //give money
            int salePrice = Mathf.CeilToInt(getSellPrice(cropName)) * amount;
            player.currentMoney += salePrice;

            //update market
            cropToData[cropName].cropSellUpdate(amount);
        }
        else
        {
            Debug.Log("not enough crop");
        }
    }

    /// <summary>
    /// Performs inter-turn updates for each crop, particularly including updating their prices.
    /// </summary>
    public void passTurn()
    {
        foreach (CropMarketData cropData in cropToData.Values)
        {
            cropData.marketTurnUpdate();
        }
        marketMenuList.updatePriceText();
    }


    /////Private Methods

    // Start is called before the first frame update
    void Start()
    {
        GameFlowController mainController = FindObjectOfType<GameFlowController>();
        cropToData = new Dictionary<string, CropMarketData>();
        foreach (GameFlowController.Crop crop in mainController.availableCrops)
        {
            CropMarketData cropData = new CropMarketData(crop.baseSupply, crop.baseDemand,
                crop.mVarC1, crop.mVarC2, crop.mVarS, crop.mVarD);
            cropToData.Add(crop.cropName, cropData);
        }

        player = FindObjectOfType<Player>();
        marketMenuList = marketPanel.gameObject.GetComponentInChildren<MarketList>();
    }
}
