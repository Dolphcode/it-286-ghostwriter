using UnityEngine;
using TMPro;

public class ThermoBehavior : ItemBehavior
{
    Ghost ghost;
    float roomTemp;
    float aggressionPercent;
    public TMP_Text text;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (FindAnyObjectByType<Ghost>() != null)
        {
            ghost = FindAnyObjectByType<Ghost>();
        }
        roomTemp = 20;
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
    }

    public override void Load(ItemData itemData)
    {
        itemData.Behavior = this;
        data = itemData;
    }
    public override void Unload()
    {

    }

    void Update()
    {
        aggressionPercent = ghost.GetAggression() / ghost.GetAggressionThreshold();
        if (!data.isOn)
        {
            text.text = "off";
        }
        else
        {
            FindRoomTemp();
            
        }
    }
    /// <summary>
    /// Finds the temperature of the room that the thermometer is in.
    /// As the ghost aggression goes up, the temperature goes down towards 0 degrees Celsius (Only if the ghost type is not Bio.)
    /// If the ghost type is bio. the thermometer stops at 5 degrees Celsius.
    /// If the ghost is not in the room, the temperature slowly rises until 20 degrees Celsius.
    /// Temperature does not change when the thermometer is off.
    /// </summary>
    void FindRoomTemp()
    {

        if (levelManager.IsGhostInRoom(transform.position))
        {
            if (ghost.GetGhostType() == "Psychological" | ghost.GetGhostType() == "Metaphysical")
            {
                if (roomTemp > aggressionPercent * 20)
                {
                    roomTemp -= 0.5f * Time.deltaTime;
                }
                else
                {
                    roomTemp = (int)roomTemp;
                }
            }

            else
            {
                if (roomTemp > 5)
                {
                    roomTemp -= 0.5f * Time.deltaTime;
                }
                else
                {
                    roomTemp = 5;
                }
            }

        }
        else
        {
            if (roomTemp < 20)
            {
                roomTemp += 0.5f * Time.deltaTime;
            }
            else
            {
                roomTemp = 20;
            }
        }

        text.text = ((int) roomTemp).ToString();
    }
}
