using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MarketEntry : MonoBehaviour
{
    public Button buyButton;
    public Button sellButton;
    public TextMeshProUGUI buyPrice;
    public TextMeshProUGUI sellPrice;

    private Item item;
    private MarketList market;    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Setup(Item currentItem, MarketList currentMarket)
    {
        item = currentItem;
        buyPrice.text = item.buyPrice.ToString();
        sellPrice.text = item.sellPrice.ToString();

        market = currentMarket;
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
