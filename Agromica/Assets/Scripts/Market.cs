using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the market, tracking the current prices of crops and the variables influencing them.
/// Currently heavily assumes that prices are not meant to change until the end of a turn.
/// </summary>
public class Market : MonoBehaviour
{
    private Dictionary<string, CropMarketData> cropToData;
    private Player player;

    /// <summary>
    /// Contains relevant market information for a single crop, and provides ways to update it
    /// </summary>
    private class CropMarketData
    {
        private float baseSupply;
        private float currentSupply;
        private float baseDemand;
        private float currentDemand;
        private int timesBought;
        private int timesSold;

        private float currentBuyPrice;
        private float currentSellPrice;

        /////Constructor

        /// <summary>
        /// Initializes market data for a crop.
        /// </summary>
        /// <param name="baseSupply">The base supply for the crop</param>
        /// <param name="baseDemand">The base demand for the crop</param>
        public CropMarketData(float baseSupply, float baseDemand)
        {
            this.baseSupply = baseSupply;
            this.currentSupply = baseSupply;
            this.baseDemand = baseDemand;
            this.currentDemand = baseDemand;

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
            //TODO: implement

            timesBought += 1;
        }

        /// <summary>
        /// Update the supply and demand of the crop after having sold some of it. Does not update the price.
        /// </summary>
        /// <param name="amount">The amount of the crop that was sold</param>
        public void cropSellUpdate(int amount)
        {
            //TODO: implement

            timesSold += 1;
        }

        /// <summary>
        /// Performs updates to market data that occur between turns, including updating the price.
        /// </summary>
        public void marketTurnUpdate()
        {
            //TODO: other changes to supply/demand

            updatePrices();
        }


        /////Private Methods

        /// <summary>
        /// Recalculates the current buy and sell price of the crop.
        /// </summary>
        private void updatePrices()
        {
            //TODO: not constant
            currentBuyPrice = 100;
            currentSellPrice = 100;
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

    /// <summary>
    /// Buys crops from the market, updating the player's inventory and funds accordingly.
    /// Does nothing if the player cannot afford the purchase.
    /// </summary>
    /// <param name="cropName">The name of the crop; must be listed by GameFlowController</param>
    /// <param name="amount">The amount of the crop to buy</param>
    public void buyCrop(string cropName, int amount)
    {
        float purchasePrice = getBuyPrice(cropName) * amount;
        if (player.currentMoney >= purchasePrice)
        {
            //TODO: take away money (should probably make player's funds/debt into floats)

            //TODO: update player's inventory (add amount of cropname)

            //update market
            cropToData[cropName].cropBuyUpdate(amount);
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
            float salePrice = getSellPrice(cropName) * amount;

            //TODO: give money (should probably make player's fund/debt into floats)

            //TODO: update player's inventory (remove amount of cropname)

            //update market
            cropToData[cropName].cropSellUpdate(amount);
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
    }


    /////Private Methods

    // Start is called before the first frame update
    void Start()
    {
        GameFlowController mainController = FindObjectOfType<GameFlowController>();
        cropToData = new Dictionary<string, CropMarketData>();
        foreach (GameFlowController.Crop crop in mainController.availableCrops)
        {
            CropMarketData cropData = new CropMarketData(crop.baseSupply, crop.baseDemand);
            cropToData.Add(crop.cropName, cropData);
        }

        player = FindObjectOfType<Player>();
    }
}
