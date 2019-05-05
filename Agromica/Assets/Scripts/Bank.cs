using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Bank : MonoBehaviour
{
    public float interestRate;
    private Player player;
    public Image bankPanel;

    // Start is called before the first frame update
    void Start()
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
        GameObject loanInput = findInputText("Loan Input");
        Debug.Log(loanInput.GetComponent<Text>().text);
        string input = loanInput.GetComponent<Text>().text;
        int amount = Int32.Parse(input);
        if (amount < 10000 && amount > 0)
        {
            player.currentDebt += amount;
            player.currentMoney += amount;
            player.updateInventory();
        }
        bankPanel.gameObject.SetActive(false);
    }

    public void payLoan()
    {
        GameObject paymentInput = findInputText("Pay Input");
        Debug.Log(paymentInput.GetComponent<Text>().text);
        string input = paymentInput.GetComponent<Text>().text;
        int amount = Int32.Parse(input);
        if (amount < 10000 && amount > 0 && amount <= player.currentMoney && amount <= player.currentDebt)
        {
            player.currentDebt -= amount;
            player.currentMoney -= amount;
            player.updateInventory();
        }
        bankPanel.gameObject.SetActive(false);
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
        GameObject current = findInputText("Current Debt");
        current.GetComponent<Text>().text = "Current Debt: " + player.currentDebt.ToString();
    }

}
