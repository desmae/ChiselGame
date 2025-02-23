using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
     * GemPlacementManager.cs
     * Created by: Evan Robertson
     * Date Created: 2025-02-21
     * 
     * Description: This algorithm is used in regenerating gems based on the current difficulty level. The higher the difficulty, the less frequent
     * gems of the same health spawn next to each other
     * 
     * Last Changed by: Evan Robertson
     * Last Date Changed: 2025-02-23
     * 
     * 
     *   -> 1.0 - Created GemPlacementManager.cs and added global difficulty float and collection of 
     *   
     *   -> 1.1 - Added inpector elements for adjusting the lower and upper bounds of random events.
     *      
     *   v1.1
     */
public class GemPlacementManager : MonoBehaviour
{
    public float globalDifficulty = 1f;
    public List<BlockScript> allBlocks = new List<BlockScript>();
    [SerializeField] private LayerMask blockLayer;
    
    [Header("Randomness Parameters")]
    [Tooltip("The chance of a color change on lowest difficulty")]
    [SerializeField] private float colorChangeLowerBound = 0.05f;
    [Tooltip("The chance of a color change on highest difficulty")]
    [SerializeField] private float colorChangeUpperBound = 0.9f;

    [Tooltip("The chance of a gem leveling up on lowest difficulty")]
    [SerializeField] private float levelUpLowerBound = 0.05f;
    [Tooltip("The chance of a gem leveling up on highest difficulty")]
    [SerializeField] private float levelUpUpperBound = 0.4f;


    public void AdjustGemsBasedOnDifficulty()
    {
        foreach (BlockScript block in allBlocks)
        {
            if (ShouldChangeColor(block))
            {
                print("Block color changed!");
                ChangeBlockColor(block);
            }
        }

        // Unfinished
        //foreach (BlockScript block in allBlocks)
        //{
        //    if (ShouldLevelUp(block))
        //    {
        //        LevelUpBlock(block);
        //    }
        //}
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.R))
        //{
        //    print("Refreshing...");
        //    AdjustGemsBasedOnDifficulty();
        //}
    }

    private bool ShouldChangeColor(BlockScript block)
    {
        int similarCount = 0;
        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        foreach (Vector2 direction in directions)
        {
            Vector2 start = block.transform.position;
            Vector2 end = start + direction;
            RaycastHit2D hit = Physics2D.Raycast(end, end, .5f, blockLayer);
            if (hit.collider != null)
            {
                BlockScript adjacentBlock = hit.collider.GetComponent<BlockScript>();
                if (adjacentBlock != null && adjacentBlock.blockHealth == block.blockHealth)
                {
                    similarCount++;
                }
            }
        }

        float changeChance = Mathf.Lerp(0.05f, 0.9f, (globalDifficulty - 1) / 4f); // Chance scales based on difficulty
        return similarCount >= 1 && Random.value < changeChance;
    }

    private void ChangeBlockColor(BlockScript block)
    {
        List<int> availableColors = Enumerable.Range(1, block.blockColorList.Count - 1).ToList();
        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };

        foreach (Vector2 direction in directions)
        {
            Vector2 start = block.transform.position;
            Vector2 end = start + direction;
            RaycastHit2D hit = Physics2D.Raycast(end, end, .5f, blockLayer);
            if (hit.collider != null)
            {
                BlockScript adjacentBlock = hit.collider.GetComponent<BlockScript>();
                if (adjacentBlock != null)
                {
                    availableColors.Remove(adjacentBlock.blockHealth);
                }
            }
        }

        if (availableColors.Count > 0)
        {
            block.blockHealth = availableColors[Random.Range(0, availableColors.Count)];
            block.SetColorAndSprite();
        }
    }

    private bool ShouldLevelUp(BlockScript block)
    {
        float levelUpChance = Mathf.Lerp(0.05f, 0.4f, (globalDifficulty - 1) / 4f);
        return Random.value < levelUpChance && block.blockHealth < 7;
    }

    private void LevelUpBlock(BlockScript block)
    {
        block.blockHealth = Mathf.Min(block.blockHealth + 1, 6);
        block.SetColorAndSprite();
    }
}
