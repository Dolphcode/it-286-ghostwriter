using UnityEngine;

public class SodaBehavior : ItemBehavior
{

    public override void Interact()
    {
        //if (camControl.lookingAt.transform.gameObject.CompareTag("Interactable"))
       // {
        //    return;
       // }

       // else
       // {
            Debug.Log("Interacting with Soda");

            if (data.durability >= 1)
            {
                data.durability -= 1;
                GameObject.Find("Player UI").GetComponent<Fear>().ChangeFearMeter(-20);
            }
            if (data.durability == 0)
            {
                GameObject.Find("Player UI").GetComponent<Fear>().ChangeFearMeter(-20);
                BreakItem(0.001f);
                Debug.Log("No More Durability");
            }
      //  }
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
