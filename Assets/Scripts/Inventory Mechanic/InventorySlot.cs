using UnityEngine;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class InventorySlot : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
    
    public ItemData ItemData => itemData;

    public InventorySlot(ItemData source)
    {
        itemData = source;
    }

    public InventorySlot()
    {
        ClearSlot();
    }

    public void ClearSlot()
    {
        itemData = null;
    }
}
