using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearToDark : MonoBehaviour
{
    public bool canRun = false;
    public string nextSceneName = string.Empty;

    float maxTime = 2f;
    float currentTime = 0;

    void Update()
    {
        if (canRun)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= maxTime)
            {
                canRun = false;
                SceneManager.LoadScene(nextSceneName,LoadSceneMode.Single);
            }
        }
    }
}
