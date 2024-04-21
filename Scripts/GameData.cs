using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int arcadeHiscorePizzaMade;
    public string arcadeHiscoreTime;
    public float bgmVolume;
    public float sfxVolume;


    // the values defined in the constructor will be the default values
    // the game starts with when there's no data to load
    public GameData()
    {
        this.arcadeHiscorePizzaMade = 10;
        this.arcadeHiscoreTime = "00h:05m:00s";
        this.bgmVolume = 1f;
        this.sfxVolume = 1f;
    }
}
