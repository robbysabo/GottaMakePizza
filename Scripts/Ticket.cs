using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Ticket : MonoBehaviour
{
    public Game game;
    public ArrayList ticketOrder = new ArrayList();
    public int maxRNG = 101;
    public bool isClickable = true;
    public int ticketNumber = 0;
    public int setDebugRNG = 101;
    private Dictionary<string, ArrayList> possibleOrders = new Dictionary<string, ArrayList>();
    private ArrayList possibleToppings = new ArrayList
    {
        "bacon","beef","bellpepper","ham","mushroom","olive","onion","pepperoni","pineapple","spinach"
    };


    private void OnMouseDown()
    {
        if (isClickable)
        {
            game.TicketClicked(name);
        }
    }


    private void OnMouseOver()
    {
        if (isClickable)
        {
            game.ShowTooltip(name);
        }
    }

    private void OnMouseExit()
    {
        game.HideTooltip();
    }


    public void StartMakingTicket()
    {
        if (game == null)
        {
            Debug.LogError("No Parent Script");
            return;
        }
        AddAllPossibleOrders();
        CreateTicketOrder();
    }

    private void CreateTicketOrder()
    {
        int rng = Random.Range(1, maxRNG);
        //rng = setDebugRNG; // DEBUG ONLY
        string ticketName = "custom_ticket";
        /*
        # all_pizza_types can only be 15 characters long. (including spaces)
	    # 1-100 chance        =99            =98           =97          =1-25    =26-50      =51-65   =66-75     =76-85       =86-95     =96       =100+
	    # all_pizza_types = ["cooked_dough","just_cheese","just_sauce","cheese","pepperoni","veggie","hawaiian","meat_lover","surpreme","everything","custom_pizza"]
	    # 1-100 percentage    01%            01%           01%          25%      25%         15%      10%        10%          10%        01%          01%+
        */

        if (rng >= 100)
        {
            ticketOrder = BuildCustomTicket();
        } else if (rng == 99)
        {
            ticketName = "cooked_dough";
        } else if (rng == 98)
        {
            ticketName = "just_cheese";
        } else if (rng == 97)
        {
            ticketName = "just_sauce";
        } else if (rng == 96)
        {
            ticketName = "everything";
        } else if (rng >= 86)
        {
            ticketName = "surpreme";
        } else if (rng >= 76)
        {
            ticketName = "meat_lover";
        } else if (rng >= 66)
        {
            ticketName = "hawaiian";
        } else if (rng >= 51)
        {
            ticketName = "veggie";
        } else if (rng >= 26)
        {
            ticketName = "pepperoni";
        } else
        {
            ticketName = "cheese";
        }

        // assign ticket order
        
        bool checkIfPossibleOrder = possibleOrders.ContainsKey(ticketName);
        Main main = GameObject.Find("main").GetComponent<Main>();
        if (checkIfPossibleOrder)
        {
            ticketOrder = possibleOrders[ticketName];
        }

        // get main tickets ordered
        ticketNumber = main.currentTicketsOrdered[ticketName] + 1;
        main.currentTicketsOrdered[ticketName] = ticketNumber;

        // rename ticket
        name = ticketName + ticketNumber.ToString();
    }


    private ArrayList BuildCustomTicket()
    {
        ArrayList customTicket = new ArrayList();

        // sauce + cheese
        int rng = Random.Range(1, 101);
        bool rngHasSauce = rng == 13 ? false : true;
        rng = Random.Range(1, 101);
        bool rngHasCheese = rng == 13 ? false : true;
        rng = Random.Range(1, 101);
        bool isSauceThanCheese = rng == 13 ? false : true;

        // toppings
        // random amount of toppings (2-6)
        int minToppings = 2;
        int maxToppings = 6;
        Dictionary<string,int> buildToppings = new Dictionary<string,int>();
        int possibleAmountOfToppings = possibleToppings.Count;
        int rngAmountOfToppings = Random.Range(1,possibleAmountOfToppings + 1);
        int toppingCount = 0;
        ArrayList toppingsPicked = new ArrayList();
        while(toppingCount != rngAmountOfToppings)
        {
            int rngToppingList = Random.Range(0, possibleToppings.Count);
            if (toppingsPicked.Count != 0)
            {
                if (!toppingsPicked.Contains(rngToppingList))
                {
                    toppingsPicked.Add(rngToppingList);
                    toppingCount++;
                }
            } else
            {
                toppingsPicked.Add(rngToppingList);
                toppingCount++;
            }
        }

        foreach(int tp in toppingsPicked)
        {
            rng = Random.Range(minToppings, maxToppings);
            buildToppings.Add((string)possibleToppings[tp], rng);
        }

        // build ticket
        // sauce + cheese
        if (rngHasCheese && rngHasSauce)
        {
            if (isSauceThanCheese)
            {
                customTicket.Add("sauce");
                customTicket.Add("cheese");
            } 
            else
            {
                customTicket.Add("cheese");
                customTicket.Add("sauce");
            }
        }
        else if (!rngHasCheese && rngHasSauce)
        {
            customTicket.Add("sauce");
        }
        else if (rngHasCheese && !rngHasSauce)
        {
            customTicket.Add("cheese");
        }

        // toppings
        customTicket.Add(buildToppings);
        
        return customTicket;
    }


    public void AddAllPossibleOrders()
    {
        Dictionary<string, int> toppings = new Dictionary<string, int>(); // "name":# minimum amount
        // cheese
        possibleOrders.Add("cheese", new ArrayList { "sauce", "cheese", toppings });
        // pepperoni
        toppings = new Dictionary<string, int>
        {
            { "pepperoni", 12 }
        };
        possibleOrders.Add("pepperoni", new ArrayList { "sauce", "cheese", toppings });
        // veggie
        toppings = new Dictionary<string, int>
        {
            { "bellpepper", 4 },
            { "mushroom", 4 },
            { "olive", 4 },
            { "onion", 4 },
            { "spinach", 4 }
        };
        possibleOrders.Add("veggie", new ArrayList { "sauce", "cheese", toppings });
        // hawaiian
        toppings = new Dictionary<string, int>
        {
            { "bacon", 6 },
            { "ham", 6 },
            { "pineapple", 6 }
        };
        possibleOrders.Add("hawaiian", new ArrayList { "sauce", "cheese", toppings });
        // surpreme
        toppings = new Dictionary<string, int>
        {
            { "bellpepper", 4 },
            { "mushroom", 4 },
            { "olive", 4 },
            { "onion", 4 },
            { "pepperoni", 8 }
        };
        possibleOrders.Add("surpreme", new ArrayList { "sauce", "cheese", toppings });
        // meat lovers
        toppings = new Dictionary<string, int>
        {
            { "bacon", 5 },
            { "beef", 5 },
            { "ham", 5 },
            { "pepperoni", 5 }
        };
        possibleOrders.Add("meat_lover", new ArrayList { "sauce", "cheese", toppings });
        // everything
        toppings = new Dictionary<string, int>
        {
            { "bacon", 3 },
            { "beef", 3 },
            { "bellpepper", 3 },
            { "ham", 3 },
            { "mushroom", 3 },
            { "olive", 3 },
            { "onion", 3 },
            { "pepperoni", 3 },
            { "pineapple", 3 },
            { "spinach", 3 }
        };
        possibleOrders.Add("everything", new ArrayList { "sauce", "cheese", toppings });
        // cooked dough
        toppings = new Dictionary<string, int>();
        possibleOrders.Add("cooked_dough", new ArrayList { toppings });
        // just cheese
        possibleOrders.Add("just_cheese", new ArrayList { "cheese", toppings});
        // just sauce
        possibleOrders.Add("just_sauce", new ArrayList { "sauce", toppings});

    }

    public void SetClickable(bool boolValue)
    {
        isClickable = boolValue;
    }
}
