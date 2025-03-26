using UnityEngine;

public class CrucifixBehavior : ItemBehavior
{
    Ghost ghost;
    void Start()
    {
        ghost = FindAnyObjectByType<Ghost>();
    }

    public override void Interact()
    {
        /*if (ghost.IsGhostHunting())
        //{ 
            ghost.GhostHuntOff();
        }
        else
        {
           new WaitForSeconds(3f);
        }
        */
        new WaitForSeconds(7f);
        ghost.IncreaseAggression(1, 2, 10);
        Debug.Log("Ghost Agression Increased");
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
