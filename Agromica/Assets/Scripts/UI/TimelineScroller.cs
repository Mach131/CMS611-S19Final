using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimelineScroller : MonoBehaviour
{
    public ScrollRect scrollRect;
    public SimpleObjectPool objectPool;

    private GameFlowController controller;

    // Use this for initialization
    void Awake()
    {
        controller = FindObjectOfType<GameFlowController>();
    }

    void Start()
    {
        RefreshDisplay();
    }

    void RefreshDisplay()
    {
        RemoveEntries();
        AddEntries();
    }

    private void RemoveEntries()
    {
        while (scrollRect.content.childCount > 0)
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
            obj.transform.SetParent(scrollRect.content, false);

            TimelineEntry entry = obj.GetComponent<TimelineEntry>();
            entry.Setup(turnNumber, quota, this);
        }
    }

    public void SnapTo(RectTransform target)
    {
    }

    public void SnapToCurrent()
    {
        // Push current timeline entry to top of the visible entries.
        TimelineEntry[] timelineEntries = this.GetComponentsInChildren<TimelineEntry>();
        TimelineEntry curTurnEntry = timelineEntries.FirstOrDefault(
            t => int.Parse(t.turnNumber.text) == controller.currentTurn
            );
        if (curTurnEntry != null)
        {
            Debug.Log("Auto-scrolling to current turn's timeline entry.");
            this.SnapTo(curTurnEntry.GetComponent<RectTransform>());
        }
    }
}