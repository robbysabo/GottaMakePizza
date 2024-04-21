using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickVoid : MonoBehaviour
{
    public Game game;

    private void OnMouseDown()
    {
        game.VoidClicked();
    }
}
