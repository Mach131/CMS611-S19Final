using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlotPurchasePanel : MonoBehaviour
{
    private TextMeshProUGUI message;
    
    void Awake()
    {
        message = transform.Find("Text Blurb").GetComponent<TextMeshProUGUI>();
    }

    void OnEnable()
    {
        message.text = string.Format("Purchase this plot?\n{0} rupees", Plot.plotPrice);
    }

}
