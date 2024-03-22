using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    public GameObject ClearToDark;
    public GameObject canvas;

    private string nextScene;

    private Animator animator;

    private void Awake()
    {
        ClearToDark.SetActive(false);
        animator = GetComponent<Animator>();
    }

    public void StartAnim()
    {
        animator.Play("Start2");
    }
}
