using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour, IDataPersistence
{
    public bool isClickable = true;
    public int maxToppings = 10;
    public Dictionary<string, int> ticketsOrdered = new Dictionary<string, int>
    {
        { "cheese",0 },
        { "pepperoni",0 },
        { "veggie",0 },
        { "hawaiian",0 },
        { "surpreme",0 },
        { "meat_lover",0 },
        { "everything",0 },
        { "just_sauce",0 },
        { "just_cheese",0 },
        { "cooked_dough",0 },
        { "custom_ticket",0 }
    };
    public Dictionary<string, int> currentTicketsOrdered = new Dictionary<string, int>
    {
        { "cheese",0 },
        { "pepperoni",0 },
        { "veggie",0 },
        { "hawaiian",0 },
        { "surpreme",0 },
        { "meat_lover",0 },
        { "everything",0 },
        { "just_sauce",0 },
        { "just_cheese",0 },
        { "cooked_dough",0 },
        { "custom_ticket",0 }
    };
    public int pizzasMade = 0;
    public DateTime startGame = DateTime.Now;
    public DateTime endGame = DateTime.Now;
    public int howManyHours = 0;
    public int howManyMinutes = 0;
    public int howManySeconds = 0;

    // For Arcade Mode Only
    public float profit = 0f;
    public float arcadeTime = 0f;

    // For Story Mode Only
    public bool storyDone = false; // saved
    public bool storyStarted = false; // saved
    public int storyDays = 30; // saved
    public float storyMoney = -10000f; // saved

    // Loading
    public float loadingLength = 1f; // 0=short 1=med 2=long
    public string nextSceneName = "Title";

    // Audio
    public int currentSong = 0;
    public float maxVolume = 0.5f;
    public float bgmVolume = 1f;//saved
    public float sfxVolume = 1f;//saved

    // Cost of Ingredients
    public Dictionary<string, float> prices = new Dictionary<string, float>
    {
        {"dough",-1f},
        {"sauce",-0.02075f},
        {"cheese",-0.058125f},
        {"bacon",-0.1234375f},
        {"beef",-0.10195f},
        {"bellpepper",-0.04875f},
        {"ham",-0.10359375f},
        {"mushroom",-0.07125f},
        {"olive",-0.0201875f},
        {"onion",-0.04875f},
        {"pepperoni",-0.09840625f},
        {"pineapple",-0.020495f},
        {"spinach",-0.020495f},
    };

    // Hiscores
    public bool isNewHiscore = false;
    public int arcadeHiscorePizzaMade = 100; // saved
    public string arcadeHiscoreTime = "00h:05m:00s"; // saved
    public int storyHiscoreDays = 30; // saved
    public float storyHiscoreMoney = 0f; // saved
    public int arcadeTrophyVisual = 0; // 0-none 1-bronze 2-silver 3-gold 4-platinum
    public int storyTrophyVisual = 0; // 0-none 1-bronze 2-silver 3-gold 4-platinum

    // Data Persistence Manager
    DataPersistenceManager dataManager;

    // Runs before a scene gets loaded
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void LoadMain()
    {
        GameObject main = Instantiate(Resources.Load("Main")) as GameObject;
        DontDestroyOnLoad(main);
        main.name = "main";

        GameObject dataPerManager = Instantiate(Resources.Load("DataPersistenceManager")) as GameObject;
        DontDestroyOnLoad(dataPerManager);
        dataPerManager.name = "dataManager";
    }

    private void Start()
    {
        dataManager = GameObject.Find("dataManager").GetComponent<DataPersistenceManager>();
    }

    public string FixDecimal(string txt)
    {
        string newTxt = txt;

        if (!newTxt.Contains('.'))
        {
            newTxt += ".00";
        }

        string txtLengthAfterDecimal = newTxt.Split('.')[1];
        if (txtLengthAfterDecimal.Length < 2)
        {
            newTxt += "0";
        }

        return newTxt;
    }

    public string Capitalize(string ticketName)
    {
        string newTitle = ticketName;
        if (ticketName.Contains('_'))
        {
            newTitle = "";
            for (int i = 0; i < ticketName.Split("_").Length; i++)
            {
                newTitle += ticketName.Split("_")[i] + " ";
            }
            newTitle = newTitle.Trim();
        }

        newTitle = newTitle.FirstCharacterToUpper();
        return newTitle;
    }

    public void ResetPizzaValues()
    {
        isNewHiscore = false;
        startGame = DateTime.Now;
        pizzasMade = 0;
        profit = 0f;
        ticketsOrdered = new Dictionary<string, int>
        {
            { "cheese",0 },
            { "pepperoni",0 },
            { "veggie",0 },
            { "hawaiian",0 },
            { "surpreme",0 },
            { "meat_lover",0 },
            { "everything",0 },
            { "just_sauce",0 },
            { "just_cheese",0 },
            { "cooked_dough",0 },
            { "custom_ticket",0 }
        };
        currentTicketsOrdered = new Dictionary<string, int>
        {
            { "cheese",0 },
            { "pepperoni",0 },
            { "veggie",0 },
            { "hawaiian",0 },
            { "surpreme",0 },
            { "meat_lover",0 },
            { "everything",0 },
            { "just_sauce",0 },
            { "just_cheese",0 },
            { "cooked_dough",0 },
            { "custom_ticket",0 }
        };
    }

    public void EndArcade()
    {
        endGame = DateTime.Now;
        TimeSpan calcTime = endGame - startGame;
        howManyHours = Mathf.FloorToInt((float)calcTime.TotalHours % 24);
        howManyMinutes = Mathf.FloorToInt((float)calcTime.TotalMinutes % 60);
        howManySeconds = Mathf.FloorToInt((float)calcTime.TotalSeconds % 60);
        CheckNewArcadeHiscore();
    }

    private void CheckNewArcadeHiscore()
    {
        isNewHiscore = false;
        // Arcade Time
        int hiscoreHours = int.Parse(arcadeHiscoreTime.Substring(0,2));
        int hiscoreMinutes = int.Parse(arcadeHiscoreTime.Substring(4,2));
        int hiscoreSeconds = int.Parse(arcadeHiscoreTime.Substring(8,2));

        if (hiscoreHours < howManyHours || hiscoreMinutes < howManyMinutes || hiscoreSeconds < howManyHours)
        {
            string h = howManyHours.ToString();
            string m = howManyMinutes.ToString();
            string s = howManySeconds.ToString();

            if (h.Length < 2)
            {
                h = "0" + h;
            }

            if (m.Length < 2)
            {
                m = "0" + m;
            }

            if (s.Length < 2)
            {
                s = "0" + s;
            }

            string newArcadeTime = h + "h:" + m + "m:" + s + "s";
            arcadeHiscoreTime = newArcadeTime;
            isNewHiscore = true;
        }

        // Arcade Pizza's made
        if (arcadeHiscorePizzaMade < pizzasMade)
        {
            arcadeHiscorePizzaMade = pizzasMade;
            isNewHiscore = true;
        }

        // Save Game
        dataManager.SaveGame();
    }

    public void AddTicketOrderValue(string ticketOrderName)
    {
        ticketsOrdered[ticketOrderName] += 1;
        pizzasMade++;
    }

    public void UpdateLoadTime()
    {
        // Add one and modulos 3 to keep it between 0-2
        loadingLength += 1f;
        loadingLength %= 3;
    }

    public float GetBGMVolume()
    {
        float bgmVol = Mathf.Lerp(0f, maxVolume, bgmVolume);
        return bgmVol;
    }

    public float GetSFXVolume()
    {
        float sfxVol = Mathf.Lerp(0f, maxVolume, sfxVolume);
        return sfxVol;
    }


    public bool CheckArcadeHiscores()
    {
        /*
         * pizza made
         */
        if (arcadeHiscorePizzaMade >= 1000)
        {
            arcadeTrophyVisual = 4;
        }
        else if (arcadeHiscorePizzaMade >= 500)
        {
            arcadeTrophyVisual = 3;
        }
        else if (arcadeHiscorePizzaMade >= 250)
        {
            arcadeTrophyVisual = 2;
        }
        else if (arcadeHiscorePizzaMade >= 100)
        {
            arcadeTrophyVisual = 1;
        }
        else
        {
            arcadeTrophyVisual = 0;
        }

        bool checkArcadeTrophy = (arcadeTrophyVisual != 0); 
        return checkArcadeTrophy;
    }

    public int GetPizzasMadeTrophy()
    {
        int theRank = 0; // 0=none 1=bronze 2=silver 3=gold 4=platinum

        if (arcadeHiscorePizzaMade >= 1000)
        {
            theRank = 4;
        } else if (arcadeHiscorePizzaMade >= 500)
        {
            theRank = 3;
        } else if (arcadeHiscorePizzaMade >= 250)
        {
            theRank = 2;
        } else if (arcadeHiscorePizzaMade >= 100)
        {
            theRank = 1;
        }

        return theRank;
    }

    public int GetTimeTrophy()
    {
        int theRank = 0; // 0=none 1=bronze 2=silver 3=gold 4=platinum
        int hiscoreHours = int.Parse(arcadeHiscoreTime.Substring(0, 2));
        int hiscoreMinutes = int.Parse(arcadeHiscoreTime.Substring(4, 2));

        if (hiscoreHours > 0)
        {
            theRank = 3;
            if (hiscoreHours >= 2)
            {
                theRank = 4;
            }
        }
        else
        {
            if (hiscoreMinutes >= 10)
            {
                theRank = 1;
            }
            if (hiscoreMinutes >= 30)
            {
                theRank = 2;
            }
        }

        return theRank;
    }

    public void ManualSaveData()
    {
        dataManager.SaveGame();
    }

    public void LoadData(GameData data)
    {
        this.arcadeHiscorePizzaMade = data.arcadeHiscorePizzaMade;
        this.arcadeHiscoreTime = data.arcadeHiscoreTime;
        this.bgmVolume = data.bgmVolume;
        this.sfxVolume = data.sfxVolume;
    }

    public void SaveData(ref GameData data)
    {
        data.arcadeHiscorePizzaMade = this.arcadeHiscorePizzaMade;
        data.arcadeHiscoreTime = this.arcadeHiscoreTime;
        data.bgmVolume = this.bgmVolume;
        data.sfxVolume = this.sfxVolume;
    }
}
