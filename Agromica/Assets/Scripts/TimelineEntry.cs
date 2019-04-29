using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimelineEntry : MonoBehaviour
{
    public TextMeshProUGUI turnNumber;
    public Transform quota;
    public TextMeshProUGUI crop1Count;
    public TextMeshProUGUI crop2Count;
    public TextMeshProUGUI crop3Count;

    private Turn turn;
    private Timeline timeline;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
     
    }

    public void Setup(Turn currentTurn, Timeline currentTimeline)
    {
        turn = currentTurn;

        turnNumber.text = turn.number.ToString();
        GameObject qp = quota.gameObject;
        qp.SetActive(turn.hasQuota);
        crop1Count.text = turn.count1.ToString();
        crop2Count.text = turn.count2.ToString();
        crop3Count.text = turn.count3.ToString();

        timeline = currentTimeline;
    }
}
