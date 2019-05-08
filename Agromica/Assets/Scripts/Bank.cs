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
    
    private Text currentDebt;

    private InputField loanInput, payInput;
    private Player player;
    private BankPanel bankPanel;

    void Awake()
    {
        player = FindObjectOfType<Player>();
        bankPanel = FindObjectOfType<BankPanel>();
        loanInput = FindObjectOfType<LoanBorrowInputField>().gameObject.GetComponent<InputField>();
        payInput = FindObjectOfType<LoanRepayInputField>().gameObject.GetComponent<InputField>();
        interestRate = .1f;

        currentDebt = bankPanel.transform.Find("Player Debt").GetComponent<Text>();
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
