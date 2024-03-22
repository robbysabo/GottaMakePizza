using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickTable : MonoBehaviour
{
    public Game theParent;
    public bool isClickable = true;
    public Sprite addButton;
    public Sprite pizzaShadow;

    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    private void OnMouseDown()
    {
        if (isClickable)
        {
            theParent.TableClicked();
        }
    }

    /*private void OnMouseOver()
    {
        if (isClickable)
        {
            theParent.ShowTableTooltip();
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

    public void SetSprite(bool isAddButton)
    {
        if (isAddButton)
        {
            sr.sprite = addButton;
        } else
        {
            sr.sprite = pizzaShadow;
        }
    }

    public void SetAddVisible(bool boolValue)
    {
        Color clr = sr.color;
        if (boolValue)
        {
            clr.a = 1;
        } else
        {
            clr.a = 0;
        }
        sr.color = clr;
    }
}
