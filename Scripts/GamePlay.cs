using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class GamePlay : MonoBehaviour
{
    /*[Header("Script only")]
    public GameObject canvas;
    public Main mainScript;
    private GameObject GameContainer;
    private PizzaCutters pizzaCutters;
    private ClickTable clickTable;

    [Header("Game Objects")]
    public GameObject PizzaObj;
    public GameObject TimerUIObj;
    public GameObject TicketObj;
    public GameObject TicketUIObj;
    public GameObject UpdateMoneyTextObj;
    public GameObject UpdateRatingsTextObj;
    public GameObject MoneyText;
    public GameObject RatingsText;
    public GameObject GameOverObj;
    public GameObject BaseTimerObj;
    public GameObject TooltipObj;
    public GameObject TooltipTextObj;
    public GameObject ReviewPanelObj;


    // game play vars
    [Header("Game Play Vars")]
    public float tip = 0f;
    public const float MAX_TIP = 5.0f;
    public float money = 0f;
    public const float PIZZA_PRICE = 10f;
    public float ratings = 2.5f;
    public ArrayList reviewRatings = new ArrayList { 2.5f,2.5f,2.5f };
    public const int MAX_AMOUNT_RATINGS = 8;
    public float ticketRatings = 5.0f;
    public const int MAX_TOPPING_MULTIPLIER = 2;
    public ArrayList ticketTimerEnded = new ArrayList();
    public ArrayList ticketFailed = new ArrayList();
    public string oven1Pizza = "";
    public string oven2Pizza = "";
    private float maxTicketSpawnWaitTime = 32f;
    private float minTicketSpawnWaitTime = 7f;
    private bool isTooltipOn = false;
    private Vector3 tooltipOffset = new Vector3(15, -15, 0);
    private ArrayList customerReview = new ArrayList();


    // pizza vars
    private string pizzaSelected = "";
    private bool isPizzaOnTable = false;
    private Vector3 pizzaOnTablePos = new Vector3(-0.24f, -1.91f, 0f);
    private Vector3 pizzaInOven1Pos = new Vector3(5.2f, -1.635f, 0f);
    private Vector3 pizzaInOven2Pos = new Vector3(5.2f, 1.545f, 0f);
    private Vector3 pizzaOnTrayPos = new Vector3(-5.073f, -0.68f);
    private bool pizzaCutterSelected = false;
    private string pizzaOnTray = "";


    // ingredient vars
    private string foodSelected = "";


    // timer ui vars
    private Color YellowTimer = new Color(0.9569f,0.8863f,0.3373f,1f);
    private Color GreenTimer = new Color(0.2471f, 0.6f, 0.2471f, 1f);
    private Color RedTimer = new Color(0.7686f, 0.149f, 0.1569f, 1f);


    // oven timer
    private Vector2 OvenTimerSize = new Vector2(150f,150f);
    private Vector3 Oven1TimerPos = new Vector3(552f, -135f, 0f);
    private Vector3 Oven2TimerPos = new Vector3(552f, 190f, 0f);
    private Vector3 OvenPizzaCookingPos = new Vector3(1552f,1190f,0f); // offscreen


    // ticket+order vars
    private int maxCustomTicketRNG = 101;
    private Vector3[] ticketPositions = new Vector3[7];
    private ArrayList tickets = new ArrayList();
    private Vector3[] ticketUIPositions = new Vector3[7];
    private Vector2 ticketTimerSize = new Vector2(28f, 28f);
    private float ticketWaitTime = 120f; // ############################################################################## Testing
    private Vector3 ticketTimerPos = new Vector3(-2000f, -2000f, 0f);

    private void Start()
    {
        Setup();
    }

    public void Setup()
    {
        GameContainer = GameObject.Find("Game");
        clickTable = GameObject.Find("ClickTable").GetComponent<ClickTable>();
        pizzaCutters = GameObject.Find("PizzaCutter").GetComponent<PizzaCutters>();
        canvas = GameObject.Find("Canvas");
        mainScript = GameObject.Find("main").GetComponent<Main>();
        mainScript.ResetPizzaValues();
        TooltipObj.SetActive(false);
        CreateTicketPosition();
        StartRandomTicketSpawning();
        UpdateMoneyUI();
        UpdateRatingsUI();
    }

    private void Update()
    {
        GamePlayUpdate();
    }

    public void GamePlayUpdate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            UnselectedEverything();
        }

        if (isTooltipOn)
        {
            TooltipObj.transform.position = GetTooltipPos();
        }

        CheckClickTableState();
    }

    private Vector3 GetTooltipPos()
    {
        Vector3 newPos = Input.mousePosition;
        float screenSizeX = Screen.width;
        RectTransform tooltipRect = TooltipObj.GetComponent<RectTransform>();
        float tooltipRectWidth = TooltipObj.GetComponent<RectTransform>().sizeDelta.x;
        float paddingOffset = 7f;
        Vector3 ttOffset = tooltipOffset;

        if (screenSizeX < (tooltipRectWidth + newPos.x + paddingOffset))
        {
            tooltipRect.pivot = new Vector2(1, 1);
            ttOffset = new Vector3(0, tooltipOffset.y, 0);
        } else
        {
            tooltipRect.pivot = new Vector2(0, 1);
        }

        newPos += ttOffset;

        return newPos;
    }

    private void StartRandomTicketSpawning()
    {
        GameObject ticketSpawnTimer = Instantiate(BaseTimerObj);
        ticketSpawnTimer.name = "ticketSpawnTimer";
        BaseTimer baseTimer = ticketSpawnTimer.GetComponent<BaseTimer>();
        baseTimer.theParent = this;
        baseTimer.callTimeoutMethodName = "CreateTicketSpawn";
        baseTimer.maxTime = 5f;
        baseTimer.timeLeft = 5f;
        baseTimer.StartTimer();
    }

    public void CreateTicketSpawn(string theName)
    {
        Quaternion rot = new Quaternion(0f, 0f, 0f, 0f);
        GameObject _ticket = Instantiate(TicketObj, ticketTimerPos, rot, GameContainer.transform);
        Ticket ticketScript = _ticket.GetComponent<Ticket>();
        ticketScript.maxRNG = maxCustomTicketRNG; // Adjust based on every minute, add more
        ticketScript.theParent = this;
        ticketScript.StartMakingTicket();
        CreateTicketTimer(_ticket.name, GreenTimer, YellowTimer, "TicketTimerTimeOut");
        tickets.Add(new Dictionary<string, ArrayList> { { _ticket.name, ticketScript.ticketOrder } });
        UpdateTicketsOnTable();

        float rngTime = Mathf.Lerp(maxTicketSpawnWaitTime, minTicketSpawnWaitTime, ratings / 5.0f);
        GameObject ticketSpawnTimer = Instantiate(BaseTimerObj);
        ticketSpawnTimer.name = "ticketSpawnTimer";
        BaseTimer baseTimer = ticketSpawnTimer.GetComponent<BaseTimer>();
        baseTimer.theParent = this;
        baseTimer.callTimeoutMethodName = "CreateTicketSpawn";
        baseTimer.maxTime = rngTime;
        baseTimer.timeLeft = rngTime;
        baseTimer.StartTimer();
    }

    public void TableClicked()
    {
        // no pizza on the table and no other pizza is selected.
        if (!isPizzaOnTable && pizzaSelected == "")
        {
            // add pizza dough
            isPizzaOnTable = true;
            Quaternion rot = new Quaternion(0f, 0f, 0f, 0f);
            GameObject newPizza = Instantiate(PizzaObj, pizzaOnTablePos, rot, GameContainer.transform);
            newPizza.name = RandomName("pizza");
            Pizza pizzaScript = newPizza.GetComponent<Pizza>();
            pizzaScript.theParent = this;
            // minus dough price
            float doughPrice = 0f;
            mainScript.prices.TryGetValue("dough", out doughPrice);
            UpdateMoney(doughPrice);
        }
        else if (!isPizzaOnTable && pizzaSelected != "")
        {
            // place selected pizza on table
            isPizzaOnTable = true;
            Transform pizzaSelectedObj = GameContainer.transform.Find(pizzaSelected);
            oven1Pizza = pizzaSelectedObj.position == pizzaInOven1Pos ? "" : oven1Pizza;
            oven2Pizza = pizzaSelectedObj.position == pizzaInOven2Pos ? "" : oven2Pizza;
            pizzaSelectedObj.position = pizzaOnTablePos;
            Pizza pizzaSelectedScript = pizzaSelectedObj.GetComponent<Pizza>();
            string pizzaState = pizzaSelectedScript.currentState;
            pizzaSelectedScript.isOnTable = true;
        }
        else
        {
            UnselectedEverything();
        }
    }

    public void PizzaClicked(string theName)
    {
        Pizza pizzaSelectedScript = GameContainer.transform.Find(theName).GetComponent<Pizza>();
        string pizzaSelectedState = pizzaSelectedScript.currentState;
        bool isPizzaCut = pizzaSelectedScript.isPizzaCut;
        bool isCurrentPizzaOnTable = pizzaSelectedScript.isOnTable;
        if (pizzaSelected != theName && foodSelected == "" && !pizzaCutterSelected)
        {
            // if food NOT select & pizza does NOT equal theName.
            // Select pizza
            UnselectedEverything();
            SelectPizza(theName);
        } 
        else if (foodSelected != "" && pizzaSelectedState == "raw" && isCurrentPizzaOnTable)
        {
            // minus price
            float foodPrice = 0f;
            mainScript.prices.TryGetValue(foodSelected, out foodPrice);
            float fixFoodPrice = Mathf.Round(foodPrice * 100) / 100;
            UpdateMoney(foodPrice);
            // if food is selected and clicked on pizza
            // Place cheese / sauce / topping
            if (foodSelected == "cheese")
            {
                pizzaSelectedScript.ShowCheese();
                UnselectedEverything();
            } 
            else if (foodSelected == "sauce")
            {
                pizzaSelectedScript.ShowSauce();
                UnselectedEverything();
            } 
            else
            {
                pizzaSelectedScript.PlaceTopping(foodSelected);
            }
        } 
        else if (pizzaCutterSelected && pizzaSelectedState == "cooked" && !isPizzaCut && isCurrentPizzaOnTable)
        {
            pizzaSelectedScript.CutPizza();
            UnselectedEverything();
        } 
        else
        {
            UnselectedEverything();
        }
    }


    private void SelectPizza(string theName)
    {
        pizzaSelected = theName;
        Pizza pizzaSelectedScript = GameContainer.transform.Find(pizzaSelected).GetComponent<Pizza>();
        pizzaSelectedScript.isSelected = true;
        string currentState = pizzaSelectedScript.currentState;
        pizzaSelectedScript.ChangeState(currentState);
    }


    public void FoodClicked(string theName)
    {
        SelectFood(theName);
    }


    private void SelectFood(string theName)
    {
        if (foodSelected == theName)
        {
            UnselectedEverything();
        }
        else
        {
            UnselectedEverything();
            foodSelected = theName;
            string foodSelectedContainer = foodSelected + "_container";
            FoodContainer foodSelectedScript = GameContainer.transform.Find(foodSelectedContainer).GetComponent<FoodContainer>();
            foodSelectedScript.isSelected = true;
            foodSelectedScript.UpdateSprite();
        }
    }

    public void PizzaCutterClicked()
    {
        if (pizzaCutterSelected)
        {
            UnselectedEverything();
        } 
        else
        {
            UnselectedEverything();
            pizzaCutterSelected = true;
            pizzaCutters.isSelected = true;
            pizzaCutters.UpdateSprite();
        }
    }

    public void ServingTrayClicked()
    {
        if (pizzaSelected != "")
        {
            Transform thePizza = GameContainer.transform.Find(pizzaSelected);
            Pizza pizzaSelectedScript = thePizza.GetComponent<Pizza>();
            string pizzaState = pizzaSelectedScript.currentState;
            bool isPizzaCut = pizzaSelectedScript.isPizzaCut;
            if (pizzaState == "cooked" &&  isPizzaCut)
            {
                // move pizza and update pizzaOnTray and start timer
                pizzaOnTray = pizzaSelected;
                UnselectedEverything();
                thePizza.position = pizzaOnTrayPos;
                CreateServingTimer();
                isPizzaOnTable = false;
                FindAndDestroyPizza();
            }
        }
        else
        {
            UnselectedEverything();
        }
    }

    private void CreateServingTimer()
    {
        GameObject newTimer = Instantiate(BaseTimerObj);
        BaseTimer timerScript = newTimer.GetComponent<BaseTimer>();
        float rngTime = Random.Range(3f, 5f);
        timerScript.theParent = this;
        timerScript.callTimeoutMethodName = "OnServingTrayTimeOut";
        timerScript.maxTime = rngTime;
        timerScript.timeLeft = rngTime;
        timerScript.StartTimer();
    }

    public void OnServingTrayTimeOut(string servingTrayTimerName)
    {
        GameObject thePizza = GameContainer.transform.Find(pizzaOnTray).gameObject;
        Pizza pizzaSelectedScript = thePizza.GetComponent<Pizza>();

        ArrayList pizzaOrder = pizzaSelectedScript.CheckPizzaOrder();

        CheckTicketOrder(pizzaOrder);
        thePizza.SetActive(false);
        pizzaOnTray = "";
    }

    private void FindAndDestroyPizza()
    {
        foreach (Transform child in GameContainer.transform)
        {
            string first5 = child.name.Substring(0,5);
            
            if (first5 == "pizza")
            {
                if (!child.gameObject.activeSelf)
                {
                    Destroy(child.gameObject);
                }
            }
        }
    }

    public void TrashCanClicked()
    {
        if (pizzaSelected != "")
        {
            GameObject thePizza = GameContainer.transform.Find(pizzaSelected).gameObject;
            Vector3 thePizzaPos = thePizza.transform.position;
            UnselectedEverything();
            thePizza.SetActive(false);

            if (thePizzaPos == pizzaOnTablePos)
            {
                isPizzaOnTable = false;
            }

            oven1Pizza = thePizzaPos == pizzaInOven1Pos ? "" : oven1Pizza;
            oven2Pizza = thePizzaPos == pizzaInOven2Pos ? "" : oven2Pizza;
        }
        else
        { 
            UnselectedEverything(); 
        }
    }

    public void VoidClicked()
    {
        UnselectedEverything();
    }


    private void UnselectedEverything()
    {
        if (pizzaSelected != "")
        {
            Pizza pizzaSelectedScript = GameContainer.transform.Find(pizzaSelected).GetComponent<Pizza>();
            pizzaSelectedScript.isSelected = false;
            string currentState = pizzaSelectedScript.currentState;
            pizzaSelectedScript.ChangeState(currentState);
            pizzaSelected = "";
        }
        if (foodSelected != "")
        {
            string foodSelectedContainer = foodSelected + "_container";
            FoodContainer foodSelectedScript = GameContainer.transform.Find(foodSelectedContainer).GetComponent<FoodContainer>();
            foodSelectedScript.isSelected = false;
            foodSelectedScript.UpdateSprite();
            foodSelected = "";
        }
        if (pizzaCutterSelected)
        {
            pizzaCutters.isSelected = false;
            pizzaCutters.UpdateSprite();
            pizzaCutterSelected = false;
        }
    }

    private string RandomName(string baseName)
    {
        string newName = baseName;
        int rng = Random.Range(0, int.MaxValue);
        newName += rng.ToString();
        Transform checkName = GameContainer.transform.Find(newName);
        if (checkName != null)
        {
            Debug.Log("That name exist");
            newName = RandomName(baseName);
        }

        return newName;
    }


    public void OvenClicked(string ovenName)
    {
        Oven currentOven = GameContainer.transform.Find(ovenName).GetComponent<Oven>();
        bool canClickDoors = currentOven.canClickDoors;
        bool isCooking = currentOven.isCooking;
        string pizzainovenpath = ovenName + "Pizza";
        string pizzainoven = (string)this.GetType().GetField(pizzainovenpath).GetValue(this);
        if (pizzainoven == "" && pizzaSelected != "") 
        {
            // no pizza in the oven and pizza has been selected
            Transform pizzaSelectedObj = GameContainer.transform.Find(pizzaSelected);
            Pizza pizzaSelectedScript = pizzaSelectedObj.GetComponent<Pizza>();
            string pizzaState = pizzaSelectedScript.currentState;
            if (pizzaState == "raw")
            {
                // raw pizza goes into the oven
                this.GetType().GetField(pizzainovenpath).SetValue(this, pizzaSelected); // set the oven#Pizza to pizzaSelected
                pizzaSelectedObj.position = ovenName == "oven1" ? pizzaInOven1Pos : pizzaInOven2Pos;
                pizzaSelectedScript.isOnTable = false;
                pizzaSelectedScript.EnableClickArea(false);

                isPizzaOnTable = false;
            }
            UnselectedEverything();
        }
        else if (pizzainoven != "")
        {
            Transform pizzaSelectedObj = GameContainer.transform.Find(pizzainoven);
            Pizza pizzaSelectedScript = pizzaSelectedObj.GetComponent<Pizza>();
            string pizzaState = pizzaSelectedScript.currentState;
            if (pizzaState == "raw" && canClickDoors && !isCooking)
            {
                // pizza is in oven, close door, and create timer
                currentOven.canClickDoors = false;
                currentOven.isCooking = true;
                currentOven.ChangeSpriteOpen(false);

                pizzaSelectedObj.position = OvenPizzaCookingPos;

                Vector3 ovenTimerPos = ovenName == "oven1" ? Oven1TimerPos : Oven2TimerPos;
                CreateOvenTimer(ovenName, ovenTimerPos, YellowTimer, GreenTimer, "OvenTimerTimeOut");
            }
            else if (pizzaState != "raw" && canClickDoors && isCooking)
            {
                // pizza is done cooking, can open doors, move pizza back and able to take pizza out
                currentOven.isCooking = false;
                currentOven.ChangeSpriteOpen(true);

                pizzaSelectedObj.position = ovenName == "oven1" ? pizzaInOven1Pos : pizzaInOven2Pos;
                pizzaSelectedScript.EnableClickArea(true);


                TimerUI currentOvenTimer = canvas.transform.Find(ovenName + "_timer").GetComponent<TimerUI>();
                currentOvenTimer.StopAndDestroy();
            }
        }
        else
        {
            UnselectedEverything();
        }
    }

    public void ShowOvenToolTip(string ovenName)
    {
        string msg = "Oven";
        string isPizzaInOven = (ovenName == "oven1") ? oven1Pizza : oven2Pizza;
        Oven ovenScript = GameContainer.transform.Find(ovenName).GetComponent<Oven>();
        bool canOvenClickDoors = ovenScript.canClickDoors;
        bool isOvenCooking = ovenScript.isCooking;

        if (isPizzaInOven == "" && pizzaSelected != "")
        {
            // no pizza in oven, pizza is selected, pizza selected is raw
            Pizza pizzaSelectedScript = GameContainer.transform.Find(pizzaSelected).GetComponent<Pizza>();
            string pizzaState = pizzaSelectedScript.currentState;
            if (pizzaState == "raw")
            {
                msg = "Place pizza in oven";
            }
            else
            {
                msg = "You can't place that in here";
            }
        }
        else if (isPizzaInOven != "" && pizzaSelected != "")
        {
            msg = "You can't place that in here";
        }
        else if (isPizzaInOven != "" && !isOvenCooking && canOvenClickDoors)
        {
            Pizza pizzaSelectedScript = GameContainer.transform.Find(isPizzaInOven).GetComponent<Pizza>();
            string pizzaState = pizzaSelectedScript.currentState;
            if (pizzaState == "raw")
            {
                msg = "Close oven door";
            } else
            {
                msg = "Take Pizza Out";
            }
        }
        else if (isPizzaInOven != "" &&  isOvenCooking && !canOvenClickDoors)
        {
            msg = "Cooking...";
        } 
        else if (isPizzaInOven != "" && isOvenCooking && canOvenClickDoors)
        {
            msg = "Open oven doors";
        }

        ShowTooltip(msg);
    }

    private void CreateOvenTimer(string ovenName, Vector3 ovenTimerPos, Color startColor, Color endColor, string timeOutMethodName)
    {
        GameObject newTimer = Instantiate(TimerUIObj, canvas.transform);
        TimerUI timerScript = newTimer.GetComponent<TimerUI>();
        timerScript.theParent = this;
        newTimer.name = ovenName + "_timer";
        timerScript.callTimeoutMethodName = timeOutMethodName;
        timerScript.SetPositionAndSize(ovenTimerPos, OvenTimerSize);
        timerScript.SetNewTime(10f);
        timerScript.SetColor(startColor, endColor);
        timerScript.StartTimer();
    }

    public void OvenTimerTimeOut(string theName)
    {
        GameObject theTimer = canvas.transform.Find(theName).gameObject;
        Destroy(theTimer);
        string ovenName = theName.Split("_")[0];
        Oven currentOven = GameContainer.transform.Find(ovenName).GetComponent<Oven>();

        string pizzaInOvenName = ovenName == "oven1" ? oven1Pizza : oven2Pizza;
        Pizza pizzaSelectedScript = GameContainer.transform.Find(pizzaInOvenName).GetComponent<Pizza>();
        string pizzaState = pizzaSelectedScript.currentState;

        if (pizzaState == "raw")
        {
            // cook pizza, allow oven to be open, create another timer for burnt pizza
            pizzaSelectedScript.ChangeState("cooked");
            currentOven.canClickDoors = true;
            Vector3 ovenTimerPos = ovenName == "oven1" ? Oven1TimerPos : Oven2TimerPos;
            CreateOvenTimer(ovenName, ovenTimerPos, GreenTimer, RedTimer, "OvenTimerTimeOut");
        }
        else
        {
            // pizza burnt.
            pizzaSelectedScript.ChangeState("burnt");
        }
    }

    private void EverythingClickable(bool boolValue)
    {
        foreach (Transform child in GameContainer.transform)
        {
            BoxCollider2D bc = child.GetComponent<BoxCollider2D>();
            CircleCollider2D cc = child.GetComponent<CircleCollider2D>();
            if (bc != null)
            {
                bc.enabled = boolValue;
            }
            else if (cc != null)
            {
                cc.enabled = boolValue;
            }
        }

        // check and keep pizza nonclickable if pizza is in oven
        if (oven1Pizza != "")
        {
            Transform getPizza = GameContainer.transform.Find(oven1Pizza);
            Pizza getPizzaScript = getPizza.GetComponent<Pizza>();
            string pizzaState = getPizzaScript.currentState;
            getPizza.GetComponent<CircleCollider2D>().enabled = pizzaState == "raw" ? false : boolValue;
        }

        if (oven2Pizza != "")
        {
            Transform getPizza = GameContainer.transform.Find(oven2Pizza);
            Pizza getPizzaScript = getPizza.GetComponent<Pizza>();
            string pizzaState = getPizzaScript.currentState;
            getPizza.GetComponent<CircleCollider2D>().enabled = pizzaState == "raw" ? false : boolValue;
        }
    }

    private void SetRunningTimer(bool boolValue)
    {
        foreach (Transform child in canvas.transform)
        {
            TimerUI getTimerUI = child.GetComponent<TimerUI>();
            if (getTimerUI != null)
            {
                getTimerUI.isRunning = boolValue;
            }
        }
    }

    private void CreateTicketPosition()
    {
        ticketPositions[0] = new Vector3(-3.43f, -2.78f);
        ticketPositions[1] = new Vector3(-3.43f, -2.08f);
        ticketPositions[2] = new Vector3(-3.43f, -1.38f);
        ticketPositions[3] = new Vector3(-3.43f, -0.68f);
        ticketPositions[4] = new Vector3(-3.43f, 0.02f);
        ticketPositions[5] = new Vector3(-3.43f, 0.72f);
        ticketPositions[6] = new Vector3(-3.43f, 1.42f);

        ticketUIPositions[0] = new Vector3(-343f, -278f);
        ticketUIPositions[1] = new Vector3(-343f, -208f);
        ticketUIPositions[2] = new Vector3(-343f, -138f);
        ticketUIPositions[3] = new Vector3(-343f, -68f);
        ticketUIPositions[4] = new Vector3(-343f, 2f);
        ticketUIPositions[5] = new Vector3(-343f, 72f);
        ticketUIPositions[6] = new Vector3(-343f, 142f);
    }

    private void CreateTicketTimer(string ticketName, Color startColor, Color endColor, string timeOutMethodName)
    {
        GameObject newTimer = Instantiate(TimerUIObj, canvas.transform);
        TimerUI timerScript = newTimer.GetComponent<TimerUI>();
        timerScript.theParent = this;
        newTimer.name = ticketName + "-timer";
        timerScript.callTimeoutMethodName = timeOutMethodName;
        timerScript.SetPositionAndSize(ticketTimerPos, ticketTimerSize);
        timerScript.SetNewTime(ticketWaitTime);
        timerScript.SetColor(startColor, endColor);
        timerScript.StartTimer();
    }

    private void UpdateTicketsOnTable()
    {
        if (tickets.Count > 0)
        {
            int count = 0;
            for (int i = 0; i < tickets.Count; i++)
            {
                Dictionary<string,ArrayList> ticketKey = (Dictionary<string, ArrayList>)tickets[i];
                Dictionary<string, ArrayList>.KeyCollection ticketNames = ticketKey.Keys;
                string ticketName = ticketNames.ToArrayPooled()[0];
                if (count < ticketPositions.Length)
                {
                    GameContainer.transform.Find(ticketName).position = ticketPositions[count];
                    canvas.transform.Find(ticketName + "-timer").localPosition = ticketUIPositions[count];
                }
                count++;
            }
        }
    }

    public void TicketTimerTimeOut(string theName)
    {
        GameObject theTicketTimer = canvas.transform.Find(theName).gameObject;
        bool isInTicketTimerEnded = ticketTimerEnded.Contains(theName);
        if (isInTicketTimerEnded)
        {
            ticketTimerEnded.Remove(theName);
            ticketFailed.Add(theName);
        } else
        {
            ticketTimerEnded.Add(theName);
            string ticketName = theName.Split("-")[0];
            CreateTicketTimer(ticketName,YellowTimer, RedTimer, "TicketTimerTimeOut");
            Destroy(theTicketTimer);
            Invoke("UpdateTicketsOnTable", 0.1f);
        }
    }

    public void TicketClicked(string theName)
    {
        Ticket currentTicket = GameContainer.transform.Find(theName).GetComponent<Ticket>();
        ArrayList ticketOrder = currentTicket.ticketOrder;
        int ticketNumber = currentTicket.ticketNumber;
        TicketUI TicketUIObjScript = TicketUIObj.gameObject.GetComponent<TicketUI>();
        TicketUIObjScript.OnShowUI(theName, ticketOrder, ticketNumber);
        ShowTicketUI();
    }

    private void CheckTicketOrder(ArrayList thePizzaOrder)
    {
        tip = MAX_TIP;
        ticketRatings = 5.0f;
        int ticketCount = 0;

        customerReview = new ArrayList();

        foreach (Dictionary<string, ArrayList> ticket in tickets)
        {
            bool isMatching = false;
            string ticketKey = ticket.Keys.ToArrayPooled()[0];
            ArrayList ticketOrder = ticket[ticketKey];

            if (thePizzaOrder.Count != ticketOrder.Count)
            {
                // count doesnt equal
                ticketCount++;
                continue;
            }

            if (thePizzaOrder.Count > 1)
            {
                if (ticketOrder[0] != thePizzaOrder[0])
                {
                    // if sauce and cheese are in the correct order
                    ticketCount++;
                    continue;
                }
            }

            Dictionary<string, int> ticketToppings = (Dictionary<string, int>)ticketOrder[ticketOrder.Count - 1];
            Dictionary<string, int> pizzaToppings = (Dictionary<string, int>)thePizzaOrder[thePizzaOrder.Count - 1];
            if (ticketToppings.Count != pizzaToppings.Count)
            {
                ticketCount++;
                continue;
            }

            isMatching = CheckTicketToppings(ticketKey,pizzaToppings,ticketToppings);

            if (isMatching)
            {
                mainScript.pizzasMade += 1;
                AddTicketsOrdered(ticketKey);
                CheckHowLongPizzaTook(ticketKey);
                RemoveTicket(ticketKey, ticketCount);
                GetPaid();
                GiveRating();
                GiveReview();
                CheckFailed();
                break;
            }

            ticketCount++;
        }
    }

    private bool CheckTicketToppings(string ticketKey, Dictionary<string, int> pizzaToppings, Dictionary<string, int> ticketToppings)
    {
        foreach (string tt in ticketToppings.Keys)
        {
            if (pizzaToppings.ContainsKey(tt))
            {
                // check amount
                int ticketAmount = ticketToppings[tt];
                int maxAmount = ticketAmount * MAX_TOPPING_MULTIPLIER;
                int pizzaAmount = pizzaToppings[tt];
                if (pizzaAmount < ticketAmount)
                {
                    float tipRateAffected = pizzaAmount / ticketAmount;
                    tip *= tipRateAffected;
                    ticketRatings *= tipRateAffected;
                    ticketRatings -= 1.0f - tipRateAffected;
                    // customer comment, need_more_toppings_review
                    AddCustomerComment(mainScript.needMoreToppingsReview);
                }
                else if (pizzaAmount > maxAmount)
                {
                    float tipRateAffected = maxAmount / pizzaAmount;
                    tip *= tipRateAffected;
                    ticketRatings *= tipRateAffected;
                    ticketRatings -= 1.0f - tipRateAffected;
                    // customer comment, need_less_toppings_review
                    AddCustomerComment(mainScript.needLessToppingsReview);
                }
            }
            else
            {
                return false;
            }
        }
        return true;
    }

    private void AddTicketsOrdered(string theTicketName)
    {
        string baseName = string.Empty;
        foreach (char c in  theTicketName)
        {
            if (!char.IsDigit(c))
            {
                baseName += c.ToString();
            }
        }
        mainScript.AddTicketOrderValue(baseName);
    }

    private void RemoveTicket(string ticketKey, int ticketCount)
    {
        GameObject currentTicket = GameContainer.transform.Find(ticketKey).gameObject;
        GameObject currentTicketTImer = canvas.transform.Find(ticketKey + "-timer").gameObject;
        Destroy(currentTicket);
        Destroy(currentTicketTImer);
        tickets.RemoveAt(ticketCount);
        UpdateTicketsOnTable();
    }

    private void CheckHowLongPizzaTook(string ticketKey)
    {
        string timerName = ticketKey + "-timer";
        bool isInTicketTimerEnded = ticketTimerEnded.Contains(timerName);
        bool isInTicketFailed = ticketFailed.Contains(timerName);

        TimerUI ticketTimer = canvas.transform.Find(timerName).GetComponent<TimerUI>();
        float timeLeft = ticketTimer.timeLeft;

        float amountLeft = timeLeft / ticketWaitTime;

        if (ticketRatings > 0.0f && !isInTicketTimerEnded && !isInTicketFailed)
        {
            float checkTimeLeft = amountLeft * 2.0f;
            if (checkTimeLeft < 1.0f)
            {
                ticketRatings *= checkTimeLeft;
                // customer comment, little_slow_review
                AddCustomerComment(mainScript.littleSlowReview);
            }
        }

        if (isInTicketTimerEnded)
        {
            ticketTimerEnded.Remove(ticketKey);
            tip = tip / 2.0f;
            ticketRatings = ticketRatings / 2.0f;
            // customer comment, very_slow_review
            AddCustomerComment(mainScript.verySlowReview);
        }

        tip *= amountLeft;

        if (isInTicketFailed)
        {
            ticketFailed.Remove(ticketKey);
            tip = 0.0f;
            ticketRatings = 0.0f;
            // customer comment, failed_review
            AddCustomerComment(mainScript.failedReview);
        }
    }

    private void AddCustomerComment(ArrayList globalReviewList)
    {
        int rng = Random.Range(0, globalReviewList.Count);
        string comment = (string)globalReviewList[rng];
        customerReview.Add(comment);
    }

    private void GetPaid()
    {
        float amountPaid = PIZZA_PRICE + tip;
        float fixAmountPaid = Snapping.Snap(amountPaid, 0.01f);
        UpdateMoney(fixAmountPaid);
    }

    private void UpdateMoney(float moneyAmount)
    {
        money += moneyAmount;
        GameObject updateMoneyText = Instantiate(UpdateMoneyTextObj,canvas.transform);
        UpdateMoneyText updateMoneyTextScript = updateMoneyText.GetComponent<UpdateMoneyText>();
        updateMoneyTextScript.SetupText(moneyAmount);
        UpdateMoneyUI();
    }

    private void UpdateMoneyUI()
    {
        float snappedMoney = Snapping.Snap(money, 0.01f);
        string moneyText = mainScript.FixDecimal(snappedMoney.ToString());
        MoneyText.GetComponent<TextMeshProUGUI>().text = "$ " + moneyText;
    }

    private void GiveRating()
    {
        if (reviewRatings.Count == MAX_AMOUNT_RATINGS)
        {
            reviewRatings.RemoveAt(0);
        }

        if (ticketRatings < 0f)
        {
            ticketRatings = 0f;
        }

        reviewRatings.Add(ticketRatings);

        float addAllRatings = 0.0f;
        float reviewRatingsCount = reviewRatings.Count;

        foreach(float r in reviewRatings)
        {
            addAllRatings += r;
        }

        float newRatings = addAllRatings / reviewRatingsCount;

        float ratingDifference = newRatings - ratings;

        ratings = newRatings;

        UpdateRatings(ratingDifference);
    }

    private void UpdateRatings(float ratingDifference)
    {
        GameObject ratingsText = Instantiate(UpdateRatingsTextObj, canvas.transform);
        UpdateRatingText updateRatingText = ratingsText.GetComponent<UpdateRatingText>();

        if (ratingDifference == 0f && ratings == 5f)
        {
            updateRatingText.SetupMaxText();
        } else
        {
            float snappedAmount = Snapping.Snap(ratingDifference, 0.01f);
            updateRatingText.SetupText(snappedAmount);
        }

        UpdateRatingsUI();
    }

    public void UpdateRatingsUI()
    {
        float snappedRatings = Snapping.Snap(ratings, 0.01f);
        string ratingText = mainScript.FixDecimal(snappedRatings.ToString());
        TextMeshProUGUI ratingTextMesh = RatingsText.GetComponent<TextMeshProUGUI>();
        ratingTextMesh.text = ratingText;
    }

    public virtual void CheckFailed()
    {
        if (ratings < 1.0f)
        {
            normalCheckFailed();
        }
    }

    public void normalCheckFailed()
    {
        // disable pause button
        SetRunningTimer(false);
        EverythingClickable(false);
        mainScript.profit = money;
        mainScript.EndGameTime();
        Instantiate(GameOverObj, canvas.transform);
    }

    private void GiveReview()
    {
        ReviewUI reviewUI = ReviewPanelObj.GetComponent<ReviewUI>();
        string comment = "";

        if (customerReview.Count == 0)
        {
            int rng = Random.Range(0, mainScript.perfectReview.Count);
            comment = (string)mainScript.perfectReview[rng];
        }
        else
        {
            foreach (var cr in customerReview)
            {
                comment += (string)cr + "\n";
            }
            comment = comment.Trim();
        }

        float snappedTicketRatings = Snapping.Snap(ticketRatings, 0.01f);

        reviewUI.ShowReviewUI(comment, snappedTicketRatings);
    }

    public void DayDone()
    {
        Debug.Log("Day done");
    }

    private void ShowTicketUI()
    {
        EverythingClickable(false);
        TicketUI TicketUIObjScript = TicketUIObj.gameObject.GetComponent<TicketUI>();
        TicketUIObjScript.ShowUI(true);
    }

    public void HideTicketUI()
    {
        EverythingClickable(true);
        TicketUI TicketUIObjScript = TicketUIObj.gameObject.GetComponent<TicketUI>();
        TicketUIObjScript.ShowUI(false);
    }

    public void ShowTooltip(string msg)
    {
        isTooltipOn = true;
        TooltipObj.SetActive(true);
        TextMeshProUGUI tooltipText = TooltipTextObj.GetComponent<TextMeshProUGUI>();
        tooltipText.text = msg;
        float howLong = msg.Length * 12.5f;
        RectTransform tooltipRect = TooltipObj.GetComponent<RectTransform>();
        tooltipRect.sizeDelta = new Vector2(howLong, 32f);
        RectTransform tooltipTextRect = TooltipTextObj.GetComponent<RectTransform>();
        tooltipTextRect.sizeDelta = new Vector2(howLong, 32f);
    }

    public void ShowTableTooltip()
    {
        if (isPizzaOnTable)
        {
            return;
        }
        string msg = "Create Pizza Dough";
        
        if (pizzaSelected != "")
        {
            msg = "Move Pizza here";
        }

        ShowTooltip(msg);
    }

    public void ShowServingTrayTooltip()
    {
        string msg = "Serving Tray";
        if (pizzaSelected != "")
        {
            Pizza thePizza = GameContainer.transform.Find(pizzaSelected).GetComponent<Pizza>();
            if (thePizza.isPizzaCut)
            {
                msg = "Serve Pizza";
            }
        }
        ShowTooltip(msg);
    }

    private void CheckClickTableState()
    {
        // in Update()
        if (isPizzaOnTable)
        {
            clickTable.SetAddVisible(false);
        } else
        {
            clickTable.SetAddVisible(true);
            if (pizzaSelected != "")
            {
                clickTable.SetSprite(false);
            }
            else
            {
                clickTable.SetSprite(true);
            }
        }
    }

    public void HideTooltip()
    {
        isTooltipOn = false;
        TooltipObj.SetActive(false);
    }*/
}
