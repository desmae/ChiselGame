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
     * Last Date Changed: 2024-11-11
     * 
     * 
     *   -> 1.0 - Created BlockScript.cs and added basic block functionality,
     *      color changing when broken, and disappearing when a red block is broken,
     *      as well as changing adjacent block colors
     *   
     *   -> 1.1 - Updated code to allow all blocks to reference colors from a singleton 
     *      manager.
     *      
     *   -> 1.2 - blockColors now use default colors when starting the game scene
     *   
     *   -> 1.3 - Removed excessive Debug.Log messages to clean up console.
     *      Added blockList.Remove() functionality to make the win screen work.
     *      An issue has also been found where orange blocks disappear instead of becoming
     *      red. Troubleshooting required.
     *   -> 1.4 - Added code for reducing moves only when a block is hit. 
     *   -> 1.5 - Particle system added to blocks as soon as they are broken.
     *   -> 1.6 - Added Animations to each of the blocks with parameters to control
     *      animation offsets and speed multipliers.
     *   v1.6
     */
public class BlockScript : MonoBehaviour
{

    // Animation references
    private Animator animator;
    private float animSpeed;
    private float animDelay;

    public List<SpriteRenderer> blockSpriteList;
    public List<Color> blockColorList;
    public int blockHealth;
    public LayerMask blockLayer;
    GameStateControl gameStateControl;
    private static HashSet<BlockScript> hitBlocks = new HashSet<BlockScript>();
    private static bool isChainReactionInProgress = false;

    void Start()
    {
        if (SettingsManager.Instance.GetColors().Count != 0)
        {
            blockColorList = SettingsManager.Instance.GetColors();
        }
        else
        {
            blockColorList = SettingsManager.Instance.GetDefaultColors();
        }
        
        gameStateControl = GameObject.FindGameObjectWithTag("GameStateManager")
        .GetComponent<GameStateControl>();
        animator = GetComponent<Animator>();
        ChooseRandomColor();
        SetColorAndSprite();
        SetAnimationParams();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider != null && hit.transform.gameObject == gameObject)
            {
                gameStateControl.DecrementMoves();

                OnBreak();
            }
        }
    }

    public void OnBreak()
    {
        animator.SetTrigger("GemBroken");
        ParticleSystem.EmitParams emitParams = new ParticleSystem.EmitParams
        {
            startColor = blockColorList[blockHealth - 1]
        };

        GetComponent<ParticleSystem>().Emit(emitParams, 1);

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

    private void SetAnimationParams()
    {
        animSpeed = Random.Range(0f, 2f);
        animDelay = Random.Range(0f, 2f);
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
        gameStateControl.blockList.Remove(gameObject);
        Destroy(gameObject);
    }

    
    private void OnDestroy()
    {
        StopAllCoroutines();
    }

}