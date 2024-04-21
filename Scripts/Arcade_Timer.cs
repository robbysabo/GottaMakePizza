using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Arcade_Timer : MonoBehaviour
{
    TextMeshProUGUI timerText;
    public float timerTime = 0f;
    public bool canRun = true;

    float theSecondsTime = 0f;
    float theMinutesTime = 0f;
    float theHoursTime = 0f;

    // Start is called before the first frame update
    void Awake()
    {
        timerText = GameObject.Find("TimerText").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (canRun)
        {
            CalcTime();
        }
    }

    private void CalcTime()
    {
        // Calculate time
        timerTime += Time.deltaTime;

        theSecondsTime = Mathf.Floor(timerTime % 60);
        theMinutesTime = Mathf.Floor((timerTime / 60) % 60);
        theHoursTime = Mathf.Floor((timerTime / 60) / 60);

        string theSeconds = FixTimeText(theSecondsTime.ToString());
        string theMinutes = FixTimeText(theMinutesTime.ToString());
        string theHours = theHoursTime.ToString();

        string txt = theHours + ":" + theMinutes + ":" + theSeconds;
        timerText.text = txt;
    }

    private string FixTimeText(string theTime)
    {
        int theTimeLength = theTime.Length;
        string newTime = theTime;

        if (theTimeLength < 2)
        {
            newTime = "0" + theTime;
        }

        return newTime;
    }

    public List<float> getTime()
    {
        List<float> theTime = new List<float>
        {
            theSecondsTime,
            theMinutesTime,
            theHoursTime
        };
        return theTime;
    }
}
