using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public AudioSource BGM;
    public AudioSource SFX;
    //public AudioSource Ambience;
    public Slider musicSlider;
    public Slider sfxSlider;
    private Main mainScript;
    private float maxVolume = 0.5f;
    //private float maxAmbienceVolume = 0.4f;
    // Start is called before the first frame update
    void Start()
    {
        mainScript = GameObject.Find("main").GetComponent<Main>();
    }

    public virtual void MusicVolumeChange()
    {
        if (mainScript != null)
        {
            float newValue = musicSlider.value;
            mainScript.bgmVolume = newValue;
            float newVolume = Mathf.Lerp(0f, maxVolume, newValue);
            BGM.volume = newVolume;
            //AmbienceVolumeChange(newValue);
        }
    }

    public void SFXVolumeChange()
    {
        if (mainScript != null)
        {
            float newValue = sfxSlider.value;
            mainScript.sfxVolume = newValue;
            float newVolume = Mathf.Lerp(0f, maxVolume, newValue);
            SFX.volume = newVolume;
        }
    }

    /*private void AmbienceVolumeChange(float musicVolume)
    {
        if (mainScript != null)
        {
            float newVolume = Mathf.Lerp(0f, maxAmbienceVolume, musicVolume);
            Ambience.volume = newVolume;
        }
    }*/
}
