using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Arcade_Game : Game
{
    private void Start()
    {
        Setup();
    }

    private void Update()
    {
        BaseGameUpdate();
    }

    public override void CheckFailed()
    {
        if (ratings <= 0f)
        {
            normalCheckFailed();
            SpawnGameOver("Arcade_Gameover");
        }
    }

    public void QuitGame()
    {
        Main mainScr = GameObject.Find("main").GetComponent<Main>();
        mainScr.nextSceneName = "Title";
        SceneManager.LoadScene("Loading", LoadSceneMode.Single);
    }
}
