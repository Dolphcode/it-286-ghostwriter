using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "Inventory Item for Inventory system")]
public class ItemData : ScriptableObject
{
    public int ID;
    public string Name;
    public Sprite Icon;
    [SerializeField]
    public GameObject Item;
    public ItemBehavior Behavior;

    public bool isEquiped;
    public bool isOn;
    public float durability; // flashlight and the pills but not emf
}
