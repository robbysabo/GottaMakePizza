using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodContainer : MonoBehaviour
{
    public Game theParent;
    public bool isSelected = false;
    public string foodName = "";
    public bool isClickable = true;

    private string texturePath = "Art/SpriteSheet";

    private void OnMouseDown()
    {
        if (isClickable)
        {
            theParent.FoodClicked(foodName);
        }
    }

    private void OnMouseOver()
    {
        if (isClickable)
        {
            string msg = "Select " + foodName;
            if (isSelected)
            {
                msg = "Unselect " + foodName;
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
        string currentName = foodName + "_container";

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
