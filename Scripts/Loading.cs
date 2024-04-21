using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loading : MonoBehaviour
{
    public Animator mAni;
    public Animator mpAni;
    public Transform cheese;
    public Transform cuts;
    public AudioSource sfx;
    public AudioClip sweepout;
    public string nextSceneName = "Title";

    float spin = 0f;
    bool isSpining = true;
    float spinLength = 1f;
    float currentTime = 0f;

    private Main mainScript;

    void Start()
    {
        mainScript = GameObject.Find("main").GetComponent<Main>();
        mAni.Play("loading_metal_closing");
        mpAni.Play("loading_metalP_closing");
        spinLength += mainScript.loadingLength;
        mainScript.UpdateLoadTime();
        nextSceneName = mainScript.nextSceneName;
    }

    void spinPizza()
    {
        currentTime += Time.deltaTime;
        spin += Time.deltaTime / 5f;
        spin %= 360;
        Vector3 rot = new Vector3(0, 0, spin);
        cheese.Rotate(rot);
        cuts.Rotate(rot);

        if (currentTime >= spinLength)
        {
            currentTime = 0;
            mAni.Play("loading_metal_opening");
            mpAni.Play("loading_metalP_opening");
            sfx.clip = sweepout;
            sfx.Play();
            isSpining = false;
        }
    }

    void GoToNextScene()
    {
        currentTime += Time.deltaTime;

        if (currentTime >= 0.5f)
        {
            SceneManager.LoadScene(nextSceneName, LoadSceneMode.Single);
        }
    }

    void Update()
    {
        if (isSpining)
        {
            spinPizza();
        }
        else
        {
            GoToNextScene();
        }
    }
}
