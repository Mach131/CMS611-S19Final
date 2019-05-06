using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CropEntry : MonoBehaviour
{
    public TextMeshProUGUI count;
    public Image icon;

    private GameFlowController controller;
    private QuotaScroller scroller;

    // Start is called before the first frame update
    void Start()
    {
    }

    void Update() {}

    public void Setup(GameFlowController.Quota.Requirement req, QuotaScroller currentScroller)
    {
        controller = FindObjectOfType<GameFlowController>();
        GameFlowController.Crop crop = controller.cropLookup(req.cropName);
        count.text = req.requiredAmount.ToString();
        icon.sprite = Resources.Load<Sprite>(crop.iconResourcePath);
        scroller = currentScroller;
    }
}

