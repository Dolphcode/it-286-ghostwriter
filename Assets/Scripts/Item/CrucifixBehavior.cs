using System.Collections;
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
        
    
        if (data.durability > 0)
        {
            data.durability -= 1;
            StartCoroutine(Compel(10));
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

    public void Update()
    {
        if (ghost == null)
        {
            ghost = FindAnyObjectByType<Ghost>();
        }
    }

    /// <summary>
    /// Waits for <paramref name="time"/> seconds to pass and then increases ghost agression.
    /// If the ghost is hunting, it turns the hunting mode off. The ghost will then gain 5 agression in 10 seconds.
    /// If the Ghost is not hunting, the ghost gains agression in 5 seconds.
    /// </summary>
    /// <param name="time"> Amount of Seconds to count for </param>
    private IEnumerator Compel(int time)
    {
        if (ghost.IsGhostHunting())
        {
            Debug.Log("Stopped Ghost From Hunting");
            ghost.GhostHuntOff();
        }
        else
        {
            Debug.Log("Ghost Was Not Hunting");
            time -= 5;
        }
        yield return new WaitForSeconds(time);
        ghost.IncreaseAggression(5);
        Debug.Log("Ghost Agression Increased");
    }
 
    public override void Unload()
    {
        
    }

}
