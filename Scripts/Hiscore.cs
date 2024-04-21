using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hiscore : MonoBehaviour
{
    private float timeToDestory = 5f;
    private float timeToGrow = 1f;
    private float currentTime = 0f;

    private bool isRunning = true;

    private RectTransform rect;

    // Start is called before the first frame update
    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning)
        {
            currentTime += Time.deltaTime;
            if (currentTime >= timeToDestory)
            {
                Destroy(gameObject);
                return;
            }
            if (currentTime < timeToGrow)
            {
                rect.localScale = new Vector3(currentTime, currentTime, currentTime);
            }
            else
            {
                rect.localScale = new Vector3(1f, 1f, 1f);
            }
        }
    }
}
