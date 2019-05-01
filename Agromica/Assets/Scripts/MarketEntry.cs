using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MarketEntry : MonoBehaviour
{
    public Image icon;
    public Button buyButton;
    public Button sellButton;
    public TextMeshProUGUI buyPrice;
    public TextMeshProUGUI sellPrice;

    private GameFlowController.Crop crop;
    private Market market;
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Setup(GameFlowController.Crop currentCrop)
    {
        crop = currentCrop;
        market = FindObjectOfType<Market>();

        buyButton.onClick.AddListener(() => market.buyCrop(crop.cropName, 1));
        sellButton.onClick.AddListener(() => market.sellCrop(crop.cropName, 1));

        icon.sprite = Resources.Load<Sprite>(crop.iconResourcePath);
    }

    public void updatePrices()
    {
        //TODO: maybe not integers later

        buyPrice.text = Mathf.CeilToInt(market.getBuyPrice(crop.cropName)).ToString();
        sellPrice.text = Mathf.CeilToInt(market.getSellPrice(crop.cropName)).ToString();
    }

    // public void Buy(Player player, Item item)
    // {
    //     // If player has money, subtract funds and add Item to their inventory.
    //     if (player.currentMoney >= item.buyPrice) {
    //         player.currentMoney -= item.buyPrice;
    //         player.cropInventory.AddorUpdate(item.id, 1, (id, count) => count + 1);
    //     }
    // }

    // public void Sell(Player player, Item item)
    // {
    //     // Subtract item (if it exists) from player inventory and add money.
    //     if (player.cropInventory.TryGetValue(item.id) > 0) {
    //         player.currentMoney += item.sellPrice;
    //         player.cropInventory.AddorUpdate(id, 0, (id, count) => count - 1);
    //     }
    // }
}
