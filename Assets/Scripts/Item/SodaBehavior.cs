using UnityEngine;

public class SodaBehavior : ItemBehavior
{

    public override void Interact()
    {
        
        Debug.Log("Interacting with Soda");
        
        if (data.durability > 0)
        {
            data.durability -= 1;
            GameObject.Find("Fear").GetComponent<Fear>().ChangeFearMeter(-20);
        }
        else
        {
            
            Debug.Log("No More Durability");
        }

        
    }

    public override void Load(ItemData itemData)
    {
        itemData.Behavior = this;
        data = itemData;
    }

    public override void Unload()
    {
        
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
