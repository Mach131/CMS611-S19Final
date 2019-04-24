using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Represents the state of the player, in particular their possessions and finances.
/// </summary>
public class Player : MonoBehaviour
{
    public float currentMoney;
    public float currentDebt;
    public Text inventoryDisplay;
    public Dictionary<string, int> cropInventory = new Dictionary<string, int>();

    public List<Text> Inventory;

    private void Start()
    {
        updateInventory();
    }

    public void updateInventory()
    {
        inventoryDisplay.text = "";
        foreach(string cropName in cropInventory.Keys)
        {
            inventoryDisplay.text = inventoryDisplay.text + cropName + 
                        ": " + cropInventory[cropName].ToString() + "\n";
        }
    }
}
