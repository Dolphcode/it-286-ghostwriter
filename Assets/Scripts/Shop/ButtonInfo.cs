using UnityEngine;
using TMPro;
public class ButtonInfo : MonoBehaviour
{
    public int itemID;
    public TMP_Text priceTxt;
    public TMP_Text quantityTxt;
    ShopManager shopMan;

    private void Awake()
    {
        shopMan = FindAnyObjectByType<ShopManager>();
    }
    // Update is called once per frame
    void Update()
    {
        priceTxt.text = "$" + shopMan.shopItems[2,itemID].ToString();
        quantityTxt.text = shopMan.shopItems[3, itemID].ToString();
    }
}
