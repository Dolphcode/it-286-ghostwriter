using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot 
{
    [SerializeField] private ItemData itemData;

    public ItemData ItemData => itemData;
    public bool active = false;

    /*
     * @brief create inventory slot with an item in it
     */
    public InventorySlot (ItemData source)
    {
        itemData = source;
        active = true;
    }

    /*
     * @brief create an empty inventory slot
     */
    public InventorySlot () 
    {
        itemData = null;
        active = false;
    }

    /*
    * @brief clear the slot -> Used when the item is being moved, removed or used.
    */
    public void ClearSlot()
    {
        itemData = null;
        active = false;
    }
}
