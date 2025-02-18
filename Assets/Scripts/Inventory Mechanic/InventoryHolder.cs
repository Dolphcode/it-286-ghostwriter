using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/*
 * @brief This script is so that the game object can interact with the inventory
 * 
 * HOW TO USE:
 * Attach to the player/storage and put in the amount of slots the inventory should have
 */
[System.Serializable]
public class InventoryHolder : MonoBehaviour
{
    [SerializeField] private int inventorySize;
    [SerializeField] protected Inventory inventorySystem;

    public RaycastHit lookingAt;
    public Transform itemContainer;
    public Transform cameraHolder;
    public Inventory InventorySystem => inventorySystem;

    public static UnityAction<Inventory> InventoryDisplayRequest;

    // On spawn, add a new inventory system to the object
    private void Awake()
    {
        inventorySystem = gameObject.AddComponent<Inventory>();
        inventorySystem.CreateInventory(inventorySize);
        inventorySystem.InventorySlots[0].holdOut = true;
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Physics.Raycast(cameraHolder.position, cameraHolder.forward, out lookingAt, 5f);
            if (lookingAt.collider != null)
            {
                if (lookingAt.collider.GetComponent<ItemBehavior>() != null)
                {
                    inventorySystem.PickUpItem(lookingAt.collider.GetComponent<ItemBehavior>(),itemContainer);
                    Debug.Log("Pickiing up item");
                }
            }
        }
        //Debug.Log("looking At " + lookingAt.collider);

        if (Input.GetKeyDown(KeyCode.G))
        {
            itemContainer.GetChild(0).GetComponent<ItemBehavior>().Drop();
       

        }
    }
}
