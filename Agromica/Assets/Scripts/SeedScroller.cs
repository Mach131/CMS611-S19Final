using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SeedScroller : MonoBehaviour
{
    public Transform contentPanel;
    public SimpleObjectPool objectPool;

    private HashSet<SeedEntry> currentEntries = new HashSet<SeedEntry>();
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
            currentEntries.Remove(toRemove.GetComponent<SeedEntry>());
            objectPool.ReturnObject(toRemove);
        }
    }

    private void AddEntries()
    {
        for (int i = 0; i < controller.availableCrops.Count; i++)
        {
            GameFlowController.Crop crop = controller.availableCrops[i];
            GameObject obj = objectPool.GetObject();
            obj.transform.SetParent(contentPanel, false);

            SeedEntry seedEntry = obj.GetComponent<SeedEntry>();
            seedEntry.Setup(crop, this);
            currentEntries.Add(seedEntry);
        }
    }
}
