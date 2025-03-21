using System.Collections;
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

    public Vector3 heldItemPosition;
    public Vector3 heldItemRotation;

    public bool isEquiped;
    public bool isOn;
    public float durability; // flashlight and the pills but not emf
}
