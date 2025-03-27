using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{

    LevelDataManager levelDataManager;
    public Text moneyTxt;

    //For the 2-D Array:

    // Row 1: Item ID
    // Row 2: Item Cost
    // Row 3: Item Quantity

    // Columns = Individual Items
    // Column 1: Handheld Camera
    // Column 2: Flashlight
    // Column 3: EMF
    // Column 4: Soda
    // Column 5: Crucifix

    public int[,] shopItems = new int[6,6];
 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelDataManager = FindAnyObjectByType<LevelDataManager>();
        moneyTxt.text = "Money: " + levelDataManager.GetMoney().ToString();

        // Item Id's
        // WHEN ADDING NEW ITEM: Make sure to add new element in Level Data Manager and add one more column to shopItems

           shopItems[1, 1] = 1;
           shopItems[1, 2] = 2;
           shopItems[1, 3] = 3;
           shopItems[1, 4] = 4;
           shopItems[1, 5] = 5;


        // Item Cost
        
          shopItems[2, 1] = 999;
          shopItems[2, 2] = 300;
          shopItems[2, 3] = 400;
          shopItems[2, 4] = 200;
          shopItems[2, 5] = 666;

        // Quantity
        //Left at 0 for now, should try to make it change to how many the player has currently.

          shopItems[3, 1] = 0;
          shopItems[3, 2] = 0;
          shopItems[3, 3] = 0;
          shopItems[3, 4] = 0;
          shopItems[3, 5] = 0;

    }


    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// Add Item to ItemCount and change all the text in the UI.
    /// Tests if money would be 0 after buying and buys if it will not be.
    /// </summary>
    public void Purchase()
    {
        GameObject ButtonRef = GameObject.FindGameObjectWithTag("Event").GetComponent<EventSystem>().currentSelectedGameObject;
        if(levelDataManager.SpendMoney(shopItems[2,ButtonRef.GetComponent<ButtonInfo>().itemID]))
        {
            levelDataManager.RemoveMoney(shopItems[2, ButtonRef.GetComponent<ButtonInfo>().itemID]);
        }

        else
        {
            return;
        }
        shopItems[3, ButtonRef.GetComponent<ButtonInfo>().itemID]++;
        levelDataManager.AddItem(ButtonRef.GetComponent<ButtonInfo>().itemID);
        moneyTxt.text = "Money: $" + levelDataManager.GetMoney().ToString();
        ButtonRef.GetComponent<ButtonInfo>().quantityTxt.text = shopItems[3, ButtonRef.GetComponent<ButtonInfo>().itemID].ToString();
        Debug.Log("Purchasing");
    }
}
