using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class TicketUI : MonoBehaviour, IPointerClickHandler
{
    public Game game;

    public GameObject Title;
    public GameObject Checkbox1;
    public GameObject Checkbox2;
    public GameObject OrderContainer;

    private float timeToShow = 0.4f;
    private float timeLeft = 0f;
    private bool isShowing = false;
    private Vector3 startPos = new Vector3(0f,-288f,0f);
    private Vector3 endPos = Vector3.zero;

    private int maxToppings = 10;

    private string texturePath = "Art/SpriteSheet";

    private Main mainScript;

    void Start()
    {
        mainScript = GameObject.Find("main").GetComponent<Main>();
        maxToppings = mainScript.maxToppings;
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        game.HideTicketUI();
    }

    void Update()
    {
        Vector3 calcPos = Vector3.zero;
        if (isShowing)
        {
            // move ui down
            timeLeft += Time.deltaTime;
            if (timeLeft > timeToShow)
            {
                timeLeft = timeToShow;
            }
        }
        else
        {
            // move ui up
            timeLeft -= Time.deltaTime;
            if (timeLeft < 0f)
            {
                timeLeft = 0f;
            }
        }

        calcPos = Vector3.Lerp(startPos, endPos, timeLeft / timeToShow);
        transform.localPosition = calcPos;
    }

    public void OnShowUI(string ticketName, ArrayList ticketOrder, int orderNumber)
    {
        int hasSauce = ticketOrder.IndexOf("sauce",0);
        int hasCheese = ticketOrder.IndexOf("cheese",0);
        Dictionary<string, int> toppings = (Dictionary<string, int>)ticketOrder[ticketOrder.Count - 1];
        string theName = "";

        foreach (char c in ticketName)
        {
            bool isValidNumber = char.IsDigit(c);
            if (!isValidNumber)
            {
                theName += c.ToString();
            }
        }

        theName += " " + orderNumber.ToString();

        ResetUI();
        BuildUI(theName, hasSauce, hasCheese, toppings);
        ShowUI(true);
    }

    public void ResetUI()
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
            if (child.childCount > 0)
            {
                foreach (Transform child2 in child)
                {
                    child2.gameObject.SetActive(true);
                }
            }
        }
    }

    public void BuildUI(string ticketName, int sauceIndex, int cheeseIndex, Dictionary<string, int> toppings)
    {
        TextMeshProUGUI txt = Title.gameObject.GetComponent<TextMeshProUGUI>();
        string newTitle = mainScript.Capitalize(ticketName);
        txt.text = newTitle;

        bool hasSauce = sauceIndex >= 0;
        bool hasCheese = cheeseIndex >= 0;

        TextMeshProUGUI saucecheese1 = Checkbox1.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI saucecheese2 = Checkbox2.transform.GetChild(0).GetComponent<TextMeshProUGUI>();

        Transform theX1 = Checkbox1.transform.GetChild(1);
        Transform theX2 = Checkbox2.transform.GetChild(1);

        if (hasCheese)
        {
            if (cheeseIndex < sauceIndex || !hasSauce)
            {
                // change cheese and sauce text
                saucecheese1.text = "Cheese";
                saucecheese2.text = "Sauce";
            }
        }

        if (!hasSauce)
        {
            // no sauce, remove X
            string saucecheesetxt = saucecheese1.text;
            if (saucecheesetxt == "Sauce")
            {
                theX1.gameObject.SetActive(false);
            }
            else
            {
                theX2.gameObject.SetActive(false);
            }
        }

        if (!hasCheese)
        {
            // no cheese, remove X
            string saucecheesetxt = saucecheese1.text;
            if (saucecheesetxt == "Cheese")
            {
                theX1.gameObject.SetActive(false);
            }
            else
            {
                theX2.gameObject.SetActive(false);
            }
        }

        if (toppings.Count > 0)
        {
            // make invisible the extra order#
            int toppingCount = 0;
            foreach (var t in toppings)
            {
                string currentName = t.Key.ToString() + "_raw";
                Sprite[] sprites = Resources.LoadAll<Sprite>(texturePath);
                int findSprite = Array.FindIndex(sprites, s => s.name == currentName);
                Sprite toppingSprite = sprites[findSprite];
                GameObject orderObj = OrderContainer.transform.GetChild(toppingCount).gameObject;
                Image orderImage = orderObj.GetComponent<Image>();
                orderImage.sprite = toppingSprite;
                TextMeshProUGUI orderAmount = orderObj.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                orderAmount.text = "x " + t.Value.ToString();

                toppingCount++;
            }
            int[] restOfOrderObj = new int[maxToppings];
            for (int i = toppingCount;  i < maxToppings; i++)
            {
                OrderContainer.transform.GetChild(i).gameObject.SetActive(false);
            }
        } else
        {
            // no toppings = set all other orders to invisible
            foreach (Transform child in OrderContainer.transform)
            {
                child.gameObject.SetActive(false);
            }
        }
    }

    public void ShowUI(bool boolValue)
    {
        isShowing = boolValue;
    }
}
