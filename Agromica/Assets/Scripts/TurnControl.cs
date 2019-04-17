using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnControl : MonoBehaviour
{
    public static List<GameObject> PLOTS;
    public static int TURN = 0;
    public static int BLUE_OWNED = 0;
    public static int RED_OWNED = 0;
    // Start is called before the first frame update
    void Start()
    {
        PLOTS = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateTurn()
    {
        // Need to update turn counter
        // Need to update market
        // Need to grow crops
        TurnControl.TURN += 1;
        Debug.Log("Next turn");

        while (TurnControl.PLOTS.Count > 0)
        {
            Plot plot = TurnControl.PLOTS[0].GetComponent<Plot>();
            int item = plot.Harvest();
            Debug.Log(item);
            TurnControl.PLOTS.Remove(plot.gameObject);
        }
    }
}
