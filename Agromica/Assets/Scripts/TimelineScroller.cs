using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimelineScroller : MonoBehaviour
{
    public Transform contentPanel;
    public SimpleObjectPool objectPool;

    private GameFlowController controller;

    // Use this for initialization
    void Start()
    {
        controller = FindObjectOfType<GameFlowController>();
        RefreshDisplay();
    }

    void RefreshDisplay()
    {
        RemoveEntries();
        AddEntries();
    }

    private void RemoveEntries()
    {
        while (contentPanel.childCount > 0)
        {
            GameObject toRemove = transform.GetChild(0).gameObject;
            objectPool.ReturnObject(toRemove);
        }
    }

    private void AddEntries()
    {
        for (int i = 0; i < (controller.numberOfRounds + 1); i++)
        {
            int turnNumber = i;
            GameFlowController.Quota quota = null;
            controller.turnToQuota.TryGetValue(turnNumber, out quota);

            GameObject obj = objectPool.GetObject();
            obj.transform.SetParent(contentPanel, false);

            TimelineEntry entry = obj.GetComponent<TimelineEntry>();
            entry.Setup(turnNumber, quota, this);
        }
    }
}