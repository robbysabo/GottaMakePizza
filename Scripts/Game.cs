using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Game : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject PizzaObj;
    public GameObject DarkToClear;
    public GameObject TimerUIObj;
    public GameObject TicketObj;
    public GameObject TicketUIObj;
    //public GameObject UpdateMoneyTextObj;
    //public GameObject UpdateRatingsTextObj;
    //public GameObject MoneyText;
    //public GameObject RatingsText;
    public GameObject GameOverObj;
    public GameObject BaseTimerObj;
    public GameObject TooltipObj;
    public GameObject TooltipTextObj;
    //public GameObject ReviewPanelObj;

    // game play vars
    [Header("Game Play Vars")]
    public float tip = 0f;
    public const float MAX_TIP = 5.0f;
    public float money = 0f;
    public const float PIZZA_PRICE = 10f;
    public float ratings = 2.5f;
    public ArrayList reviewRatings = new ArrayList { 2.5f, 2.5f, 2.5f };
    public const int MAX_AMOUNT_RATINGS = 8;
    public float ticketRatings = 5.0f;
    public const int MAX_TOPPING_MULTIPLIER = 2;
    public ArrayList ticketTimerEnded = new ArrayList();
    public ArrayList ticketFailed = new ArrayList();
    public string oven1Pizza = "";
    public string oven2Pizza = "";

    private Main mainScript;
    private GameObject GameContainer;
    private GameObject canvas;
    private GameObject tooltipCanvas;
    private PizzaCutters pizzaCutters;
    private ClickTable clickTable;

    private float maxTicketSpawnWaitTime = 32f;
    private float minTicketSpawnWaitTime = 7f;
    private bool isTooltipOn = false;
    private Vector3 tooltipOffset = new Vector3(15, -15, 0);
    private ArrayList customerReview = new ArrayList();


    // pizza vars
    private string pizzaSelected = "";
    private bool isPizzaOnTable = false;
    private Vector3 pizzaOnTablePos = new Vector3(-0.095f, -0.41f, 0f);
    private Vector3 pizzaInOven1Pos = new Vector3(1.015f, -0.43f, 0f);
    private Vector3 pizzaInOven2Pos = new Vector3(1.015f, 0.2f, 0f);
    private Vector3 pizzaOnTrayPos = new Vector3(-1.195f, -0.12f);
    private bool pizzaCutterSelected = false;
    private string pizzaOnTray = "";


    // ingredient vars
    private string foodSelected = "";


    // timer ui vars
    private Color YellowTimer = new Color(0.9569f, 0.8863f, 0.3373f, 1f);
    private Color GreenTimer = new Color(0.2471f, 0.6f, 0.2471f, 1f);
    private Color RedTimer = new Color(0.7686f, 0.149f, 0.1569f, 1f);


    // oven timer
    private Vector2 OvenTimerSize = new Vector2(32f, 32f);
    private Vector3 Oven1TimerPos = new Vector3(109.5f, -32f, 0f);
    private Vector3 Oven2TimerPos = new Vector3(109.5f, 31f, 0f);
    private Vector3 OvenPizzaCookingPos = new Vector3(9000f, 9000f, 0f); // offscreen


    // ticket+order vars
    private int maxCustomTicketRNG = 101;
    private Vector3[] ticketPositions = new Vector3[6];
    private ArrayList tickets = new ArrayList();
    private Vector3[] ticketUIPositions = new Vector3[6]; // X = -85.5
    private Vector2 ticketTimerSize = new Vector2(8f, 8f);
    private float ticketWaitTime = 120f; // ####################################### Testing
    private Vector3 ticketTimerPos = new Vector3(9000f, 9000f, 0f);


    private void Start()
    {
        Setup();
    }

    public void Setup()
    {
        GameContainer = transform.Find("Game").gameObject;
        clickTable = GameContainer.transform.Find("ClickTable").GetComponent<ClickTable>();
        pizzaCutters = GameContainer.transform.Find("PizzaCutter").GetComponent<PizzaCutters>();
        canvas = transform.Find("Canvas").gameObject;
        tooltipCanvas = transform.Find("Canvas2").gameObject;
        mainScript = GameObject.Find("main").GetComponent<Main>();
        mainScript.ResetPizzaValues();
        TooltipObj.SetActive(false);
        CreateTicketPosition();
        StartRandomTicketSpawning();
        //UpdateMoneyUI();
        //UpdateRatingsUI();

        // Dark to clear
        Instantiate(DarkToClear, canvas.transform);
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

    private void UpdateTicketsOnTable()
    {
        if (tickets.Count > 0)
        {
            int count = 0;
            for (int i = 0; i < tickets.Count; i++)
            {
                Dictionary<string, ArrayList> ticketKey = (Dictionary<string, ArrayList>)tickets[i];
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



    private void FixedUpdate()
    {
        BaseGameUpdate();
    }

    public void BaseGameUpdate()
    {
        if (Input.GetMouseButtonDown(1))
        {
            // right click anytime to unselect everything
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
        }
        else
        {
            tooltipRect.pivot = new Vector2(0, 1);
        }

        newPos += ttOffset;

        return newPos;
    }

    private void CreateTicketPosition()
    {
        ticketPositions[0] = new Vector3(-0.855f, -0.6f);
        ticketPositions[1] = new Vector3(-0.855f, -0.41f);
        ticketPositions[2] = new Vector3(-0.855f, -0.22f);
        ticketPositions[3] = new Vector3(-0.855f, -0.03f);
        ticketPositions[4] = new Vector3(-0.855f, 0.16f);
        ticketPositions[5] = new Vector3(-0.855f, 0.35f);

        ticketUIPositions[0] = new Vector3(-85.5f, -60f);
        ticketUIPositions[1] = new Vector3(-85.5f, -41f);
        ticketUIPositions[2] = new Vector3(-85.5f, -22f);
        ticketUIPositions[3] = new Vector3(-85.5f, -3f);
        ticketUIPositions[4] = new Vector3(-85.5f, 16f);
        ticketUIPositions[5] = new Vector3(-85.5f, 35f);
    }

    private void CheckClickTableState()
    {
        // in Update()
        if (isPizzaOnTable)
        {
            clickTable.SetAddVisible(false);
        }
        else
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


    public void TableClicked()
    {
        // no pizza on the table and no other pizza is selected.
        if (!isPizzaOnTable && pizzaSelected == "")
        {
            // add pizza dough
            print("is clicking though");
            isPizzaOnTable = true;
            Quaternion rot = new Quaternion(0f, 0f, 0f, 0f);
            GameObject newPizza = Instantiate(PizzaObj, pizzaOnTablePos, rot, GameContainer.transform);
            newPizza.name = RandomName("pizza");
            Pizza pizzaScript = newPizza.GetComponent<Pizza>();
            pizzaScript.theParent = this;
            // minus dough price
            /*float doughPrice = 0f;
            mainScript.prices.TryGetValue("dough", out doughPrice);
            UpdateMoney(doughPrice);*/
            UnselectedEverything();
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
            UnselectedEverything();
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
            /*// minus price
            float foodPrice = 0f;
            mainScript.prices.TryGetValue(foodSelected, out foodPrice);
            float fixFoodPrice = Mathf.Round(foodPrice * 100) / 100;
            UpdateMoney(foodPrice);*/
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

                TimerUI currentOvenTimer = null;
                currentOvenTimer = canvas.transform.Find(ovenName + "_timer").GetComponent<TimerUI>();
                if (currentOvenTimer != null)
                {
                    currentOvenTimer.StopAndDestroy();
                }
            }
        }
        else
        {
            UnselectedEverything();
        }
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

    public void TicketClicked(string theName)
    {
        Ticket currentTicket = GameContainer.transform.Find(theName).GetComponent<Ticket>();
        ArrayList ticketOrder = currentTicket.ticketOrder;
        int ticketNumber = currentTicket.ticketNumber;
        TicketUI TicketUIObjScript = TicketUIObj.gameObject.GetComponent<TicketUI>();
        TicketUIObjScript.OnShowUI(theName, ticketOrder, ticketNumber);
        ShowTicketUI();
    }

    public void ServingTrayClicked()
    {
        if (pizzaSelected != "")
        {
            Transform thePizza = GameContainer.transform.Find(pizzaSelected);
            Pizza pizzaSelectedScript = thePizza.GetComponent<Pizza>();
            string pizzaState = pizzaSelectedScript.currentState;
            bool isPizzaCut = pizzaSelectedScript.isPizzaCut;
            if (pizzaState == "cooked" && isPizzaCut)
            {
                // move pizza and update pizzaOnTray and start timer
                pizzaOnTray = pizzaSelected;
                UnselectedEverything();
                thePizza.position = pizzaOnTrayPos;
                CreateServingTimer();
                isPizzaOnTable = false;
                //FindAndDestroyPizza();
                /* DEBUG ONLY */
                //thePizza.gameObject.SetActive(false); /* DEBUG ONLY */
                /* END DEBUG ONLY */
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
        float howLong = msg.Length * 3f;
        RectTransform tooltipRect = TooltipObj.GetComponent<RectTransform>();
        tooltipRect.sizeDelta = new Vector2(howLong, 6f);
        TooltipObj.transform.position = GetTooltipPos();
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
            }
            else
            {
                msg = "Take Pizza Out";
            }
        }
        else if (isPizzaInOven != "" && isOvenCooking && !canOvenClickDoors)
        {
            msg = "Cooking...";
        }
        else if (isPizzaInOven != "" && isOvenCooking && canOvenClickDoors)
        {
            msg = "Open oven doors";
        }

        ShowTooltip(msg);
    }

    public void HideTooltip()
    {
        isTooltipOn = false;
        TooltipObj.SetActive(false);
    }
}
