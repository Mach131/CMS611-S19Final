using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the state of the player, in particular their possessions and finances.
/// </summary>
public class Player : MonoBehaviour
{
    public int currentMoney;
    public int currentDebt;
    public Dictionary<string, int> cropInventory = new Dictionary<string, int>();
}
