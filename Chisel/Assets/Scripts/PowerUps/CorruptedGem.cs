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
        Name = "Corrupted Score";
        Description = "Increases your score multiplier by 20%, but reduces your starting moves by 5.";
        Rarity = 4.0f;
        Icon = Resources.Load<Sprite>("Icons/CorruptedGemIcon");
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
        Name = "Remove Reds";
        Description = "All Red gems are removed from hereon.";
        Rarity = 4.0f;
        Icon = Resources.Load<Sprite>("Icons/CorruptedGemIcon");
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
        Icon = Resources.Load<Sprite>("Icons/CorruptedGemIcon");
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