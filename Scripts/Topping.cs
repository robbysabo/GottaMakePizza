using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.ComponentModel;
using UnityEngine.UI;
using UnityEditor;

public class Topping : MonoBehaviour
{
    public string currentState = "raw";
    public string toppingBaseName = "ham";

    private SpriteRenderer spriteRenderer;
    private string texturePath = "Art/SpriteSheet";
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        Sprite getSprite = GetSpriteByName();
        spriteRenderer.sprite = getSprite;
    }

    // Get sprite by name
    Sprite GetSpriteByName()
    {
        string currentToppingName = toppingBaseName + "_" + currentState;
        Sprite[] sprites = Resources.LoadAll<Sprite>(texturePath);
        int findSprite = Array.FindIndex(sprites, s => s.name == currentToppingName);
        return sprites[findSprite];
    }

    // Update topping sprite
    public void SetNewState(string newState)
    {
        currentState = newState;
        Sprite getSprite = GetSpriteByName();
        spriteRenderer.sprite = getSprite;
    }
}