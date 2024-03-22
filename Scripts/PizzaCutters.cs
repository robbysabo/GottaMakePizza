using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaCutters : MonoBehaviour
{
    public Game theParent;
    public bool isSelected = false;
    public bool isClickable = true;

    private string texturePath = "Art/SpriteSheet";

    private void OnMouseDown()
    {
        if (isClickable)
        {
            theParent.PizzaCutterClicked();
        }
    }

    private void OnMouseOver()
    {
        if (isClickable) 
        {
            string msg = "Select Pizza Cutter";
            if (isSelected)
            {
                msg = "Unselect Pizza Cutter";
            }

            theParent.ShowTooltip(msg);
        }
    }

    private void OnMouseExit()
    {
        theParent.HideTooltip();
    }

    public void UpdateSprite()
    {
        SpriteRenderer spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        Sprite getSprite = GetSpriteByName();
        spriteRenderer.sprite = getSprite;
    }

    private Sprite GetSpriteByName()
    {
        string currentName = "pizza_cutter";

        if (isSelected)
        {
            currentName += "_selected";
        }

        Sprite[] sprites = Resources.LoadAll<Sprite>(texturePath);
        int findSprite = Array.FindIndex(sprites, s => s.name == currentName);
        return sprites[findSprite];
    }

    public void SetClickable(bool boolValue)
    {
        isClickable = boolValue;
    }
}
