using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatDisplay : MonoBehaviour
{
    public Player player;
    public TextMeshProUGUI money;
    public TextMeshProUGUI debt;
    public TextMeshProUGUI inventory;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Refresh();
    }

    private void Refresh()
    {
        money.text = string.Format("Money: {0} rupees", player.currentMoney.ToString());
        debt.text = string.Format("Debt: {0} rupees", player.currentDebt.ToString());
        inventory.text = "Crop Inventory\n";
        foreach (KeyValuePair<string, int> kvp in player.cropInventory)
        {
            inventory.text += string.Format("{1} {0}\n", kvp.Key, kvp.Value);
        }
    }
}
