using UnityEngine;

public class ButtonAction : MonoBehaviour
{
    public enum ButtonType { ComputerButton, ShopItem, WorldButton }
    
    public ButtonType buttonType;

    // The UI that will be displayed
    public GameObject targetUI;

    // All UI objects that will be turned off
    public GameObject uiOff1;
    public GameObject uiOff2;
    public GameObject uiOff3;

    // For shop UI
    public ItemData itemShop;

    // When the button is interacted with, turn on the targeted ui and turn off all other ui screens
    public void Button_clicked()
    {
        // Computer Button
        if (buttonType == ButtonType.ComputerButton)
        {
            // Show the UI of the button
            targetUI.SetActive(true);

            // Hide all other UI screens
            uiOff1.SetActive(false); uiOff2.SetActive(false); uiOff3.SetActive(false);
        }
        // Shop Item Button
        else if (buttonType == ButtonType.ShopItem)
        {

        }
    }
}
