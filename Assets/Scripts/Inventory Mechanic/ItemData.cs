using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Inventory Item for Inventory system")]
public class ItemData : ScriptableObject
{
    public int ID;
    public string Name;
    public string Description;
    public Sprite Icon;
    [SerializeField]
    public GameObject Item;
    public ItemBehavior Behavior;

    public Vector3 heldItemPosition;
    public Vector3 heldItemRotation;

    public bool inInventory = false;
    public bool isEquiped;
    public bool isOn;
    public int durability; // flashlight and the pills but not emf
    public int itemCost;

    public float[] cooldownMaxes; // Feel free to designate this arbitrarily per item
    public float[] cooldowns;
}
