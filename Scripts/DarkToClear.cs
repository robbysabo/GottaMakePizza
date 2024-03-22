using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DarkToClear : MonoBehaviour
{
    public Image img;

    Color initColor;

    public float currentTime = 2f;
    public bool canRun = true;

    float maxTime;

    void Awake()
    {
        img = GetComponent<Image>();
        initColor = img.color;
        maxTime = currentTime;
    }

    // Update is called once per frame
    void Update()
    {
        BaseUpdate();
    }

    void BaseUpdate()
    {
        if (canRun)
        {
            if (img == null)
            {
                Destroy(gameObject);
            }

            currentTime -= Time.deltaTime;
            CalcImg();

            if (currentTime <= 0)
            {
                canRun = false;
                Destroy(gameObject);
            }
        }
    }

    public void CalcImg()
    {
        float aValue = currentTime / maxTime;
        Color clr = img.color;
        clr.a = aValue;
        img.color = clr;
    }
}
