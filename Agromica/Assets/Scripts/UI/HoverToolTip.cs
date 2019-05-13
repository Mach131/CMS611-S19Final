using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverToolTip : MonoBehaviour
{
    public RectTransform toolTip;
    public Text toolTipText;
 //   public Vector2 cursorOffset;

    private Plot plot;
    private RectTransform plotTransform;

    void Awake()
    {
        HideToolTip();
    }

//    void Update()
//    {
//        Vector3 pos = Input.mousePosition;
//        pos.z = 10.0f; //distance of the plane from the camera
//        pos += (Vector3)cursorOffset;
//        toolTip.position = Camera.main.ScreenToWorldPoint(pos);
//    }

    private void OnDisable()
    {
        plot = null;
        plotTransform = null;
        toolTipText.text = null;
    }

    public void ShowToolTip(Plot parentPlot)
    {
        //Debug.Log("Showing tooltip...");
        toolTip.gameObject.SetActive(true);
        plot = parentPlot;
        plotTransform = plot.gameObject.transform as RectTransform;
        Vector3 pos = plotTransform.localPosition;
        pos.y -= 35 + plotTransform.rect.height/2; // 40 is hardcoded to avoid a glitch I couldn't figure out
        toolTip.localPosition = pos;
        UpdateText();
    }

    public void HideToolTip()
    {
        //Debug.Log("Hiding tooltip...");
        plot = null;
        plotTransform = null;
        toolTipText.text = null;
        toolTip.localPosition = Vector3.zero;
        toolTip.gameObject.SetActive(false);
    }

    private void UpdateText()
    {
        if (plot != null && plot.state != 0)
        {
            int wait = plot.plantedSeed.timeLeft();

            if (wait > 1)
                toolTipText.text = string.Format("{0} turns left...", wait);
            else if (wait == 1)
                toolTipText.text = string.Format("{0} turn left...", wait);
            else
                toolTipText.text = "Ready to harvest!";
        }
    }
}