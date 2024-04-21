using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningText : MonoBehaviour
{
    public TextMeshProUGUI theText;

    private float secondsToFadeInOut = 2f;
    private float currentSeconds = 0f;
    private bool isFadeOut = false;
    private bool isPlaying = true;

    // Update is called once per frame
    void Update()
    {
        if (!isPlaying)
        {
            return;
        }

        currentSeconds += Time.deltaTime;

        float newAlpha = 0f;
        Color newColor = Color.white;

        if (isFadeOut)
        {
            if (currentSeconds >= secondsToFadeInOut)
            {
                isPlaying = false;
                SceneManager.LoadScene("Loading", LoadSceneMode.Single);
            }
        } else
        {
            if (currentSeconds >= secondsToFadeInOut)
            {
                currentSeconds = 0f;
                isFadeOut = true;
            }

            newAlpha = (currentSeconds / secondsToFadeInOut);
            newColor = new Color(theText.color.r, theText.color.g, theText.color.b, newAlpha);

            theText.color = newColor;
        }
    }
}
