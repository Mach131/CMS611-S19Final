using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerStatDisplay : MonoBehaviour
{
    private Player player;
    [SerializeField]
    private TextMeshProUGUI money, debt, inventory;

    void Awake()
    {
        player = FindObjectOfType<Player>();
    }

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
            inventory.text += string.Format("{0} (x{1})\n", kvp.Key, kvp.Value);
        }
    }
}
