using System.Collections;
using UnityEngine;

public class CrucifixBehavior : ItemBehavior
{
    Ghost ghost;
    private bool isCounting;
    private float currentTime;
    void Start()
    {
        ghost = FindAnyObjectByType<Ghost>();
    }

    public override void Interact()
    {
        StartCoroutine(Compel(10));
    }
    public override void Load(ItemData itemData)
    {
        itemData.Behavior = this;
        data = itemData;
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
            ghost.GhostHuntOff();
        }
        else
        {
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
