using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oven : MonoBehaviour
{
    public Game game;
    public bool canClickDoors = true;
    public bool isCooking = false;
    public bool isClickable = true;

    public Sprite OvenOpenSprite;
    public Sprite OvenCloseSprite;

    private void OnMouseDown()
    {
        if (isClickable)
        {
            game.OvenClicked(name);
        }
    }

    private void OnMouseOver()
    {
        if (isClickable)
        {
            game.ShowOvenToolTip(name);
        }
    }

    private void OnMouseExit()
    {
        game.HideTooltip();
    }

    public void ChangeSpriteOpen(bool boolValue)
    {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (boolValue)
        {
            // oven door open
            sr.sprite = OvenOpenSprite;
        }
        else
        {
            // oven door closed
            sr.sprite = OvenCloseSprite;
        }
    }

    public void SetClickable(bool boolValue)
    {
        isClickable = boolValue;
    }
}
