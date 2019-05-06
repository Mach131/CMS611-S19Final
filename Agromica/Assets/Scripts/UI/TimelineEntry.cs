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
    private GameFlowController.Quota quota;

    // Start is called before the first frame update
    void Start()
    {
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
}
