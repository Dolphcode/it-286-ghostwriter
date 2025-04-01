using TMPro;
using UnityEngine;

public class EMFBehavior : ItemBehavior
{

    [SerializeField]
    TextMeshProUGUI m_TextMeshProUGUI;

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
            Debug.Log("We are on");
            Debug.Log(levelManager.IsGhostInRoom(levelManager.GetRoomFromPosition(transform.position)));
            if (levelManager.IsGhostInRoom(levelManager.GetRoomFromPosition(transform.position)))
            {
                Debug.Log(ghosty.GetEmf());
                m_TextMeshProUGUI.text = ghosty.GetEmf().ToString();

            } else
            {
                m_TextMeshProUGUI.text = "0";
            }
        } else
        {
            m_TextMeshProUGUI.text = "";
        }
        //Debug.Log(levelManager.GetRoomFromPosition(transform.position).name);
    }
}
