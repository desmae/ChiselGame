using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
     * BlockScript.cs
     * Created by: Nicolas Kaplan
     * Date Created: 2024-10-08
     * 
     * Description: This code is written for individual block behaviours such as changing color, disappearing, and checking for
     * nearby block colors to see if they should change too.
     * 
     * Last Changed by: Nicolas Kaplan
     * Last Date Changed: 2024-10-08
     * 
     * 
     *   -> 1.0 - Created BlockScript.cs and added basic block functionality,
     *      color changing when broken, and disappearing when a red block is broken,
     *      as well as changing adjacent block colors
     *   
     *   
     *   
     *   v1.0
     */
public class BlockScript : MonoBehaviour
{
    public List<SpriteRenderer> blockSpriteList;
    public List<Color> blockColorList;
    public int blockHealth;
    public LayerMask blockLayer;

    private static HashSet<BlockScript> hitBlocks = new HashSet<BlockScript>();
    private static bool isChainReactionInProgress = false;
    void Start()
    {
        ChooseRandomColor();
        SetColorAndSprite();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.transform.gameObject == gameObject)
            {
                OnBreak();
            }
        }
    }

    public void OnBreak()
    {
        if (hitBlocks.Contains(this))
        {
            return; 
        }

        if (!isChainReactionInProgress)
        {
            // Start of a new chain reaction
            isChainReactionInProgress = true;
            hitBlocks.Clear();
        }

        hitBlocks.Add(this);

        int currentBlockHealth = blockHealth;
        ChangeBlockHealth(currentBlockHealth - 1);

        // Check and change health of adjacent blocks
        CheckAdjacentBlocks(currentBlockHealth);

        if (!isChainReactionInProgress)
        {
            // End of the chain reaction
            ResetHitBlocks();
        }
    }

    private void CheckAdjacentBlocks(int originalHealth)
    {
        Vector2[] directions = { Vector2.up, Vector2.down, Vector2.left, Vector2.right };
        bool foundAdjacentBlock = false;

        foreach (Vector2 direction in directions)
        {
            if (CheckAdjacentBlock(direction, originalHealth))
            {
                foundAdjacentBlock = true;
            }
        }

        if (!foundAdjacentBlock)
        {
            // This block is the end of a branch in the chain reaction
            isChainReactionInProgress = false;
        }
    }

    private bool CheckAdjacentBlock(Vector2 direction, int originalHealth)
    {
        Vector2 start = transform.position;
        Vector2 end = start + direction;
        RaycastHit2D hit = Physics2D.Raycast(end, end, .5f, blockLayer);

        if (hit.collider != null)
        {
            BlockScript adjacentBlock = hit.collider.gameObject.GetComponent<BlockScript>();
            if (adjacentBlock != null && adjacentBlock.blockHealth == originalHealth && !hitBlocks.Contains(adjacentBlock))
            {
                adjacentBlock.OnBreak();
                Debug.DrawLine(start, end, Color.green, .5f);
                return true;
            }
            else
            {
                Debug.DrawLine(start, end, Color.red, .5f);
            }
        }
        else
        {
            Debug.DrawLine(start, end, Color.red, .5f);
        }

        return false;
    }

    private void ChangeBlockHealth(int newHealth)
    {
        int oldHealth = blockHealth;
        blockHealth = Mathf.Clamp(newHealth, 0, blockColorList.Count - 1);

        if (blockHealth <= 0)
        {
            StartCoroutine(DestroyNextFrame());
        }
        SetColorAndSprite();
        Debug.Log($"Block at {transform.position} changed health from {oldHealth} to {blockHealth}");

    }

    private void ChooseRandomColor()
    {
        int randomNumber = Random.Range(1, blockColorList.Count);
        blockHealth = randomNumber;
    }

    private void SetColorAndSprite()
    {
        GetComponent<SpriteRenderer>().color = blockColorList[blockHealth];
        GetComponent<SpriteRenderer>().sprite = blockSpriteList[blockHealth].sprite;
    }

    private static void ResetHitBlocks()
    {
        hitBlocks.Clear();
        isChainReactionInProgress = false;
    }

    private IEnumerator DestroyNextFrame()
    {
        yield return null;
        Destroy(gameObject);
    }

    
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}