using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServingTray : MonoBehaviour
{
    public Game theParent;
    public bool isClickable = true;

    private void OnMouseDown()
    {
        if (isClickable)
        {
            theParent.ServingTrayClicked();
        }
    }

    /*private void OnMouseOver()
    {
        if (isClickable)
        {
            theParent.ShowServingTrayTooltip();
        }
    }

    private void OnMouseExit()
    {
        theParent.HideTooltip();
    }*/

    public void SetClickable(bool boolValue)
    {
        isClickable = boolValue;
    }
}
