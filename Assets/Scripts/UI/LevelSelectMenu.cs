using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectMenu : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField]
    private Button levelLoadButton;
    [SerializeField]
    private Button levelActivateButton;

    [SerializeField]
    private List<TextMeshProUGUI> itemCounters; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UpdateInventoryDisplay();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdateInventoryDisplay()
    {
        for (int i = 0; i < itemCounters.Count; i++)
        {
            itemCounters[i].text = LevelDataManager._Instance.GetItemCount(i).ToString();
        }
    }

    /// <summary>
    /// Essentially a wrapper function for interfacing with the level data
    /// manager
    /// </summary>
    /// <param name="index">Which item to add to</param>
    public void BringItem(int index)
    {
        LevelDataManager._Instance.BringItem(index);
        UpdateInventoryDisplay();
    }

    /// <summary>
    /// Also a wrapper function for interfacing with the level data manager
    /// </summary>
    /// <param name="index">Which item to leave behind</param>
    public void LeaveItem(int index)
    {
        LevelDataManager._Instance.LeaveItem(index);
        UpdateInventoryDisplay();
    }

    public void StartLevelLoad(int index)
    {
        LevelLoader._Instance.LoadLevel(index);
    }

    public void ActivateLoadedLevel()
    {
        //LevelLoader._Instance.ActivateLevel();
    }
}
