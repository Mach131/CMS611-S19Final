using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SeedEntry : MonoBehaviour
{
    public TextMeshProUGUI cropName;
    public TextMeshProUGUI turnsToGrow;
    public Image icon;

    private SeedScroller scroller;
    private GameFlowController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = FindObjectOfType<GameFlowController>();
        this.gameObject.GetComponent<Button>().onClick.AddListener(selectSeed);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void selectSeed()
    {
        controller.lastPlotToClick.Plant(cropName.text);
    }

    public void Setup(GameFlowController.Crop crop, SeedScroller currentScroller)
    {
        cropName.text = crop.cropName;
        turnsToGrow.text = string.Format("{0} turns", crop.turnsToGrow.ToString());
        icon.sprite = Resources.Load<Sprite>(crop.iconResourcePath);
        scroller = currentScroller;
    }

}
