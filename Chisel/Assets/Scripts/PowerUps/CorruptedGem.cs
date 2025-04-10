using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
     * CorruptedGem.cs
     * Created by: Nicolas Kaplan
     * Date Created: 2025-03-19
     * 
     * Description: This script keeps track of all Corrupted Gems as derived from the Corrupted Gems
     *              class. They will essentially send messages to the PowerUpManager
     *              to change certain player stats or enable new functionalities.
     * 
     * Last Changed by: Evan Robertson
     * Last Date Changed: 2025-03-21
     * 
     *
     *   -> 1.0 Create CorruptedGem.cs and made a couple of starter corrupted gems to set up
     *         a basic template for making more power ups.
     *   -> 1.1 Added stat tracking for save file
     *   v1.1
     */
public abstract class CorruptedGem
{
    public string Name;
    public string Description;
    public float Rarity;
    public Sprite Icon;
    public Color Color;

    // New fields to describe buff and nerf
    public string BuffDescription;
    public string NerfDescription;

    public CorruptedGem()
    {
        Icon = null;
    }
    public abstract void ApplyCorruptedGem();
    public abstract void OnCorruptedGemRemove();

    // Update stats in save manager
    protected void UpdateStats(string name)
    {
        SaveDataManager.Instance.corruptedGemsCollected++;

        //todo - keep track of how many times each corrupt gem is selected
    }
}

// Note: All power-up names are not final and are placeholders until a permanent name
// is decided upon.
public class ScorePlusMovesMinusGem : CorruptedGem
{
    public ScorePlusMovesMinusGem()
    {
        Name = "Score Corruptor";
        Description = "Increases your score multiplier by 20%, but reduces your starting moves by 5.";
        Rarity = 4.0f;
        Icon = Resources.Load<Sprite>("Icons/Corrupted Gem 3");
        Color = new Color(0.15f, 0.42f, 1f); 
        BuffDescription = "+20% Score Multiplier";
        NerfDescription = "-5 Starting Moves";
    }

    public override void ApplyCorruptedGem()
    {
        PowerUpManager.scoreMultiplier += 0.2f;
        PowerUpManager.extraStartingMoves -= 5;

        UpdateStats(Name);
    }

    public override void OnCorruptedGemRemove()
    {
        PowerUpManager.scoreMultiplier -= 0.2f;
        PowerUpManager.extraStartingMoves += 5;
    }
}

public class RemoveRedsGem : CorruptedGem
{
    public RemoveRedsGem()
    {
        Name = "Crimson Banisher";
        Description = "All Red gems are removed from hereon.";
        Rarity = 4.0f;
        Icon = Resources.Load<Sprite>("Icons/Corrupted Gem 2");
        Color = new Color(1f, 0f, .4f);
        BuffDescription = "Reds are removed, more moves kept overall!";
        NerfDescription = "Reds are removed, less score earned overall.";
    }

    public override void ApplyCorruptedGem()
    {
        PowerUpManager.skipAllReds = true;

        UpdateStats(Name);
    }

    public override void OnCorruptedGemRemove()
    {
        PowerUpManager.skipAllReds = false;

    }
}
public class ComboCatalystGem : CorruptedGem
{
    public ComboCatalystGem()
    {
        Name = "Combo Catalyst";
        Description = "If you have a combo multiplier, it is doubled. Otherwise all score is halved.";
        Rarity = 4.0f;
        Icon = Resources.Load<Sprite>("Icons/Corrupted Gem 1");
        Color = new Color(0f, 1f, .5f);
        BuffDescription = "2x and above are doubled.";
        NerfDescription = "1x is 0.5x.";
    }

    public override void ApplyCorruptedGem()
    {
        PowerUpManager.comboCatalyst = true;

        UpdateStats(Name);
    }

    public override void OnCorruptedGemRemove()
    {
        PowerUpManager.comboCatalyst = false;
    }
}
public class ShufflePowerUpsGem : CorruptedGem
{
    public ShufflePowerUpsGem()
    {
        Name = "Power Shuffle";
        Description = "Randomizes all active power-ups to new ones, once.";
        Rarity = 4.0f;
        Icon = Resources.Load<Sprite>("Icons/Corrupted Gem 4");
        Color = new Color(1f, 1f, 1f);
        BuffDescription = "Randomize all your power-ups";
        NerfDescription = "Completely randomly...";
    }

    public override void ApplyCorruptedGem()
    {
        Debug.Log("ShufflePowerUpsGem activated: Shuffling active power-ups.");

        List<PowerUp> currentActive = new List<PowerUp>(PowerUpManager.activePowerUps);
        foreach (var pu in currentActive)
        {
            pu.OnPowerUpRemove();
        }
        PowerUpManager.activePowerUps.Clear();

        List<PowerUp> pool = new List<PowerUp>
        {
            new ScoreMultiplierPowerUp(),
            new DiagonalGemsPowerUp(),
            new MovesMultiplierPowerUp(),
            new OneUpPowerUp(),
            new MovesBackChancePowerUp()
        };

        int count = currentActive.Count;
        List<PowerUp> newPowerUps = new List<PowerUp>();

        for (int i = 0; i < count; i++)
        {
            if (pool.Count == 0)
            {
                pool = new List<PowerUp>
                {
                    new ScoreMultiplierPowerUp(),
                    new DiagonalGemsPowerUp(),
                    new MovesMultiplierPowerUp(),
                    new OneUpPowerUp(),
                    new MovesBackChancePowerUp()
                };
            }
            int randomIndex = Random.Range(0, pool.Count);
            newPowerUps.Add(pool[randomIndex]);
            pool.RemoveAt(randomIndex);
        }

        foreach (var newPU in newPowerUps)
        {
            PowerUpManager.ApplyPowerUp(newPU);
        }

        UpdateStats(Name);
    }

    public override void OnCorruptedGemRemove()
    {
        // No removal effect for a one-time shuffle.
    }
}
public class TimeDrainGem : CorruptedGem
{
    private Coroutine timeDrainCoroutine;

    public TimeDrainGem()
    {
        Name = "Temporal Decay";
        Description = "Your moves now become time—your moves automatically drop by 1 per second (via time drain), and you no longer lose moves by breaking blocks. Gain moves through the move-back system. Time stops when win/game-over screens are up or if you're not actively playing.";
        Rarity = 4.0f;
        Icon = Resources.Load<Sprite>("Icons/Corrupted Gem 5");
        Color = new Color(1f, 1f, 1f);
        BuffDescription = "Moves drop by 1 per second.";
        NerfDescription = "Block breaks no longer reduce moves.";
    }

    public override void ApplyCorruptedGem()
    {
        Debug.Log("TimeDrainGem applied: starting move drain effect.");
        PowerUpManager.timeDrainActive = true;  // Activate the time drain effect.
        GameStateControl gsc = GameObject.FindObjectOfType<GameStateControl>();
        if (gsc != null)
        {
            timeDrainCoroutine = gsc.StartCoroutine(TimeDrainCoroutine(gsc));
        }
        UpdateStats(Name);
    }

    public override void OnCorruptedGemRemove()
    {
        Debug.Log("TimeDrainGem removed: stopping move drain effect.");
        PowerUpManager.timeDrainActive = false; // Disable the time drain effect.
        GameStateControl gsc = GameObject.FindObjectOfType<GameStateControl>();
        if (gsc != null && timeDrainCoroutine != null)
        {
            gsc.StopCoroutine(timeDrainCoroutine);
            timeDrainCoroutine = null;
        }
    }

    private IEnumerator TimeDrainCoroutine(GameStateControl gsc)
    {
        GameLoopManager gm = GameObject.FindObjectOfType<GameLoopManager>();
        while (true)
        {
            bool inPlayState = gm != null && (gm.CurrentStage == GameLoopManager.GameStage.Play ||
                                               gm.CurrentStage == GameLoopManager.GameStage.PlaySpecialLevel);
            if (inPlayState && !gsc.WinCanvas.activeSelf && !gsc.GameOverCanvas.activeSelf)
            {
                gsc.DecrementMoves();
                Debug.Log("TimeDrainGem: 1 move drained due to time.");
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
public class MovesBackReworkGem : CorruptedGem
{
    public MovesBackReworkGem()
    {
        Name = "Chance Catalyst";
        Description = "Reduces your chance to get moves back to 10%, but when you do, you gain 10 moves.";
        Rarity = 4.0f;
        Icon = Resources.Load<Sprite>("Icons/Corrupted Gem 1"); 
        Color = new Color(.5f, 1f, .65f);
        BuffDescription = "Get 10 moves back on large combos!";
        NerfDescription = "Reduced probability of moves back. (10%)";
    }

    public override void ApplyCorruptedGem()
    {
        Debug.Log("Reboot Catalyst applied: modifying move-back properties.");
        PowerUpManager.movesBackModifierActive = true;
        PowerUpManager.extraMoveBackAward = 10;
        UpdateStats(Name);
    }

    public override void OnCorruptedGemRemove()
    {
        Debug.Log("Reboot Catalyst removed: restoring move-back properties.");
        PowerUpManager.movesBackModifierActive = false;
        PowerUpManager.extraMoveBackAward = 1; 
    }
}
public class LevelSkipGem : CorruptedGem
{
    public LevelSkipGem()
    {
        Name = "Warp Crystal";
        Description = "Skip 3 stages, ahead to the next level.";
        Rarity = 5.0f;
        Icon = Resources.Load<Sprite>("Icons/Corrupted Gem 2"); 
        Color = new Color(0.65f, 0.5f, 1f);
        BuffDescription = "Rewards bypassed and level advanced.";
        NerfDescription = "Lose rewards in between.";
    }

    public override void ApplyCorruptedGem()
    {
        Debug.Log("LevelSkipGem applied: Initiating level skip.");
        GameLoopManager gm = GameObject.FindObjectOfType<GameLoopManager>();
        if (gm != null)
        {
            gm.SkipRewardAndAdvance();
        }
        UpdateStats(Name);
    }

    public override void OnCorruptedGemRemove()
    {
        // This gem is one-time use; no removal effect is necessary.
    }
}
