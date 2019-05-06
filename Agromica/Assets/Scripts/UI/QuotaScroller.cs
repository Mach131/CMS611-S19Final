using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuotaScroller : MonoBehaviour
{
    public Transform contentPanel;
    public SimpleObjectPool objectPool;

    private HashSet<CropEntry> currentEntries = new HashSet<CropEntry>();
    private GameFlowController controller;
    private GameFlowController.Quota quota;

    // Use this for initialization
    void Start()
    {
    }

    void RefreshDisplay()
    {
        RemoveEntries();
        AddEntries();
    }

    public void Setup(GameFlowController.Quota currentQuota)
    {
        controller = FindObjectOfType<GameFlowController>();
        quota = currentQuota;
        RefreshDisplay();
    }

    private void RemoveEntries()
    {
        while (contentPanel.childCount > 0)
        {
            GameObject toRemove = transform.GetChild(0).gameObject;
            currentEntries.Remove(toRemove.GetComponent<CropEntry>());
            objectPool.ReturnObject(toRemove);
        }
    }

    private void AddEntries()
    {
        for (int i = 0; i < quota.cropRequirements.Count; i++)
        {
            GameFlowController.Quota.Requirement req = quota.cropRequirements[i];
            GameObject obj = objectPool.GetObject();
            obj.transform.SetParent(contentPanel, false);

            CropEntry cropEntry = obj.GetComponent<CropEntry>();
            cropEntry.Setup(req, this);
            currentEntries.Add(cropEntry);
        }
    }
}
