using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MarketEntry : MonoBehaviour
{
    public Image icon;
    public Button buyButton;
    public Button sellButton;
    public TextMeshProUGUI buyPrice;
    public TextMeshProUGUI sellPrice;

    private GameFlowController.Crop crop;
    private Market market;
    
    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update() {}

    public void Setup(GameFlowController.Crop currentCrop)
    {
        crop = currentCrop;
        market = FindObjectOfType<Market>();

        buyButton.onClick.AddListener(() => market.buyCrop(crop.cropName, 1));
        sellButton.onClick.AddListener(() => market.sellCrop(crop.cropName, 1));

        icon.sprite = Resources.Load<Sprite>(crop.iconResourcePath);
    }

    public void updatePrices()
    {
        //TODO: maybe not integers later

        buyPrice.text = Mathf.CeilToInt(market.getBuyPrice(crop.cropName)).ToString();
        sellPrice.text = Mathf.CeilToInt(market.getSellPrice(crop.cropName)).ToString();
    }
}
