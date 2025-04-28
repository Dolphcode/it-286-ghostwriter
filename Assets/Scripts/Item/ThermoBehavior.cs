using UnityEngine;

public class ThermoBehavior : ItemBehavior
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    public override void Interact()
    {
        //Directly proportional to ghost aggression,
        //temp goes down as aggression goes up, when temp reaches its lowest temp:
        //ghost starts hunting

        return;
    }

    public override void Load(ItemData itemData)
    {
        itemData.Behavior = this;
        data = itemData;
    }
    public override void Unload()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
