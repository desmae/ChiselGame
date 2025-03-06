using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GameStateControl : MonoBehaviour
{
    /*
     * GameStateControl.cs
     * Created by: Nicolas Kaplan
     * Date Created: 2024-10-11
     * 
     * Description: This script is in charge of keeping tabs on the player's move count and
     *              the amount of blocks left on screen. It will display a win / lose screen
     *              depending on whether the player's moves are 0 or no blocks are left.
     * 
     * Last Changed by: Nicolas Kaplan
     * Last Date Changed: 2025-03-06
     * 
     * 
     *   -> 1.0 - Created GameStateControl.cs and created a basic win condition to
     *           clear the screen of all blocks.
     *   -> 1.1 - Updated code clarity such as private declarations. Added code for displaying the game over screen
     *          as well as code for the moves display.
     *   -> 1.2 - Added a variable and implementations to change tasks per level (text only)
     *   -> 1.3 - Removed a noisy debug log line
     *   -> 1.4 - Added canBreak static over here so players can't interact with game when gameover is on screen and
     *   added the LoadNextLevel() functionality, and a fallback as well as changing the music to the game's song.
     *   Finally, added animations for the moves counter.
     *   
     *   -> 1.5 - Added call to regenerate gems using new gem placing algorithm
     *   -> 1.6 - Added Power-up compatibility to moves count.
     *   v1.6
     */

    private GameObject[] blocks;
    public List<GameObject> blockList = new List<GameObject>();
    [SerializeField] private GameObject winCanvas;
    [SerializeField] private bool winCanvasOnStart = false;

    [SerializeField] Animator movesAnimator;

    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private int startingMoveCount = 0;
    private static int moveCount;

    [SerializeField] private TextMeshProUGUI movesCountLight;
    [SerializeField] private TextMeshProUGUI movesCountDark;

    [SerializeField] private TextMeshProUGUI tasksTMP;
    [SerializeField] private string tasksText1;
    [SerializeField] private string tasksText2;
    [SerializeField] private string tasksText3;

    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private bool gameOverCanvasOnStart = false;

    private void Awake()
    {
        BlockScript.canBreak = true;
    }

    void Start()
    {
        AudioController.Instance.PlayMusic("GameMusic");
        if (!winCanvasOnStart)
        {
            winCanvas.SetActive(false);
        }
        else
        {
            winCanvas.SetActive(true);

        }

        if (!gameOverCanvasOnStart)
        {
            gameOverCanvas.SetActive(false);
        }
        else
        {
            gameOverCanvas.SetActive(true);

        }

        AddBlocksToList();

        SetInitialMoves();

        UpdateMovesText();

        SetTaskText();
    }

    void Update()
    {
        CheckBlocksCleared();
        CheckMovesCount();
        UpdateMovesText();

        if (gameOverCanvas.activeSelf)
        {
            BlockScript.canBreak = false;
        }
    }
    private void CheckScoreOnWin()
    {
        if (scoreManager.score >= scoreManager.minimumScoreForLevel)
        {
            DisplayGameOverScreen();
            winCanvas.SetActive(false);
        }
    }
    private void OnEnable()
    {
        ScoreManager.MoveGained += OnMoveGained;
    }

    private void OnDisable()
    {
        ScoreManager.MoveGained -= OnMoveGained;
    }

    private void OnMoveGained()
    {
        IncrementMoves(1);
    }
    // Win screen & blocks

    void AddBlocksToList()
    {
        blocks = GameObject.FindGameObjectsWithTag("Block");
        foreach (GameObject block in blocks)
        {
            blockList.Add(block);
        }

        GemPlacementManager manager = FindObjectOfType<GemPlacementManager>();
        if (manager != null)
        {
            manager.AdjustGemsBasedOnDifficulty();
        }
    }

    void DisplayWinScreen()
    {
        winCanvas.SetActive(true);
    }
    void CheckBlocksCleared()
    {
        if (blockList.Count <= 0)
        {
            // TODO animations

            // instead of displaying win screen, move towards a new stage.
            // give bonus points to players with a 1-up remaining in their inventory
            if (PowerUpManager.hasOneUp)
            {
                scoreManager.score += 20000; // 20k extra points for not wasting a 1up!
            }
            scoreManager.score += PowerUpManager.addedEndScore;
            float finalScore = (float)scoreManager.score;
            finalScore *= PowerUpManager.endScoreMultiplier;
            scoreManager.score = (int)finalScore;

            DisplayWinScreen();
        }
    }

    // Moves methods & game over

    void UpdateMovesText()
    {
        movesCountDark.text = $"{moveCount}";
        movesCountLight.text = $"{moveCount}";
        movesAnimator.SetInteger("movesCount", moveCount);
    }
    public void DecrementMoves()
    {
        moveCount--; // we're only ever decreasing the moves by 1 each time.
    }
    public void IncrementMoves(int movesToAdd)
    {
        moveCount += movesToAdd;
        movesAnimator.SetTrigger("MovesAdded");
    }
    public void SetInitialMoves()
    {
        float finalMoveCount = startingMoveCount + PowerUpManager.extraStartingMoves
        * PowerUpManager.startingMovesMultiplier;
        moveCount = (int)finalMoveCount;

        if (PowerUpManager.extraStartingMoves != 0 || PowerUpManager.startingMovesMultiplier != 0)
        {
            // show a message or animation to the player that their moves are powered up
        }
    }
    public void DisplayGameOverScreen()
    {
        gameOverCanvas.SetActive(true);
    }
    public void CheckMovesCount()
    {
        if (moveCount <= 0)
        {
            // TODO Animations?
            if (PowerUpManager.hasOneUp)
            {
                Debug.Log("Make UI for 1-up working successfully, maybe even a sound effect");
                moveCount = 20;
            }
            else
            {
                DisplayGameOverScreen();
            }
        }
    }

    private void SetTaskText()
    {
        tasksTMP.text = $"{tasksText1} \n \n {tasksText2} \n \n {tasksText3}";
    }

    // Loading methods 
    public void LoadMainMenu()
    {
        // for now we can call scenes by int, later we might need to use strings
        // or another method
        SceneManager.LoadScene(0); 
    }

    public void LoadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex + 1 < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else
        {
            LoadMainMenu();
        }
    }

}
