using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paused : MonoBehaviour
{
    public Game game;

    public void ArcadePause()
    {
        game.PauseGame();
    }
}
