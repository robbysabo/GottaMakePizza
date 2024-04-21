using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TrashCan : MonoBehaviour
{
    public Game game;
    public bool isClickable = true;
    private void OnMouseDown()
    {
        if (isClickable)
        {
            game.TrashCanClicked();
        }
    }

    private void OnMouseOver()
    {
        if (isClickable)
        {
            game.ShowTooltipTrash();
        }
    }

    private void OnMouseExit()
    {
        game.HideTooltip();
    }

    public void SetClickable(bool boolValue)
    {
        isClickable = boolValue;
    }
}
