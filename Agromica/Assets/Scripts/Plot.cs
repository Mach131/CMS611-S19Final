using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{

    public bool isRed = false;
    public bool isBlue = false;
    public Canvas plotMenu;
    // Start is called before the first frame update
    void Start()
    {

    }

    public int Harvest()
    {
        // For now, return 1 for red and 2 for blue
        // Should be an onClick function
        // Needs to be related to self
        if (isRed)
        {
            isRed = false;
            TurnControl.PLOTS.Remove(this.gameObject);
            return 1;
        }
        if (isBlue)
        {
            isBlue = false;
            TurnControl.PLOTS.Remove(this.gameObject);
            return 2;
        }
        return 0;
    }

    public void Plant(int type)
    {
        // Once there is a plant type, this will pass that instead
        if (type == 1)
        {
            TurnControl.PLOTS.Add(this.gameObject);
            this.plotMenu.gameObject.SetActive(false);
            isRed = true;
        }
        else if (type == 2)
        {
            TurnControl.PLOTS.Add(this.gameObject);
            this.plotMenu.gameObject.SetActive(false);
            isBlue = true;
        }
    }

    public void PlotMenu(Canvas plotMenu)
    {
        Debug.Log("I have been clicked");
        if (!(isRed || isBlue)) 
        {
        this.plotMenu = plotMenu;
        this.plotMenu.gameObject.SetActive(true); 
        }
    }
}
