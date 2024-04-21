using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : BaseTimer
{
    public GameObject background;
    public GameObject fill;

    private Image backgroundImage;
    private Image fillImage;

    private Color startColor = Color.white;
    private Color endColor = Color.white;

    private Slider slider;
    private RectTransform rect;
    // Start is called before the first frame update
    void Awake()
    {
        backgroundImage = background.GetComponent<Image>();
        fillImage = fill.GetComponent<Image>();
        slider = GetComponent<Slider>();
        rect = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    public override void OnUpdate()
    {
        if (timeLeft <= 0 || !isRunning)
        {
            return;
        }

        timeLeft -= Time.deltaTime;
        slider.value = Mathf.Ceil(timeLeft);

        Color clr = Color.Lerp(endColor, startColor, timeLeft/maxTime);
        backgroundImage.color = clr;
        fillImage.color = clr;

        if (timeLeft <= 0)
        {
            game.StartCoroutine(callTimeoutMethodName, name);
        }
    }

    public void SetColor(Color clr1, Color clr2)
    {
        startColor = clr1;
        endColor = clr2;
    }

    public void SetNewTime(float newTime)
    {
        timeLeft = newTime;
        maxTime = newTime;
        slider.maxValue = newTime;
        slider.value = newTime;
    }

    public void SetPositionAndSize(Vector3 pos, Vector2 siz)
    {
        rect.localPosition = pos;
        rect.sizeDelta = siz;
    }
}
