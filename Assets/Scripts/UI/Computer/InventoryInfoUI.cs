using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryInfoUI : MonoBehaviour
{
    [Header("Item Info Boxes")]
    [SerializeField] private TextMeshProUGUI inventoryList;
    [SerializeField] private TextMeshProUGUI packList;

    [Header("Pack List UI")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameLabel;
    [SerializeField] private TextMeshProUGUI countLabel;
    [SerializeField] private TextMeshProUGUI totalItemCountLabel;

    private LevelDataManager levelDataManager;
    private int selectedItemIndex = 1;
    public void BringSelectedItem()
    {
        levelDataManager.BringItem(selectedItemIndex);
    }

    public void LeaveSelectedItem()
    {
        levelDataManager.LeaveItem(selectedItemIndex);
    }

    public void CheckNextItem() { 
        selectedItemIndex++; 
        if (selectedItemIndex >= levelDataManager.TemplateCount) selectedItemIndex = 1; 
    }

    public void CheckPrevItem()
    {
        selectedItemIndex--;
        if (selectedItemIndex < 1) selectedItemIndex = levelDataManager.TemplateCount - 1;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelDataManager = LevelDataManager._Instance;
    }

    // Update is called once per frame
    void Update()
    {
        inventoryList.text = "";
        packList.text = "";

        int totalItems = 0;
        for (int i = 1; i < levelDataManager.TemplateCount; i++)
        {
            ItemData dat = levelDataManager.GetItemInfo(i);
            totalItems += levelDataManager.GetItemCount(i); // Keep track of total items while we're iterating anyway
            if (levelDataManager.GetItemCount(i) > 0)
            {
                packList.text += dat.Name + " x" + levelDataManager.GetItemCount(i).ToString() + "\n";
            }

            if (levelDataManager.GetOwnedCount(i) > 0)
            {
                inventoryList.text += dat.Name + " x" + levelDataManager.GetOwnedCount(i).ToString() + "\n";
            }
        }

        totalItemCountLabel.text = totalItems.ToString();
        countLabel.text = levelDataManager.GetItemCount(selectedItemIndex).ToString();
        nameLabel.text = levelDataManager.GetItemInfo(selectedItemIndex).Name.ToString();
        icon.sprite = levelDataManager.GetItemInfo(selectedItemIndex).Icon;
    }
}
