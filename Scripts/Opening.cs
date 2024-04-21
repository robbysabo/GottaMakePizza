using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Opening : MonoBehaviour
{
    public TextMeshProUGUI theText;

    private float secondsToFadeInOut = 2f;
    private float currentSeconds = 0f;
    private bool isFadeOut = false;
    private bool isPlaying = true;

    private void Update()
    {
        if (!isPlaying)
        {
            return;
        }

        currentSeconds += Time.deltaTime;

        float newAlpha = 1f;
        Color newColor = new Color(theText.color.r, theText.color.g, theText.color.b, 1);

        if (isFadeOut)
        {
            if (currentSeconds >= secondsToFadeInOut)
            {
                isPlaying = false;
                SceneManager.LoadScene("Loading", LoadSceneMode.Single);
            }

            newAlpha = Mathf.Lerp(1f, 0f, (currentSeconds / secondsToFadeInOut));
        }
        else
        {
            if (currentSeconds >= secondsToFadeInOut)
            {
                currentSeconds = secondsToFadeInOut;
                isPlaying = false;
                Invoke("StartFadeOut", secondsToFadeInOut);
            }

            newAlpha = (currentSeconds / secondsToFadeInOut);
        }
        newColor.a = newAlpha;
        theText.color = newColor;
    }

    private void StartFadeOut()
    {
        currentSeconds = 0f;
        isFadeOut = true;
        isPlaying = true;
    }
}
