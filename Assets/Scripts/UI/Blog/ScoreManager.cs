using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private BlogPanel blogPanel;
    [SerializeField]
    private BlogUIManager blogUIManager;
    [SerializeField]
    private BlogSaveData blogSaveData;
    [SerializeField]
    private int captureScore, quesScore;
    [SerializeField]
    private float quesAccuracy;
    [SerializeField]
    private int totalScore;
    [SerializeField]
    private int moneyChange;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        quesAccuracy = quesScore / 2f;
        captureScore = blogSaveData.GetImageScore();
        totalScore = (quesScore*5) + captureScore;
        moneyChange = blogUIManager.GetMoneyChange();
        blogUIManager.AdjustMoney(totalScore);
        if (moneyChange == 0)
            scoreText.text = "You have officially published:" + blogPanel.GetDisplayHeader() + "!\nYour image score is " + captureScore + ".\n Your blog accuracy score is " + quesAccuracy + "%.\n Your total score is " + totalScore + ". Because of the fee we take from your account each time you publish, this nets you $" + ". Thank you for contributing to the Ghost Writer forum.";
        if (moneyChange>0)
            scoreText.text = "You have officially published:" + blogPanel.GetDisplayHeader() + "!\nYour image score is " + captureScore + ".\n Your blog accuracy score is " + quesAccuracy + "%.\n Your total score is " + totalScore + ". This nets you $" + moneyChange + ". Great work! Thank you for contributing to the Ghost Writer forum!";
        else
        {
            moneyChange *= -1;
            scoreText.text = "You have officially published:" + blogPanel.GetDisplayHeader() + "!\nYour image score is " + captureScore + ".\n Your blog accuracy score is " + quesAccuracy + "%.\n Your total score is " + totalScore + ". Due to the poor quality of your work, we will be deducting a fee of $" + moneyChange + " from your account. If your blogs do not increase in quality, we may exercise our right to remove you from our platform. Thank you for contributing to the Ghost Writer forum.";
        }
    }
}
