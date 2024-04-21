using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arcade_Ambience : MonoBehaviour
{
    private float currentVolume = 0f;
    private float maxVolume = 0.4f;
    private bool canRun = false;
    private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Main mainScript = GameObject.Find("main").GetComponent<Main>();
        currentVolume = mainScript.sfxVolume;

        maxVolume = Mathf.Lerp(0f, 0.4f, currentVolume);

        canRun = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canRun)
        {
            currentVolume -= (Time.deltaTime / 60f);
            if (currentVolume <= 0f) {
            currentVolume = 0f;
            canRun = false;
            }
            float newVolume = Mathf.Lerp(maxVolume, 0f, currentVolume);
            audioSource.volume = newVolume;
        }
    }
}
