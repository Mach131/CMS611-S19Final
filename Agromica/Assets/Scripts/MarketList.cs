using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Item
{
    public string itemName;
    public Sprite icon;
    public string buyPrice;
    public string sellPrice;
}

public class MarketList : MonoBehaviour
{

    public List<Item> itemList;
    public Transform contentPanel;
    public SimpleObjectPool objectPool;

    // Use this for initialization
    void Start()
    {
        RefreshDisplay();
    }

    void RefreshDisplay()
    {
        // myGoldDisplay.text = "Gold: " + gold.ToString();
        RemoveEntries();
        AddEntries();
    }

    private void RemoveEntries()
    {
        while (contentPanel.childCount > 0)
        {
            GameObject toRemove = transform.GetChild(0).gameObject;
            objectPool.ReturnObject(toRemove);
        }
    }

    private void AddEntries()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            Item item = itemList[i];
            GameObject obj = objectPool.GetObject();
            obj.transform.SetParent(contentPanel, false);

            MarketEntry entry = obj.GetComponent<MarketEntry>();
            entry.Setup(item, this);
        }
    }

    private void AddItem(Item itemToAdd, MarketList market)
    {
        market.itemList.Add(itemToAdd);
    }

    private void RemoveItem(Item itemToRemove, MarketList market)
    {
        for (int i = market.itemList.Count - 1; i >= 0; i--)
        {
            if (market.itemList[i] == itemToRemove)
            {
                market.itemList.RemoveAt(i);
            }
        }
    }
}
