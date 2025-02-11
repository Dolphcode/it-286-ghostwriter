using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[System.Serializable]
public class Inventory : MonoBehaviour
{
    // TODO:
    /*
     * Make inventory object a do not destory on load. -> So that we save item information.
     * 
     * Add item method
     * Loss item method
     * 
     * List of obj names & coresponding prefabs -> Dictionary for getting and loading a object
     * Dict: Name(string): quantity(int) -> Initialization stat for getting quantity of items in world
     * Dict: Name(string): player object(GameObject) -> the objects the player has in inventory for entering and ex
     */
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    /*
     * @brief The inventory of the player. The inventory is saved thoughout every scene.
     */
    [SerializeField] private List<InventorySlot> inventorySlots;

    public List<InventorySlot> InventorySlots => inventorySlots;    //The inventory
    public int InventorySize => InventorySlots.Count;               //Amount of inventory slots

    public UnityAction<InventorySlot> OnInventorySlotChanged;       //Signal

    public Inventory(int size)
    {
        inventorySlots = new List<InventorySlot>(size);

        for (int i = 0; i < size; i++)
        {
            inventorySlots.Add(new InventorySlot());
        }
    }
}
