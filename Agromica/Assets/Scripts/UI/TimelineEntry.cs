using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimelineEntry : MonoBehaviour
{
    public TextMeshProUGUI turnNumber;
    public Transform quotaPanel;

    private TimelineScroller scroller;
    private GameFlowController controller;
    private GameFlowController.Quota quota;

    private Image turnImage;
    private Image quotaImage;

    private Color defaultTurnBGColor;
    private Color altTurnBGColor;
    private Color defaultQuotaBGColor;
    private Color altQuotaBGColor;

    void Awake()
    {
        controller = FindObjectOfType<GameFlowController>();
        turnImage = this.transform.Find("Turn").gameObject.GetComponent<Image>();
        quotaImage = this.transform.Find("Quota").gameObject.GetComponent<Image>();

        defaultTurnBGColor = Color.black;
        defaultQuotaBGColor = new Color32(0xD4, 0xD4, 0xD4, 0xFF);
    
        altTurnBGColor = new Color32(0x88, 0xBA, 0x4D, 0xFF);
        altQuotaBGColor = new Color32(0xA0, 0xA0, 0xA0, 0xFF);
    }

    void Update()
    {
        PaletteSwap();
    }

    public void Setup(int currentTurnNumber, GameFlowController.Quota currentQuota, TimelineScroller currentScroller)
    {
        turnNumber.text = currentTurnNumber.ToString();
        quota = currentQuota;

        if (quota == null)
        {
            quotaPanel.gameObject.SetActive(false);
        }
        else
        {
            quotaPanel.gameObject.SetActive(true);
            quotaPanel.GetComponent<QuotaScroller>().Setup(quota);
        }

        scroller = currentScroller;
    }

    public void PaletteSwap()
    {
        if (controller.currentTurn == int.Parse(turnNumber.text))
        {
            turnImage.color = altTurnBGColor;
            turnNumber.color = Color.black;
            quotaImage.color = altQuotaBGColor;
        }

        else
        {
            turnImage.color = defaultTurnBGColor;
            turnNumber.color = Color.white;
            quotaImage.color = defaultQuotaBGColor;
        }
    }
}
