using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;

public class Pizza : MonoBehaviour
{
    // need to get game objects in inspector
    [Header("Drag gameobjects here")]
    public GameObject toppingObj;

    // filled in by script
    [Header("Filled in by script")]
    public Game theParent;
    public SpriteRenderer spriteRenderer;

    // theParent needs to know the state
    [Header("For parent to get info")]
    public bool isClickable = true;
    public bool isSelected = false;
    public string currentState = "raw";
    public bool hasCheese = false;
    public bool hasSauce = false;
    public bool isPizzaCut = false;
    public bool isOnTable = true;
    public Dictionary<String, int> toppings = new Dictionary<String, int>();

    // for pizza creations.
    private GameObject toppingSpawnArea;
    private GameObject baseContainer;
    private GameObject cheeseObj;
    private GameObject toppingContainer;
    private GameObject sauceObj;
    private GameObject pizzaCutObj;
    private GameObject pizzaSelected;
    private Vector2[] toppingPolygon;
    private Vector2 minToppingSpawn = new Vector2(-0.8968046f, -0.9144961f);
    private Vector2 maxToppingSpawn = new Vector2(0.8909959f, 0.9399394f);
    private string texturePath = "Art/SpriteSheet";
    private int currentSauceCheeseLayer = 0;
    private ArrayList pizzaOrder = new ArrayList();


    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        toppingSpawnArea = transform.Find("spawnToppingArea").gameObject;
        toppingContainer = transform.Find("toppings").gameObject;
        baseContainer = transform.Find("base").gameObject;
        cheeseObj = baseContainer.transform.Find("cheese").gameObject;
        sauceObj = baseContainer.transform.Find("sauce").gameObject;
        pizzaCutObj = transform.Find("pizzacut").gameObject;
        pizzaSelected = transform.Find("pizza_selected").gameObject;
        pizzaSelected.SetActive(false);
        pizzaCutObj.SetActive(false);
        cheeseObj.SetActive(false);
        sauceObj.SetActive(false);
        CreatePolygon();
    }

    private void OnMouseDown()
    {
        if (isClickable)
        {
            theParent.PizzaClicked(name);
        }
    }

    /*private void OnMouseOver()
    {
        if (isClickable)
        {
            
        }
    }

    private void OnMouseExit()
    {
        theParent.HideTooltip();
    }*/

    private void CreatePolygon()
    {
        PolygonCollider2D polygonCollider2D = toppingSpawnArea.GetComponent<PolygonCollider2D>();
        toppingPolygon = polygonCollider2D.points;
        toppingSpawnArea.SetActive(false);
    }

    private Sprite GetSpriteByName(string basename, bool addSelected)
    {
        string currentName = basename + "_" + currentState;

        if (addSelected)
        {
            currentName += "_selected";
        }

        Sprite[] sprites = Resources.LoadAll<Sprite>(texturePath);
        int findSprite = Array.FindIndex(sprites, s => s.name == currentName);
        return sprites[findSprite];
    }


    public void ShowSauce()
    {
        if (!hasSauce)
        {
            sauceObj.SetActive(true);
            hasSauce = true;
            SpriteRenderer sr = sauceObj.GetComponent<SpriteRenderer>();
            sr.sortingOrder = currentSauceCheeseLayer;
            currentSauceCheeseLayer += 1;
            pizzaOrder.Add("sauce");
        }
    }


    public void ShowCheese()
    {
        if (!hasCheese)
        {
            cheeseObj.SetActive(true);
            hasCheese = true;
            SpriteRenderer sr = cheeseObj.GetComponent<SpriteRenderer>();
            sr.sortingOrder = currentSauceCheeseLayer;
            currentSauceCheeseLayer += 1;
            pizzaOrder.Add("cheese");
        }
    }


    public void CutPizza()
    {
        if (!isPizzaCut && currentState == "cooked" && isOnTable)
        {
            isPizzaCut = true;
            pizzaCutObj.SetActive(true);
        }
    }


    public void PlaceTopping(string currentToppingSelected)
    {
        AddToppingToDict(currentToppingSelected);
        Vector3 center = transform.position;
        Vector3 pos = RandomToppingSpawn() + center;
        Vector3 random_rot = new Vector3(0f, 0f, UnityEngine.Random.Range(1, 5) * 90);
        Quaternion rot = Quaternion.Euler(random_rot);
        GameObject topping = Instantiate(toppingObj, pos, rot, toppingContainer.transform);
        Topping toppingScript = topping.GetComponent<Topping>();
        toppingScript.toppingBaseName = currentToppingSelected;
    }

    private void AddToppingToDict(string theTopping)
    {
        // check if topping exist in dictionaty
        // if exist, get value and add 1.
        if (toppings.ContainsKey(theTopping))
        {
            int toppingValue = toppings[theTopping];
            toppingValue += 1;
            toppings[theTopping] = toppingValue;
        }
        else
        {
            // if does NOT exist, add the topping with value of 1.
            toppings.Add(theTopping, 1);
        }
    }

    private Vector3 RandomToppingSpawn()
    {
        Vector3 pos;
        bool checkPoint = false;
        float xx = 0f;
        float yy = 0f;

        while (!checkPoint)
        {
            xx = UnityEngine.Random.Range(minToppingSpawn.x, maxToppingSpawn.x);
            yy = UnityEngine.Random.Range(minToppingSpawn.y, maxToppingSpawn.y);
            checkPoint = IsPointInPolygon(new Vector2(xx, yy), toppingPolygon);
        }
        pos = new Vector3(xx, yy, 0f);

        return pos;
    }

    private bool IsPointInPolygon(Vector2 point, Vector2[] polygon)
    {
        int polygonLength = polygon.Length, i = 0;
        bool inside = false;
        // x, y for tested point.
        float pointX = point.x, pointY = point.y;
        // start / end point for the current polygon segment.
        float startX, startY, endX, endY;
        Vector2 endPoint = polygon[polygonLength - 1];
        endX = endPoint.x;
        endY = endPoint.y;
        while (i < polygonLength)
        {
            startX = endX; startY = endY;
            endPoint = polygon[i++];
            endX = endPoint.x; endY = endPoint.y;
            inside ^= (endY > pointY ^ startY > pointY) &&
                ((pointX - endX) < (pointY - endY) * (startX - endX) / (startY - endY));
        }
        return inside;
    }

    public void ChangeState(string newState)
    {
        // update current state
        currentState = newState;

        // change dough state
        Sprite sprite = GetSpriteByName("dough", isSelected);
        spriteRenderer.sprite = sprite;

        // (de)activate pizza selected sprite
        pizzaSelected.SetActive(isSelected);

        // go through all children in base and change them to cooked/burnt
        foreach (Transform child in baseContainer.transform)
        {
            SpriteRenderer sr = child.GetComponent<SpriteRenderer>();
            sprite = GetSpriteByName(child.name, false);
            sr.sprite = sprite;
        }

        // go through all toppings children and change state
        foreach (Transform child in toppingContainer.transform)
        {
            Topping t = child.GetComponent<Topping>();
            t.SetNewState(newState);
        }
    }

    public void EnableClickArea(bool boolValue)
    {
        GetComponent<CircleCollider2D>().enabled = boolValue;
    }

    public bool GetClickArea()
    {
        bool returnValue = GetComponent<CircleCollider2D>().enabled;
        return returnValue;
    }

    public void SetClickable(bool boolValue)
    {
        isClickable = boolValue;
    }

    public ArrayList CheckPizzaOrder()
    {
        // pizzaOrder, when CheckPizzaOrder is called, ex. ["sauce","cheese"]
        // append toppings, return
        pizzaOrder.Add(toppings);
        return pizzaOrder;
    }
}
