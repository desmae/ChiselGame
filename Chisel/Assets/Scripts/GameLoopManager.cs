using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
     * GameLoopManager.cs
     * Created by: Nicolas Kaplan
     * Date Created: 2025-03-17
     * 
     * Description: This script is in charge of the game loop using the enum GameStage.
     * 
     * Last Changed by: Nicolas Kaplan
     * Last Date Changed: 2025-03-17
     * 
     *   -> 1.0 - Created GameLoopManager.cs and implemented game states with transitions and placeholders.
     *
     *   v1.0
     */

[System.Serializable]
public class LevelOption
{
    public string levelName;      // The name of the layout (matches prefab and sprite name)
    public Sprite levelImage;     // The preview image for this layout
    public GameObject levelPrefab; // Reference to the actual level layout prefab
    public int difficulty;        // You could extract or assign difficulty here
}

public class GameLoopManager : MonoBehaviour
{
    // Choose Level
    public LevelSelectionPanel levelSelectionPanel;
    private LevelOption selectedOption;
    public GemPlacementManager gemPlacementManager;
    public RewardPanel rewardPanel;
    public enum GameStage
    {
        ChooseLevel,
        LoadLevel,
        Play,
        Reward,
        SpecialLevel,
        SpecialReward,
        FinalRound,
        UnlockFinalRewards,
        GameOver
    }

    private GameStage currentStage = GameStage.ChooseLevel;
    public GameStage CurrentStage => currentStage;
    private int currentStageCount = 1; // keeps track of stage

    // put references to UI stuff here (power up and rewards stuff)

    private void Start()
    {
        StartCoroutine(MainGameLoop());
    }

    private IEnumerator MainGameLoop()
    {
        while (true)
        {
            switch (currentStage) 
            { 
                case GameStage.ChooseLevel:
                    yield return StartCoroutine(ChooseLevel());
                    break;
                case GameStage.LoadLevel:
                    yield return StartCoroutine(LoadLevel());
                    break;
                case GameStage.Play:
                    yield return StartCoroutine(PlayLevel());
                    break;
                case GameStage.Reward:
                    yield return StartCoroutine(RewardPlayer());
                    break;
                case GameStage.SpecialLevel:
                    yield return StartCoroutine(SpecialLevel());
                    break;
                case GameStage.SpecialReward:
                    yield return StartCoroutine(SpecialReward());
                    break;
                case GameStage.FinalRound:
                    yield return StartCoroutine(FinalRound());
                    break;
                case GameStage.UnlockFinalRewards:
                    yield return StartCoroutine(UnlockFinalRewards());
                    break;
                case GameStage.GameOver:
                    SavePlayerStats();
                    LoadTitleScreen();
                    yield break; // exit loop, game over.
            }
            yield return null;
        }
    }

    private IEnumerator ChooseLevel()
    {
        Debug.Log("Entering chooselevel");

        // generate three random level options
        LevelOption[] options = GenerateRandomLevelOptions();
        if (options.Length == 0)
        {
            Debug.LogError("No level options found.");
            yield break;
        }
        // reset the selected option in case there is one
        selectedOption = null;

        // ui with a callback to capture player's selection
        levelSelectionPanel.SetupOptions(options, option =>
        {
            selectedOption = option;
            Debug.Log($"Player selected a level: Name: {option.levelName}, Difficulty: {option.difficulty}");
        });
        yield return new WaitUntil(() => selectedOption != null);

        currentStage = GameStage.LoadLevel;
    }

    private LevelOption[] GenerateRandomLevelOptions()
    {
        GameObject[] easyPrefabs = Resources.LoadAll<GameObject>("Prefabs/GemLayouts/Easy");
        GameObject[] mediumPrefabs = Resources.LoadAll<GameObject>("Prefabs/GemLayouts/Medium");
        GameObject[] hardPrefabs = Resources.LoadAll<GameObject>("Prefabs/GemLayouts/Hard");
        GameObject[] veryHardPrefabs = Resources.LoadAll<GameObject>("Prefabs/GemLayouts/Very Hard");

        if (easyPrefabs.Length == 0 || mediumPrefabs.Length == 0 || hardPrefabs.Length == 0 || veryHardPrefabs.Length == 0)
        {
            Debug.LogError("One or more difficulty folders in Prefabs/GemLayouts do not contain any prefabs.");
            return new LevelOption[0];
        }

        // globalDifficulty is a float between 1 (easiest) and 5 (hardest).
        float globalDiff = gemPlacementManager.globalDifficulty;
        float t = (globalDiff - 1f) / 4f; // Normalize to [0,1]: 0 = easiest, 1 = hardest.

        float weightEasy = Mathf.Lerp(0.6f, 0.1f, t);      // High weight when globalDiff is low.
        float weightMedium = Mathf.Lerp(0.2f, 0.3f, t);
        float weightHard = Mathf.Lerp(0.15f, 0.4f, t);
        float weightVeryHard = Mathf.Lerp(0.05f, 0.2f, t);
        float totalWeight = weightEasy + weightMedium + weightHard + weightVeryHard;
        Debug.Log($"Weights: Easy: {weightEasy}, Medium: {weightMedium}, Hard: {weightHard}, Very Hard: {weightVeryHard}");

        LevelOption[] options = new LevelOption[3];

        for (int i = 0; i < options.Length; i++)
        {
            float rand = Random.Range(0f, totalWeight);
            int difficultyType = -1;  // 1 = Easy, 2 = Medium, 3 = Hard, 4 = Very Hard.

            if (rand < weightEasy)
            {
                difficultyType = 1;
            }
            else if (rand < weightEasy + weightMedium)
            {
                difficultyType = 2;
            }
            else if (rand < weightEasy + weightMedium + weightHard)
            {
                difficultyType = 3;
            }
            else
            {
                difficultyType = 4;
            }

            GameObject chosenPrefab = null;
            switch (difficultyType)
            {
                case 1:
                    chosenPrefab = easyPrefabs[Random.Range(0, easyPrefabs.Length)];
                    break;
                case 2:
                    chosenPrefab = mediumPrefabs[Random.Range(0, mediumPrefabs.Length)];
                    break;
                case 3:
                    chosenPrefab = hardPrefabs[Random.Range(0, hardPrefabs.Length)];
                    break;
                case 4:
                    chosenPrefab = veryHardPrefabs[Random.Range(0, veryHardPrefabs.Length)];
                    break;
            }

            Sprite previewSprite = null;
            SpriteRenderer sr = chosenPrefab.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                previewSprite = sr.sprite;
            }
            else
            {
                Debug.LogWarning($"Chosen prefab {chosenPrefab.name} does not have a SpriteRenderer component.");
            }

            options[i] = new LevelOption
            {
                levelName = chosenPrefab.name,
                levelPrefab = chosenPrefab,
                difficulty = difficultyType,
                levelImage = previewSprite
            };
        }
        return options;
    }

    private GameObject currentLevelInstance;
    private IEnumerator LoadLevel()
    {
        Debug.Log("Loading chosen level.");
        levelSelectionPanel.gameObject.SetActive(false); // replace with objects inside panel

        if (currentLevelInstance != null)
        {
            Destroy(currentLevelInstance);
            currentLevelInstance = null;
        }

        if (selectedOption != null && selectedOption.levelPrefab != null)
        {

            currentLevelInstance = Instantiate(selectedOption.levelPrefab);

            yield return null;

            int levelNumber = (currentStageCount - 1) / 3 + 1;
            int stageNumber = (currentStageCount - 1) % 3 + 1;

            GameStateControl gsc = FindObjectOfType<GameStateControl>();
            if (gsc != null)
            {
                gsc.SetLastLevelInfo(levelNumber, stageNumber);
                gsc.InitializeStage();
            }
            if (gsc.WinCanvas.activeSelf)
            {
                gsc.WinCanvas.SetActive(false);
            }

        }
        else
        {
            Debug.LogWarning("Null or option not properly selected.");
            yield break;
        }

        currentStage = GameStage.Play;
    }

    private IEnumerator PlayLevel()
    {
        Debug.Log("Playing level now.");

        // Reference to your GameStateControl (assumes it's in the scene).
        GameStateControl gsc = FindObjectOfType<GameStateControl>();
        if (gsc == null)
        {
            Debug.LogError("No GameStateControl found in the scene.");
            yield break;
        }

        // Wait until either the winCanvas or gameOverCanvas becomes active.
        while (!gsc.WinCanvas.activeSelf && !gsc.GameOverCanvas.activeSelf)
        {
            yield return null;
        }
        if (gsc.GameOverCanvas.activeSelf)
        {
            // Player lost
            currentStage = GameStage.GameOver;
        }
        if (gsc.WinCanvas.activeSelf)
        {
            yield return new WaitForSeconds(2);
            currentStage = GameStage.Reward;
        }
    }


    private bool hasSelectedReward = false;

    private IEnumerator RewardPlayer()
    {
        Debug.Log("Rewarding player!");

        PowerUp[] randomPowerUps = GenerateRandomPowerUps(3);

        rewardPanel.SetupRewardOptions(randomPowerUps, (selectedPowerUp) => {
            PowerUpManager.ApplyPowerUp(selectedPowerUp);
            hasSelectedReward = true;
        });

        yield return new WaitUntil(() => hasSelectedReward);

        currentStageCount++;
        if (currentStageCount == 15)
        {
            currentStage = GameStage.FinalRound;
        }
        else if (currentStageCount % 3 == 0)
        {
            currentStage = GameStage.SpecialLevel;
        }
        else
        {
            currentStage = GameStage.ChooseLevel;
        }
    }
    private PowerUp[] GenerateRandomPowerUps(int count)
    {
        List<PowerUp> allPowerUps = new List<PowerUp>
    {
        new ScoreMultiplierPowerUp(),
        new DiagonalGemsPowerUp(),
        new MovesMultiplierPowerUp(),
        new OneUpPowerUp(),
        new InventoryUpgradePowerUp()
    };

        PowerUp[] results = new PowerUp[count];
        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, allPowerUps.Count);
            results[i] = allPowerUps[randomIndex];

            allPowerUps.RemoveAt(randomIndex);
        }
        return results;
    }

    private IEnumerator SpecialLevel()
    {
        Debug.Log("Loading special level");
        // set up a special layout and assign specific goal.
        yield return new WaitForSeconds(1f);

        currentStage = GameStage.SpecialLevel;
    }

    private IEnumerator SpecialReward()
    {
        Debug.Log("Giving special reward...");
        // todo: show plasyer 3 corrupted gems with pros and cons each. each should have a simple description with what they do.
        yield return new WaitUntil(() => SpecialRewardChosen());

        currentStageCount++;
        currentStage = GameStage.ChooseLevel;
    }

    private bool SpecialRewardChosen()
    {
        // todo: implement proper input check
        return Input.GetMouseButtonDown(0);
    }

    private IEnumerator FinalRound()
    {
        Debug.Log("Final round: boss battle.");
        // todo: implement final round boss battle mechanics
        yield return new WaitUntil(() => FinalRoundCompleted());

        currentStage = GameStage.UnlockFinalRewards;
    }

    private bool FinalRoundCompleted()
    {
        // check boss health = 0
        return Input.GetKeyDown(KeyCode.F); // placeholder
    }

    private IEnumerator UnlockFinalRewards()
    {
        Debug.Log("Unlocking final rewards...");
        // todo: present the final rewards being unlocked and unlock endless mode
        yield return new WaitUntil(() => FinalRewardsCollected());

        // after final rewards, prompt player to either return to title screen or to try endless mode from stage 15.
        // for endless mode implement a fair scaling mechanic.

        currentStage = GameStage.GameOver;
    }

    private bool FinalRewardsCollected()
    {
        // todo: replace with proper event trigger
        return Input.GetMouseButtonDown(0);
    }

    private void SavePlayerStats()
    {
        // todo: save stats permanently, these should persist and can be reset through the settings menu. maybe implement save FILES too.
        Debug.Log("Saving player stats...");
    }
    private void LoadTitleScreen()
    {
        SceneManager.LoadScene(0); // placeholder, might change to a string "TitleScreen" or "MainMenu"
    }
}
