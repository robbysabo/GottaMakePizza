using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseTimer : MonoBehaviour
{
    [Header("For Parent")]
    public Game game;
    public float timeLeft = 10f;
    public float maxTime = 10f;
    public string callTimeoutMethodName;

    public bool isRunning = false;

    // Update is called once per frame
    void Update()
    {
        OnUpdate();
    }

    virtual public void OnUpdate()
    {
        if (timeLeft <= 0f || !isRunning)
        {
            return;
        }

        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0f)
        {
            game.StartCoroutine(callTimeoutMethodName, name);
            Destroy(gameObject);
        }
    }

    public void StopAndDestroy()
    {
        Destroy(gameObject);
    }

    public void StartTimer()
    {
        isRunning = true;
    }
}
