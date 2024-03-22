using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TrashCan : MonoBehaviour
{
    public Game theParent;
    public bool isClickable = true;
    private void OnMouseDown()
    {
        if (isClickable)
        {
            theParent.TrashCanClicked();
        }
    }

    /*private void OnMouseOver()
    {
        if (isClickable)
        {
            theParent.ShowTooltip("Trash");
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
