using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleDarkToClear : DarkToClear
{
    public Title theTitle;

    // Update is called once per frame
    void Update()
    {
        TitleDtC();
    }

    void TitleDtC()
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
                theTitle.StartAnim();
                Destroy(gameObject);
            }
        }
    }
}
