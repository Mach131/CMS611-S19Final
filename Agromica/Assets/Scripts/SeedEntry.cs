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

    // Start is called before the first frame update
    void Start()
    {  
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void Setup(GameFlowController.Crop crop, SeedScroller currentScroller)
    {
        cropName.text = crop.cropName;
        turnsToGrow.text = string.Format("{0} turns", crop.turnsToGrow.ToString());
        icon.sprite = Resources.Load<Sprite>(crop.iconResourcePath);
        scroller = currentScroller;
    }

}
