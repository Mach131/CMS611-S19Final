using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoanBorrowButton : MonoBehaviour
{
    private Button button;
    private Bank bankScript;

    void Awake()
    {
        button = gameObject.GetComponent<Button>();
        bankScript = FindObjectOfType<Bank>();
        button.onClick.AddListener(() => bankScript.takeLoan());
    }
}
