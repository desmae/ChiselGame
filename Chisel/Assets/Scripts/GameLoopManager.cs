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
     * Last Changed by: Evan Robertson
     * Last Date Changed: 2025-03-21
     * 
     *   -> 1.0 - Created GameLoopManager.cs and implemented game states with proper transitions and some placeholders.
     *   -> 1.1 - Added check for duplicates, if a duplicate level is loaded, it will reroll until the level is different.
     *   -> 1.2 - Added stat tracking for save file
     *   v1.1
     */

[System.Serializable]
public class LevelOption
{
    public string levelName;      // The name of the layout (matches prefab and sprite name)
    public Sprite levelImage;     // The preview image for this layout
    public GameObject levelPrefab; // Reference to the actual level layout prefab
    public int difficulty;        // You could extract or assign difficulty here
    public string difficultyName; // "Easy", "Medium", "Hard", "Very Hard".
}

public class GameLoopManager : MonoBehaviour
{
    // Choose Level
    public LevelSelectionPanel levelSelectionPanel;
    private LevelOption selectedOption;
    public GemPlacementManager gemPlacementManager;
    public RewardPanel rewardPanel;
    public CorruptedSelectionPanel corruptedSelectionPanel;

    public enum GameStage
    {
        ChooseLevel,
        LoadLevel,
        Play,
        Reward,
        LoadSpecialLevel,   
        PlaySpecialLevel,   
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
                case GameStage.LoadSpecialLevel: 
                    yield return StartCoroutine(LoadSpecialLevel());
                    break;
                case GameStage.PlaySpecialLevel:
                    yield return StartCoroutine(PlaySpecialLevel());
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
                    yield break;
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

        List<GameObject> usedPrefabs = new List<GameObject>();


        for (int i = 0; i < 3; i++)
        {
            GameObject chosenPrefab = null;
            Sprite previewSprite = null;
            bool foundUnique = false;

            int difficultyType = -1;
            string difficultyName = "";

            for (int attempts = 0; attempts < 50; attempts++)
            {
                float rand = Random.Range(0f, totalWeight);
                if (rand < weightEasy)
                {
                    difficultyType = 1;
                    difficultyName = "Easy";
                    chosenPrefab = easyPrefabs[Random.Range(0, easyPrefabs.Length)];
                }
                else if (rand < weightEasy + weightMedium)
                {
                    difficultyType = 2;
                    difficultyName = "Medium";
                    chosenPrefab = mediumPrefabs[Random.Range(0, mediumPrefabs.Length)];
                }
                else if (rand < weightEasy + weightMedium + weightHard)
                {
                    difficultyType = 3;
                    difficultyName = "Hard";
                    chosenPrefab = hardPrefabs[Random.Range(0, hardPrefabs.Length)];
                }
                else
                {
                    difficultyType = 4;
                    difficultyName = "Very Hard";
                    chosenPrefab = veryHardPrefabs[Random.Range(0, veryHardPrefabs.Length)];
                }

                if (!usedPrefabs.Contains(chosenPrefab))
                {
                    usedPrefabs.Add(chosenPrefab);
                    foundUnique = true;
                    break;
                }
            }

            // if we couldn't find a unique after 50 tries, allow duplicates or fallback
            if (!foundUnique)
            {
                Debug.LogWarning("Couldn't find a unique prefab after 50 tries, allowing duplicates...");
                difficultyType = 1;
                difficultyName = "Easy";
                chosenPrefab = easyPrefabs[0]; // fallback
            }

            SpriteRenderer sr = chosenPrefab.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                previewSprite = sr.sprite;
            }

            options[i] = new LevelOption
            {
                levelName = chosenPrefab.name,
                levelPrefab = chosenPrefab,
                difficulty = difficultyType,
                difficultyName = difficultyName,  
                levelImage = previewSprite
            };
        }

        return options;
    }

    private GameObject currentLevelInstance;
    private IEnumerator LoadLevel()
    {
        Debug.Log("Loading chosen level.");
        levelSelectionPanel.gameObject.SetActive(false);

        

        // reset score for round in case.
        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.ResetStageScore();
        }


        if (selectedOption != null && selectedOption.levelPrefab != null)
        {

            currentLevelInstance = Instantiate(selectedOption.levelPrefab);

            yield return null;

            int levelNumber = (currentStageCount - 1) / 3 + 1;
            int stageNumber = (currentStageCount - 1) % 3 + 1;
            // if level 1, reset globalDifficulty to 1.
            if (levelNumber == 1 && stageNumber == 1)
            {
                GemPlacementManager manager = FindObjectOfType<GemPlacementManager>();
                if (manager != null)
                {
                    manager.globalDifficulty = 1f;
                }
            }
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

        GameStateControl gsc = FindObjectOfType<GameStateControl>();
        if (gsc == null)
        {
            Debug.LogError("No GameStateControl found in the scene.");
            yield break;
        }

        while (!gsc.WinCanvas.activeSelf && !gsc.GameOverCanvas.activeSelf)
        {
            yield return null;
        }
        if (gsc.GameOverCanvas.activeSelf)
        {
            currentStage = GameStage.GameOver;
        }
        if (gsc.WinCanvas.activeSelf)
        {
            yield return new WaitForSeconds(2);

            gsc.WinCanvas.SetActive(false);

            SaveDataManager.Instance.levelsCleared++;

            GemPlacementManager manager = FindObjectOfType<GemPlacementManager>();
            if (manager != null)
            {

                manager.IncreaseGlobalDifficulty(selectedOption.difficulty);
            }
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

        // destroy currentLevelInstance here so that levels are destroyed as soon as the reward screen is up
        if (currentLevelInstance != null)
        {
            Destroy(currentLevelInstance);
        }

        yield return new WaitUntil(() => hasSelectedReward);

        hasSelectedReward = false;
        currentStageCount++;
        if (currentStageCount == 15)
        {
            currentStage = GameStage.FinalRound;
        }
        else if (currentStageCount % 3 == 0)
        {
            currentStage = GameStage.LoadSpecialLevel;
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
    private ISpecialGoal GetRandomSpecialGoal(int levelNumber)
    {
        List<ISpecialGoal> possibleGoals = new List<ISpecialGoal>();

        possibleGoals.Add(new ScoreCapGoal(levelNumber));
        possibleGoals.Add(new ComboMultCapGoal(levelNumber));

        int randomIndex = Random.Range(0, possibleGoals.Count);
        return possibleGoals[randomIndex];
    }


    private IEnumerator LoadSpecialLevel()
    {
        Debug.Log("Loading special level...");

        GameObject[] hardPrefabs = Resources.LoadAll<GameObject>("Prefabs/GemLayouts/Hard");
        GameObject[] veryHardPrefabs = Resources.LoadAll<GameObject>("Prefabs/GemLayouts/Very Hard");
        bool pickHard = (Random.value < 0.5f);
        GameObject chosenPrefab = pickHard
            ? hardPrefabs[Random.Range(0, hardPrefabs.Length)]
            : veryHardPrefabs[Random.Range(0, veryHardPrefabs.Length)];


        currentLevelInstance = Instantiate(chosenPrefab);

        ScoreManager scoreManager = FindObjectOfType<ScoreManager>();
        if (scoreManager != null)
        {
            scoreManager.ResetStageScore();
        }
        int levelNumber = (currentStageCount - 1) / 3 + 1;
        int stageNumber = (currentStageCount - 1) % 3 + 1;
        GameStateControl gsc = FindObjectOfType<GameStateControl>();
        if (gsc != null)
        {
            gsc.SetLastLevelInfo(levelNumber, stageNumber);
            gsc.InitializeStage();

            if (gsc.WinCanvas.activeSelf)
            {
                gsc.WinCanvas.SetActive(false);
            }
        }
        yield return null; // let them spawn

        currentStage = GameStage.PlaySpecialLevel;
    }

    private IEnumerator PlaySpecialLevel()
    {
        Debug.Log("Playing special level now...");

        int levelNumber = (currentStageCount - 1) / 3 + 1;
        ISpecialGoal chosenGoal = GetRandomSpecialGoal(levelNumber);
        chosenGoal.InitializeGoal();

        GameStateControl gsc = FindObjectOfType<GameStateControl>();
        if (gsc != null)
        {
            gsc.SetCustomTaskText(chosenGoal.GetGoalDescription());
        }

        while (!chosenGoal.IsGoalMet())
        {
            yield return null;
        }

        chosenGoal.OnGoalComplete();

        while (!gsc.WinCanvas.activeSelf && !gsc.GameOverCanvas.activeSelf)
        {
            yield return null;
        }

        if (gsc.GameOverCanvas.activeSelf)
        {
            currentStage = GameStage.GameOver;
        }
        else if (gsc.WinCanvas.activeSelf)
        {
            yield return new WaitForSeconds(2);
            gsc.WinCanvas.SetActive(false);

            GemPlacementManager manager = FindObjectOfType<GemPlacementManager>();
            if (manager != null && selectedOption != null)
            {
                manager.IncreaseGlobalDifficulty(selectedOption.difficulty);
            }

            currentStage = GameStage.SpecialReward;
        }
    }
    private bool hasChosenCorruptedGem = false;
    private CorruptedGem chosenCorruptedGem = null;
    private IEnumerator SpecialReward()
    {
        Debug.Log("Giving special reward...");

        CorruptedGem[] randomGems = GenerateRandomCorruptedGems(3);

        hasChosenCorruptedGem = false;
        chosenCorruptedGem = null;

        corruptedSelectionPanel.SetupCorruptedGems(randomGems, (gem) =>
        {
            chosenCorruptedGem = gem;
            hasChosenCorruptedGem = true;
        });

        if (currentLevelInstance != null)
        {
            Destroy(currentLevelInstance);
        }

        yield return new WaitUntil(() => hasChosenCorruptedGem);

        if (chosenCorruptedGem != null)
        {
            PowerUpManager.ApplyCorruptedGem(chosenCorruptedGem);
        }
        currentStageCount++;
        currentStage = GameStage.ChooseLevel;
    }
    private CorruptedGem[] GenerateRandomCorruptedGems(int count)
    {
        List<CorruptedGem> allGems = new List<CorruptedGem>
    {
        new RemoveRedsGem(),
        new ComboCatalystGem(),
        new ScorePlusMovesMinusGem(),
        // etc. add more classes
    };

        CorruptedGem[] results = new CorruptedGem[count];
        for (int i = 0; i < count; i++)
        {
            int randomIndex = Random.Range(0, allGems.Count);
            results[i] = allGems[randomIndex];
            allGems.RemoveAt(randomIndex); 
        }
        return results;
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

        // Save game stats
        SaveDataManager.Instance.SaveGame();
    }
    private void LoadTitleScreen()
    {
        SceneManager.LoadScene(0); // placeholder, might change to a string "TitleScreen" or "MainMenu"
    }
}
