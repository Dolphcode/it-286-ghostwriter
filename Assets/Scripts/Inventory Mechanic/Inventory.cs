using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using JetBrains.Annotations;

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

    public GameObject inventorySlotPrefab;
    public List<InventorySlot> InventorySlots => inventorySlots;    //The inventory
    public int InventorySize => InventorySlots.Count;               //Amount of inventory slots

    public UnityAction<InventorySlot> OnInventorySlotChanged;       //Signal

    /// <summary>
    /// Used for initializing the inventory when player spawns.
    /// </summary>
    /// <param name="size">The amount of slots the inventory will have.</param>
    public Inventory(int size)
    {
        CreateInventory(size);
    }

    /// <summary>
    /// Used to create the inventory for the GameObject.
    /// </summary>
    /// <param name="size">The amount of slots the inventory will have.</param>
    public void CreateInventory(int size)
    {
        inventorySlots = new List<InventorySlot>(size);

        for (int i = 0; i < size; i++)
        {
            inventorySlots.Add(Instantiate(inventorySlotPrefab).GetComponent<InventorySlot>());
        }
    }

    /// <summary>
    /// Used for getting the next empty slot
    /// </summary>
    /// <returns>-1 if there are no empty slots, or the next empty slot available</returns>
    public int CheckSlots()
    {
        for (int i = 0; i < inventorySlots.Count;i++)
        {
            if (inventorySlots[i] != null)
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// Used for placing the item into inventory.
    /// Checks if there is a inventory slot aviable first and then adds the item data to the slot.
    /// </summary>
    /// <param name="itemData">The data the item uses.</param>
    /// <returns>Debug log when there is no more aviable inventory slots, or adds new item in inventory slot otherwise.</returns>
    public void PickUp(ItemBehavior itemData)
    {
        int slot = CheckSlots();

        if (slot == -1)
        {
            Debug.Log("No more avaiables slots");
            return;
        }
        else
        {
            inventorySlots[slot].AddItem(itemData.data);
        }
    }

    /// <summary>
    /// Finds the slot that is being equipped right now.
    /// Used to see what item to hold in hands.
    /// </summary>
    /// <returns>-1 if there is no slots being equipped, or the slot being equipped otherwise.</returns>
    public int CurrentHoldOut()
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (inventorySlots[i].holdOut)
            {
                return i;
            }
        }
        return -1;
    }

    /// <summary>
    /// Change the current item being held to the next selected slot.
    /// </summary>
    /// <param name="newItemHold"></param>
    public void ChangeHeldItem(int newItemHold, Transform itemHolder)
    {
        int current = CurrentHoldOut();
        inventorySlots[current].holdOut = false;
        inventorySlots[newItemHold].holdOut = true;

        // Destroy the item instance
        inventorySlots[current].ItemData.Behavior.Unload();
        inventorySlots[current].ItemData.Behavior = null;
        Destroy(inventorySlots[current].ItemData.Behavior);

        // Load the prefab for the new item being held out
        if (inventorySlots[newItemHold].ItemData != null)
        {
            ItemBehavior item = Instantiate(inventorySlots[newItemHold].ItemData.Item).GetComponent<ItemBehavior>();
            inventorySlots[newItemHold].ItemData.Behavior = item;
            item.Load(inventorySlots[newItemHold].ItemData);
            item.PutInHand(itemHolder);
        }
    }

    /// <summary>
    /// Add item to inventory if there is an available slot and equip it if the slot is the active slot. If not, don't pick the item up.
    /// </summary>
    /// <param name="item">The item's behavior</param>
    /// <param name="itemHolder">The location to carry item</param>
    /// <returns>Don't pick it up if there is no space in inventory, or pick up and either store item in inventory or hold it out.</returns>
    public void PickUpItem(ItemBehavior item, Transform itemHolder)
    {
        int emptySlot = CheckSlots();

        if (emptySlot == -1)
        {
            return;
        }

        inventorySlots[emptySlot].AddItem(item.data);

        if (inventorySlots[emptySlot].holdOut)
        {
            item.PutInHand(itemHolder);
        } else
        {
            inventorySlots[emptySlot].ItemData.Behavior.Unload();
            inventorySlots[emptySlot].ItemData.Behavior = null;
            Destroy(inventorySlots[emptySlot].ItemData.Behavior);
        }
    }

    /// <summary>
    /// Delete item data from inventory when item is drop.
    /// </summary>
    public void DropItem()
    {
        int activeSlot = CheckSlots();

        inventorySlots[activeSlot].ItemData.Behavior.Drop();
        inventorySlots[activeSlot].ClearSlot();
    }
}
