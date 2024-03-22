using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arcade_Game : Game
{
    private void Start()
    {
        Setup();
    }

    private void FixedUpdate()
    {
        BaseGameUpdate();
    }
}
