using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServingTray : MonoBehaviour
{
    public Game game;
    public bool isClickable = true;

    private void OnMouseDown()
    {
        if (isClickable)
        {
            game.ServingTrayClicked();
        }
    }

    private void OnMouseOver()
    {
        if (isClickable)
        {
            game.ShowServingTrayTooltip();
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
