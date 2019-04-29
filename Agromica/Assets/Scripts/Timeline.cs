using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class Turn
{
    public string number;
    public bool hasQuota;
    public string count1;
    public string count2;
    public string count3;
}
public class Timeline : MonoBehaviour
{
    public List<Turn> turnList;
    public Transform contentPanel;
    public SimpleObjectPool objectPool;

    // Use this for initialization
    void Start()
    {
        RefreshDisplay();
    }

    void RefreshDisplay()
    {
        // myGoldDisplay.text = "Gold: " + gold.ToString();
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
        for (int i = 0; i < turnList.Count; i++)
        {
            Turn turn = turnList[i];
            GameObject obj = objectPool.GetObject();
            obj.transform.SetParent(contentPanel, false);

            TimelineEntry entry = obj.GetComponent<TimelineEntry>();
            entry.Setup(turn, this);
        }
    }

    private void AddTurn(Turn turnToAdd, Timeline timeline)
    {
        timeline.turnList.Add(turnToAdd);
    }

    private void RemoveTurn(Turn turnToRemove, Timeline timeline)
    {
        for (int i = timeline.turnList.Count - 1; i >= 0; i--)
        {
            if (timeline.turnList[i] == turnToRemove)
            {
                timeline.turnList.RemoveAt(i);
            }
        }
    }
}