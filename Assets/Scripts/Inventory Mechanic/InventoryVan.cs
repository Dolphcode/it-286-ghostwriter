using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Events;

/*
 * @brief This script is so that the game object can interact with the inventory
 * 
 * HOW TO USE:
 * Attach to the player/storage and put in the amount of slots the inventory should have
 */
[System.Serializable]
public class InventoryVan : MonoBehaviour
{
    [SerializeField] private int inventorySize;
    [SerializeField] protected Inventory inventorySystem;
    [SerializeField] private GameObject inventorySlotPrefab;


    public RaycastHit lookingAt;
    public Transform itemContainer;
    public Transform cameraHolder;
    public Inventory InventorySystem => inventorySystem;

    public static UnityAction<Inventory> InventoryDisplayRequest;

    // On spawn, add a new inventory system to the object
    private void Awake()
    {
        inventorySystem = gameObject.AddComponent<Inventory>();
        inventorySystem.inventorySlotPrefab = inventorySlotPrefab;
        inventorySystem.CreateInventory(inventorySize);
        inventorySystem.InventorySlots[0].holdOut = true;
    }
    public ItemBehavior GetHeldItem()
    {
        int heldSlot = inventorySystem.CurrentHoldOut();
        ItemData data = inventorySystem.InventorySlots[heldSlot].ItemData;
        
        if (data == null )
        {
            return null;
        }

        return data.Behavior;
    }

    public void Update()
    {
        
    }
}
