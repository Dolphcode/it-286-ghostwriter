using System.Collections.Generic;
using UnityEngine;

/*
 * @brief Used for item dictionary.
 * @note When you need to get the prefab of an item, you search for it's name and a GameObject will be given to you.
 */
public class ItemDefinition : MonoBehaviour
{
    private Dictionary<string, GameObject> _items = new Dictionary<string, GameObject>();
    private Dictionary<string, Sprite> _item_inventory = new Dictionary<string, Sprite>();



}
