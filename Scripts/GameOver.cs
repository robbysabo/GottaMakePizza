using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public string gameOverScene;
    private Image bg;
    private Transform img;
    private float timeToTransition = 2f;
    private float timeLeft = 2f;
    private bool isStarting = false;
    private Vector3 startPos = Vector3.zero;
    private Vector3 endPos = new Vector3(0f,52f);

    // Start is called before the first frame update
    void Start()
    {
        bg = GetComponent<Image>();
        img = transform.Find("GameOverImg");
    }

    private void FadeOut()
    {
        if (timeLeft < 0f)
        {
            // transition to game over screen
            SceneManager.LoadScene(gameOverScene, LoadSceneMode.Single);
        }

        timeLeft = Mathf.Clamp(timeLeft, 0f, timeToTransition);

        Color clr = bg.color;
        clr.a = Mathf.Lerp(1.0f,0.0f, timeLeft / timeToTransition);
        bg.color = clr;

        img.localPosition = Vector3.Lerp(endPos, startPos, timeLeft / timeToTransition);
    }

    private void MoveUp()
    {
        timeLeft = Mathf.Clamp(timeLeft, 0f, timeToTransition);

        Color clr = img.GetComponent<Image>().color;
        clr.g = Mathf.Lerp(0.0f, 1.0f, timeLeft / timeToTransition);
        clr.b = Mathf.Lerp(0.0f, 1.0f, timeLeft / timeToTransition);
        img.GetComponent<Image>().color = clr;

        if (timeLeft <= 0f)
        {
            timeLeft = timeToTransition;
            isStarting = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (bg != null && img != null)
        {
            timeLeft -= Time.deltaTime;
            if (isStarting)
            {
                FadeOut();
            }
            else
            {
                MoveUp();
            }

        }
    }
}
