using System.Collections;
using System.Collections.Generic;
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
     * Last Date Changed: 2024-10-11
     * 
     * 
     *   -> 1.0 - Created GameStateControl.cs and created a basic win condition to
                clear the screen of all blocks.
     *   
     *   v1.0
     */
    
    GameObject[] blocks;
    public List<GameObject> blockList = new List<GameObject>();
    [SerializeField] GameObject winCanvas;
    [SerializeField] bool winCanvasOnStart = false;

    void Start()
    {
        if (!winCanvasOnStart)
        {
            winCanvas.SetActive(false);
        }
        else
        {
            winCanvas.SetActive(true);

        }

        AddBlocksToList();
    }
    void DisplayWinScreen()
    {
        winCanvas.SetActive(true);
    }
    void CheckBlocksCleared()
    {
        if (blockList.Count <= 0)
        {
            // TODO animations?
            DisplayWinScreen();
        }
    }
    void Update()
    {
        CheckBlocksCleared();
    }
    void AddBlocksToList()
    {
        blocks = GameObject.FindGameObjectsWithTag("Block");
        foreach (GameObject block in blocks)
        {
            blockList.Add(block);
            Debug.Log($"Added {block.name}");
        }
    }
    public void LoadMainMenu()
    {
        // for now we can call scenes by int, later we might need to use strings
        // or another method
        SceneManager.LoadScene(0); 
    }

    public void LoadCurrentLevel()
    {
        SceneManager.LoadScene(1);
    }

    // void LoadNextLevel()
}
