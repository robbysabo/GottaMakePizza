using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArcadeGameOver : MonoBehaviour
{
    public TextMeshProUGUI profitText;
    public TextMeshProUGUI pizzaMadeText;
    public TextMeshProUGUI timeText;
    public GameObject canvas;
    public GameObject hiscoreObj;

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
