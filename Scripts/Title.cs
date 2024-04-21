using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Title : MonoBehaviour
{
    public GameObject ClearToDark;
    public GameObject canvas;
    public AudioSource BGM;
    public AudioSource SFX;
    public Slider bgmSlider;
    public Slider sfxSlider;
    public TextMeshProUGUI pizzasMadeHiscoreText;
    public TextMeshProUGUI timeHiscoreText;
    public GameObject pizzaMadePanel;
    public GameObject timePanel;
    public Image timeTrophy;
    public Image pizzasMadeTrophy;


    [Header("SFX Clips")]
    public AudioClip selected;
    public AudioClip closed;


    private string nextScene;
    private Animator animator;
    private Main mainScript;


    private Color bronze = new Color(0.7358f,0.5427f,0.3436f,1.0f);
    private Color silver = new Color(0.6792f, 0.6792f, 0.6792f, 1.0f);
    private Color gold = new Color(0.8301f,0.7261f,0.2388f,1.0f);
    private Color platinum = new Color(0.4581f,0.5933f,0.6792f,1.0f);


    private void Awake()
    {
        ClearToDark.SetActive(false);
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        mainScript = GameObject.Find("main").GetComponent<Main>();
        GetHiscores();
        GetTrophy();
    }

    private void GetHiscores()
    {
        // pizza's made
        string arcadeHiscorePizzaMade = mainScript.arcadeHiscorePizzaMade.ToString();
        arcadeHiscorePizzaMade = (arcadeHiscorePizzaMade.Length == 2) ? ("0" + arcadeHiscorePizzaMade) : arcadeHiscorePizzaMade;
        pizzasMadeHiscoreText.text = arcadeHiscorePizzaMade;

        // time
        timeHiscoreText.text = mainScript.arcadeHiscoreTime;
    }

    private void GetTrophy()
    {
        // setup
        Image pizzaBronze = pizzaMadePanel.transform.Find("bronze").GetComponent<Image>();
        Image pizzaSilver = pizzaMadePanel.transform.Find("silver").GetComponent<Image>();
        Image pizzaGold = pizzaMadePanel.transform.Find("gold").GetComponent<Image>();
        Image pizzaPlatinum = pizzaMadePanel.transform.Find("platinum").GetComponent<Image>();
        Image timeBronze = timePanel.transform.Find("bronze").GetComponent<Image>();
        Image timeSilver = timePanel.transform.Find("silver").GetComponent<Image>();
        Image timeGold = timePanel.transform.Find("gold").GetComponent<Image>();
        Image timePlatinum = timePanel.transform.Find("platinum").GetComponent<Image>();

        Sprite[] allSprites = Resources.LoadAll<Sprite>("Art/SpriteSheet");

        // Reset Values
        Color blnk = Color.black;
        Color normalColor = Color.white;

        timeTrophy.color = blnk;
        pizzasMadeTrophy.color = blnk;
        
        pizzaBronze.color = blnk;
        pizzaSilver.color = blnk;
        pizzaGold.color = blnk;
        pizzaPlatinum.color = blnk;
        timeBronze.color = blnk;
        timeSilver.color = blnk;
        timeGold.color = blnk;
        timePlatinum.color = blnk;

        // PIZZAS MADE Hiscore
        int pizzasMadeHiscore = mainScript.GetPizzasMadeTrophy();
        string pizzasMadeTrophyRank = "";
        if (pizzasMadeHiscore > 0)
        {
            pizzaBronze.color = normalColor;
            pizzasMadeTrophy.color = normalColor;
            switch (pizzasMadeHiscore)
            {
                case 1:
                    pizzasMadeTrophyRank = "_bronze";
                    break;
                case 2:
                    pizzasMadeTrophyRank = "_silver";
                    pizzaSilver.color = normalColor;
                    break;
                case 3:
                    pizzasMadeTrophyRank = "_gold";
                    pizzaSilver.color = normalColor;
                    pizzaGold.color = normalColor;
                    break;
                case 4:
                    pizzasMadeTrophyRank = "_platinum";
                    pizzaSilver.color = normalColor;
                    pizzaGold.color = normalColor;
                    pizzaPlatinum.color = normalColor;
                    break;
            }
        }

        // Set title menu pizza made trophy
        if (pizzasMadeTrophyRank != "")
        {
            string TrophyRankName = "trophy" + pizzasMadeTrophyRank;
            int findTrophySprite = Array.FindIndex(allSprites, s => s.name == TrophyRankName);
            Sprite pizzamadeTrophySprite = allSprites[findTrophySprite];
            pizzasMadeTrophy.sprite = pizzamadeTrophySprite;
        }

        // TIME Hiscore
        string timeTrophyRankName = "";
        int timeTrophyRank = mainScript.GetTimeTrophy();
        if (timeTrophyRank > 0)
        {
            timeBronze.color = normalColor;
            timeTrophy.color = normalColor;
            switch (timeTrophyRank)
            {
                case 1:
                    timeTrophyRankName = "_bronze";
                    break;
                case 2:
                    timeTrophyRankName = "_silver";
                    timeSilver.color = normalColor;
                    break;
                case 3:
                    timeTrophyRankName = "_gold";
                    timeSilver.color = normalColor;
                    timeGold.color = normalColor;
                    break;
                case 4:
                    timeTrophyRankName = "_platinum";
                    timeSilver.color = normalColor;
                    timeGold.color = normalColor;
                    timePlatinum.color = normalColor;
                    break;
            }
        }

        // Set title menu time trophy
        if (timeTrophyRankName != "")
        {
            string TrophyRankName = "trophy" + timeTrophyRankName;
            int findTrophySprite = Array.FindIndex(allSprites, s => s.name == TrophyRankName);
            Sprite timeTrophySprite = allSprites[findTrophySprite];
            timeTrophy.sprite = timeTrophySprite;
        }
    }

    public void StartAnim()
    {
        // Activate Title after fadein
        animator.Play("Start2");

        UpdateVolume();

        BGM.Play();
    }

    void UpdateVolume()
    {
        bgmSlider.value = mainScript.bgmVolume;
        sfxSlider.value = mainScript.sfxVolume;
        BGM.volume = mainScript.GetBGMVolume();
        SFX.volume = mainScript.GetSFXVolume();
    }    

    public void ClickArcade()
    {
        nextScene = "Arcade_Game";
        animator.Play("Title_TransOut");
        PlaySelectSFX();
    }

    public void ClickQuitGame()
    {
        // Close the game
        Debug.Log("Close game");
        Application.Quit();
    }

    public void GoToNextScene()
    {
        mainScript.nextSceneName = nextScene;
        SceneManager.LoadScene("Loading", LoadSceneMode.Single);
    }

    public void PlaySelectSFX()
    {
        SFX.clip = selected;
        SFX.Play();
    }

    public void PlayClosedSFX()
    {
        SFX.clip = closed;
        SFX.Play();
    }

    // Save / Load / Delete Data

    public void SaveGame()
    {
        mainScript.ManualSaveData();
    }

    public void DeleteSave()
    {
        // Delete Save data
        DataPersistenceManager dataManager = GameObject.Find("dataManager").GetComponent<DataPersistenceManager>();
        dataManager.DeleteData();
        // reload Title screen
        nextScene = "Title";
        animator.Play("Title_TransOut");
        PlaySelectSFX();
        // create new save
        dataManager.LoadGame();
    }
}
