using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickVoid : MonoBehaviour
{
    public Game theParent;

    private void OnMouseDown()
    {
        theParent.VoidClicked();
    }
}
