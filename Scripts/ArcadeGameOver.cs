using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ArcadeGameOver : MonoBehaviour
{
    public TextMeshProUGUI profitText;
    public TextMeshProUGUI pizzaMadeText;
    public TextMeshProUGUI timeText;
    public GameObject canvas;
    public GameObject hiscoreObj;
    public Image pizzasMadeTrophy;
    public Image timeTrophy;
    public Sprite bronze;
    public Sprite silver;
    public Sprite gold;
    public Sprite platinum;


    private Main mainScript;
    // Start is called before the first frame update
    void Start()
    {
        mainScript = GameObject.Find("main").GetComponent<Main>();
        GetScoreValues();
        CheckHiscore();
    }

    private void GetScoreValues()
    {
        // profit
        float profit = mainScript.profit;
        profit = Snapping.Snap(profit, 0.01f);
        profitText.text = "$" + profit.ToString();

        // pizza's made
        int pizzasMade = mainScript.pizzasMade;
        pizzaMadeText.text = pizzasMade.ToString();

        int getPizzaTrophy = mainScript.GetPizzasMadeTrophy();
        if (getPizzaTrophy == 0)
        {
            pizzasMadeTrophy.gameObject.SetActive(false);
        } else
        {
            switch (getPizzaTrophy)
            {
                case 1:
                    pizzasMadeTrophy.sprite = bronze; break;
                case 2:
                    pizzasMadeTrophy.sprite = silver; break;
                case 3:
                    pizzasMadeTrophy.sprite = gold; break;
                case 4:
                    pizzasMadeTrophy.sprite = platinum; break;
            }
        }

        // time
        string h = mainScript.howManyHours.ToString();
        string m = mainScript.howManyMinutes.ToString();
        string s = mainScript.howManySeconds.ToString();

        // fix time to be 00:00:00
        h = (h.Length == 2) ? h : "0" + h;
        m = (m.Length == 2) ? m : "0" + m;
        s = (s.Length == 2) ? s : "0" + s;

        string theTime = h + ":" + m + ":" + s;
        timeText.text = theTime;

        int getTimeTrophy = mainScript.GetTimeTrophy();
        if (getTimeTrophy == 0)
        {
            timeTrophy.gameObject.SetActive(false);
        }
        else
        {
            switch (getTimeTrophy)
            {
                case 1:
                    timeTrophy.sprite = bronze; break;
                case 2:
                    timeTrophy.sprite = silver; break;
                case 3:
                    timeTrophy.sprite = gold; break;
                case 4:
                    timeTrophy.sprite = platinum; break;
            }
        }
    }

    private void CheckHiscore()
    {
        bool isNewHiscore = mainScript.isNewHiscore;
        if (isNewHiscore)
        {
            Instantiate(hiscoreObj, canvas.transform);
        }
    }

    public void GoToNextScene(string nextSceneName)
    {
        mainScript.nextSceneName = nextSceneName;
        SceneManager.LoadScene("Loading", LoadSceneMode.Single);
    }
}
