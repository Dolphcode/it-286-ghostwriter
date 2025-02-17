using UnityEngine;

public class EMF : MonoBehaviour
{
    LevelManager levelManager;
    Ghost ghosty;


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
        if (levelManager.IsGhostInRoom(levelManager.GetRoomFromPosition(transform.position)))
            {
            //For now, returns true if player is in room 3
            Debug.Log("Beep Beep");

            //Not implemented bc ada made it private tsk tsk tsk

            //Debug.Log(ghosty.getEMF());

            }
        Debug.Log(levelManager.GetRoomFromPosition(transform.position).name);
    }
}
