using UnityEngine;

public class FlashlightBehavior : ItemBehavior
{
    
    LightManager lightManager;
    public override void Drop()
    {
        base.Drop();
        lightManager.SetFlashlightState(false);
    }

    public override void PutInHand(Transform itemContainer)
    {
        base.PutInHand(itemContainer);
        lightManager.SetFlashlightState(data.isOn);
    }
    public override void Interact()
    {
        if (!data.isOn)
        {
            data.isOn = true;
        }

        else
        {
            data.isOn = false;
        }


        if (data.isOn)
        {
            lightManager.SetFlashlightState(true);
            Debug.Log("Flashlight On");
        }
        
        if (!data.isOn)
        {
            lightManager.SetFlashlightState(false);
            Debug.Log("Flashlight Off");
        }
    }
    
   
    public override void Load(ItemData itemData)
    {
        itemData.Behavior = this;
        data = itemData;
        Debug.Log(itemData);
        //lightManager.SetFlashlightState(itemData.isOn);
    }

    public override void Unload()
    {
        //lightManager.SetFlashlightState(false);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        lightManager = FindAnyObjectByType<LightManager>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
