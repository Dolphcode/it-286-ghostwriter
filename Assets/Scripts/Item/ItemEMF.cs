using UnityEngine;

public class EMFBehavior : ItemBehavior
{
    
    Ghost ghosty;
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

        Debug.Log("Interacting with EMF");
    }

    public override void Load(ItemData itemData)
    {
        itemData.Behavior = this;
        data = itemData;
    }

    public override void Unload()
    {
        Debug.Log("Unloading EMF");
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       levelManager = FindAnyObjectByType<LevelManager>();
       ghosty = FindAnyObjectByType<Ghost>();

    }

    // Update is called once per frame
    void Update()
    {
        //Finds if player is in same room as ghost
        if (data.isOn)
        {

            if (levelManager.IsGhostInRoom(levelManager.GetRoomFromPosition(transform.position)))
            {
                //For now, returns true if player is in room 3
                // Debug.Log("Beep Beep");


                Debug.Log(ghosty.GetEMF());

            }
        }
        //Debug.Log(levelManager.GetRoomFromPosition(transform.position).name);
    }
}
