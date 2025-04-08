using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class EMFBehavior : ItemBehavior
{

    [SerializeField]
    TextMeshProUGUI m_TextMeshProUGUI;

    Ghost ghosty;
    public List<GameObject> emfLvls;
    [SerializeField]
    int testEMF;
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
        
        Debug.Log("Interacting with EMF " + data.isOn);
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
        if (ghosty == null)
        {
            ghosty = FindAnyObjectByType<Ghost>();
        }

        //Finds if player is in same room as ghost
        if (data.isOn)
        {
            Debug.Log(testEMF);
            if (levelManager.IsGhostInRoom(levelManager.GetRoomFromPosition(transform.position)))
            {
                for (int i = 0; i < 5; i++)
                {
                    if (i < ghosty.GetEmf())
                    {
                        emfLvls[i].GetComponent<MeshRenderer>().material.SetFloat("_Light_On_Interior", 1);
                    }
                    else if (!data.isOn)
                    {
                        emfLvls[i].GetComponent<MeshRenderer>().material.SetFloat("_Light_On_Interior", 0);
                    }
                    else
                    {
                        emfLvls[i].GetComponent<MeshRenderer>().material.SetFloat("_Light_On_Interior", 0);
                    }
                }
            }
        }
    }
}
