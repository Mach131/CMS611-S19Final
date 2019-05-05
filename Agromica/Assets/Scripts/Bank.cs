using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using System.Globalization;

public class Bank : MonoBehaviour
{
    public float interestRate;
    public Image bankPanel;
    public Text loanInput;
    public Text payInput;
    public TextMeshProUGUI currentDebt;
    private Player player;

    void Awake()
    {
        player = FindObjectOfType<Player>();
        interestRate = .1f;
    }

    void OnEnable()
    {
        UpdateDebtDisplay();
    }

    public void takeLoan()
    {
        string input = loanInput.text;
        int amount = 0;
        if (int.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out amount)) {
            amount = int.Parse(input);
            if (amount < 10000 && amount > 0)
            {
                player.currentDebt += amount;
                player.currentMoney += amount;
                player.updateInventory();
                UpdateDebtDisplay();
            }
        } else {
            Debug.Log("Cannot parse integer: " + input);
        }   
    }

    public void payLoan()
    {
        string input = payInput.text;
        int amount = 0;
        if (int.TryParse(input, NumberStyles.Integer, CultureInfo.InvariantCulture, out amount))
        {
            amount = int.Parse(input);
            if (amount < 10000 && amount > 0 && amount <= player.currentMoney && amount <= player.currentDebt)
            {
                player.currentDebt -= amount;
                player.currentMoney -= amount;
                player.updateInventory();
                UpdateDebtDisplay();
            }
        } else {
            Debug.Log("Cannot parse integer: " + input);
        }
    }

    private GameObject findInputText(string textName)
    {
        // This transform should come from the menu for the bank
        // CurrentDebt is the text field for current debt. 
        // LoanInput is the text field for taking a loan
        // PaymentInput is the text field for paying a loan
        Transform[] ts = bankPanel.gameObject.GetComponentsInChildren<Transform>();
        GameObject timeLeft = null;
        foreach (Transform child in ts)
        {
            // Debug.Log(child.name);
            if (child.name.Equals(textName))
                timeLeft = child.gameObject;
        }
        return timeLeft;
    }

    public void CompoundInterest(int turn)
    {
        float P = player.currentDebt;
        float r = interestRate;
        Debug.Log(r);
        float n = 1f; //Was 12
        float t = turn / n;
        float bottom = (1 + r / n);
        Debug.Log(bottom);
        float exp = Mathf.Round(P * (float)Math.Pow(bottom,1));
        Debug.Log(exp);
        player.currentDebt = exp;
        UpdateDebtDisplay();
    }

    public void UpdateDebtDisplay() 
    {
        currentDebt.text = "Current Debt: " + player.currentDebt.ToString();
    }
}
