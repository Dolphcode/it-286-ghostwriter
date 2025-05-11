using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

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
    [SerializeField] private GameObject inventorySlotPrefab;

    [Header("UI")]
    [SerializeField] private GameObject uiPrefab;
    [SerializeField] private Transform inventoryUI;
    [SerializeField] private List<Image> icons = new List<Image>();
    [SerializeField] private List<TextMeshProUGUI> invNums = new List<TextMeshProUGUI>();  

    public RaycastHit lookingAt;
    public Transform itemContainer;
    public Transform cameraHolder;
    public Camera camera;
    public Inventory InventorySystem => inventorySystem;

    public static UnityAction<Inventory> InventoryDisplayRequest;

    // On spawn, add a new inventory system to the object
    private void Awake()
    {
        inventorySystem = gameObject.AddComponent<Inventory>();
        inventorySystem.inventorySlotPrefab = inventorySlotPrefab;
        Debug.Log("creating the inventory");
        inventorySystem.CreateInventory(inventorySize);
        inventorySystem.InventorySlots[0].holdOut = true;

        
    }

    private void Start()
    {
        for (int i = 0; i < inventorySize; i++)
        {
            Debug.Log(i);
            GameObject slot = Instantiate(uiPrefab);
            TextMeshProUGUI label = slot.GetComponentInChildren<TextMeshProUGUI>();
            label.text = (i + 1).ToString();
            Image image = slot.GetComponentsInChildren<Image>()[1];
            icons.Add(image);
            invNums.Add(label);
            slot.transform.SetParent(inventoryUI);
        }

        InputSystem.actions.FindAction("Drop").started += InteractDrop;
    }

    public ItemBehavior GetHeldItem()
    {
        int heldSlot = inventorySystem.CurrentHoldOut();
        if (heldSlot < 0) return null;
        ItemData data = inventorySystem.InventorySlots[heldSlot].ItemData;
        
        if (data == null )
        {
            return null;
        }

        return data.Behavior;
    }

    public void Update()
    {
        

        // Key 1
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            inventorySystem.ChangeHeldItem(0,itemContainer);
        }
        // Key 2
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            inventorySystem.ChangeHeldItem(1, itemContainer);
        }
        // Key 3
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            inventorySystem.ChangeHeldItem(2, itemContainer);
        }
        // Key 4
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            inventorySystem.ChangeHeldItem(3, itemContainer);
        }
        // Key 5
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            inventorySystem.ChangeHeldItem(4, itemContainer);
        }

        for (int i = 0; i < inventorySize; ++i)
        {
            ItemData slot = inventorySystem.InventorySlots[i].ItemData;
            if (slot == null)
            {
                icons[i].color = Color.clear;
                icons[i].sprite = null;
            } else
            {
                icons[i].color = Color.white;
                icons[i].sprite = slot.Icon;
            }
            
            if (i == inventorySystem.CurrentActiveSlot())
            {
                invNums[i].color = Color.yellow;
            } else
            {
                invNums[i].color = Color.white;
            }
        }

    }



    private void InteractDrop(InputAction.CallbackContext action)
    {
        inventorySystem.DropItem();
    }
}
