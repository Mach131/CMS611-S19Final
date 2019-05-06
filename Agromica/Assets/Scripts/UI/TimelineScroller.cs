using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimelineScroller : MonoBehaviour
{
    public RectTransform contentPanel;
    public ScrollRect scrollRect;
    public SimpleObjectPool objectPool;

    private GameFlowController controller;
    private float minScroll;
    private float maxScroll;

    // Use this for initialization
    void Awake()
    {
        controller = FindObjectOfType<GameFlowController>();
        minScroll = contentPanel.rect.yMin;
        maxScroll = contentPanel.rect.yMax;
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

    public void SnapTo(RectTransform target)
    {
        Canvas.ForceUpdateCanvases();
        Vector2 snap =
            (Vector2)scrollRect.transform.InverseTransformPoint(contentPanel.position)
            - (Vector2)scrollRect.transform.InverseTransformPoint(target.position);

        if (snap.y < minScroll)
        {
            snap.y = minScroll;
        } else if (snap.y > maxScroll)
        {
            snap.y = maxScroll;
        }
        contentPanel.anchoredPosition = snap;
    }
}