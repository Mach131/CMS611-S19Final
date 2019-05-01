using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MarketList : MonoBehaviour
{
    public List<GameFlowController.Crop> cropList;
    public Transform contentPanel;
    public SimpleObjectPool objectPool;

    private HashSet<MarketEntry> currentEntries = new HashSet<MarketEntry>();
    private GameFlowController controller;

    // Use this for initialization
    void Start()
    {
        controller = FindObjectOfType<GameFlowController>();
        cropList = controller.availableCrops;

        RefreshDisplay();
        updatePriceText();
    }

    void RefreshDisplay()
    {
        RemoveEntries();
        AddEntries();
    }

    /// <summary>
    /// Updates the prices shown by all currently active market entries
    /// </summary>
    public void updatePriceText()
    {
        foreach (MarketEntry entry in currentEntries)
        {
            entry.updatePrices();
        }
    }

    private void RemoveEntries()
    {
        while (contentPanel.childCount > 0)
        {
            GameObject toRemove = transform.GetChild(0).gameObject;
            currentEntries.Remove(toRemove.GetComponent<MarketEntry>());
            objectPool.ReturnObject(toRemove);
        }
    }

    private void AddEntries()
    {
        for (int i = 0; i < cropList.Count; i++)
        {
            GameFlowController.Crop crop = cropList[i];
            GameObject obj = objectPool.GetObject();
            obj.transform.SetParent(contentPanel, false);

            MarketEntry entry = obj.GetComponent<MarketEntry>();
            entry.Setup(crop);
            currentEntries.Add(entry);
        }
    }
}
