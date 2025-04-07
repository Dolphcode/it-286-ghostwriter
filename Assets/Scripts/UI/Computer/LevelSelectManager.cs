using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelSelectManager : MonoBehaviour
{

    [Header("Config")]
    [SerializeField] int maxDifficulty = 1;
    [SerializeField] int levelsToGen = 5;

    [Header("UI References")]
    [SerializeField] TextMeshProUGUI titleLabel;
    [SerializeField] TextMeshProUGUI statusLabel;


    [SerializeField] List<LevelEvaluator> templates;


    private int levelIndex = 0;
    private int selectedLevel = -1;
    private List<LevelEvaluator> generatedLevels;

    private LevelLoader levelLoader;

    private void Awake()
    {
        generatedLevels = new List<LevelEvaluator> ();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Randomize Possible Levels
        for (int i = 0; i < levelsToGen; ++i)
        {
            LevelEvaluator eval = Instantiate<LevelEvaluator>(templates[Random.Range(0, templates.Count)]);
            eval.difficulty = Random.Range(1, maxDifficulty + 1);
            eval.RandomizeValues();
            generatedLevels.Add(eval);
        }

        // Get Level Loader instance
        levelLoader = LevelLoader._Instance;
    }

    // Update is called once per frame
    void Update()
    {
        titleLabel.text = generatedLevels[levelIndex].settingName;
        if (levelIndex == selectedLevel)
        {
            statusLabel.text = "Selected";
        } else
        {
            statusLabel.text = "";
        }
    }

    public void CheckNextLevel()
    {
        levelIndex++;
        if (levelIndex >= generatedLevels.Count)
        {
            levelIndex = 0;
        }
    }

    public void CheckPrevLevel()
    {
        levelIndex--;
        if (levelIndex < 0)
        {
            levelIndex = generatedLevels.Count - 1;
        }
    }

    public void LevelButtonOnClick()
    {
        if (levelIndex != selectedLevel)
        {
            selectedLevel = levelIndex;
        } else
        {
            selectedLevel = -1;
        }
    }

    public void LoadLevel()
    {
        if (selectedLevel >= 0) levelLoader.LoadLevel(generatedLevels[selectedLevel].sceneIndex, generatedLevels[selectedLevel]);
    }
}
