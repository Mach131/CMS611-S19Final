using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Skinned UI Data")]
public class SkinUIData : ScriptableObject
{
    public Sprite buttonSprite;
    public SpriteState buttonSpriteState;

    public Color defaultColor;
    public Color confirmColor;
    public Color declineColor;
    public Color warningColor;
}