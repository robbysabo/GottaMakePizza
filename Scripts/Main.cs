using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Main : MonoBehaviour
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
    public float profit = 0f;
    public DateTime startGame = DateTime.Now;
    public DateTime endGame = DateTime.Now;
    public int howManyHours = 0;
    public int howManyMinutes = 0;
    public int howManySeconds = 0;

    public float arcadeTime = 0f;

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

    // List of Customer Reviews
    public ArrayList needMoreToppingsReview = new ArrayList
    {
        "Wish they would add more toppings.", "could added a bit more toppings on it.", "more toppings next time, please!"
    };
    public ArrayList needLessToppingsReview = new ArrayList
    {
        "Way too many toppings.", "Too many toppings."
    };
    public ArrayList perfectReview = new ArrayList
    {
        "Great job! 5 Stars!", "In, out, BOOM! PIZZA! 5 Stars!", "Just had to say, quickest pizza ever!", "Crazy fast! Crazy Good!", "Take all my stars!!!", "Talk about speed, never had a pizza so fast in my life!", "It should be a crime on how FAST and GOOD the pizza is here!","Kudos to the chef, on making an awesome pizza and at insane speed!"
    };
    public ArrayList goodReview = new ArrayList
    {
        "Pretty good!","Above average!","Better than most pizzas.","Best pizza around."
    };
    public ArrayList littleSlowReview = new ArrayList
    {
        "A tad slow but good.","Great, but could be a little faster.","Wish it was a little faster.","Service is a little slow.","Be faster please."
    };
    public ArrayList verySlowReview = new ArrayList
    {
        "Good pizza, very slow.","Wish it was done quicker!"
    };
    public ArrayList failedReview = new ArrayList
    {
        "Couldn't be any slower.","Slowest pizza place.","Not worth the wait."
    };

        // Runs before a scene gets loaded
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void LoadMain()
    {
        GameObject main = Instantiate(Resources.Load("Main")) as GameObject;
        DontDestroyOnLoad(main);
        main.name = "main";
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

    public void EndGameTime()
    {
        endGame = DateTime.Now;
        TimeSpan calcTime = endGame - startGame;
        howManyHours = Mathf.FloorToInt((float)calcTime.TotalHours % 24);
        howManyMinutes = Mathf.FloorToInt((float)calcTime.TotalMinutes % 60);
        howManySeconds = Mathf.FloorToInt((float)calcTime.TotalSeconds % 60);
    }

    public void AddTicketOrderValue(string ticketOrderName)
    {
        ticketsOrdered[ticketOrderName] += 1;
    }
}
