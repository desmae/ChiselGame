using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
     * PowerUpManager.cs
     * Created by: Nicolas Kaplan
     * Date Created: 2025-03-05
     * 
     * Description: This script is in charge of keep track of player's changable stats
     *              throughout the game and changing them according to external power ups.
     * 
     * Last Changed by: Nicolas Kaplan
     * Last Date Changed: 2025-03-18
     *
     *  -> 1.0 - Created PowerUpManager.cs and wrote a couple of changeable stats.
     *  -> 1.1 - Changed some abilities' types from float to int, and added some new
     *          abilities as well.
     *  -> 1.2 - Added event to update UI, since PowerUpManager does not derive from MonoBehaviour.
     *  -> 1.3 - Added a moves back chance variable
     *   v1.3
     */
public static class PowerUpManager
{
    // Player stats that can be changed:
    public static int extraStartingMoves = 0; 
    public static float startingMovesMultiplier = 1.0f;
    public static int addedEndScore = 0;
    public static float endScoreMultiplier = 1.0f;
    public static int addedScore = 0;
    public static float scoreMultiplier = 1.0f; // ADD to this multiplier and any others.

    public static float addedComboAmount = 0f; // add the amount here to the total player combo
    public static float comboAmountMultiplier = 1.0f; // add to this to multiply total player combo

    public static float movesBackChance = 6f; // 60% chance to get a move back default

    public static int powerUpCapacity = 5; // How many power-ups the player can have
    public static float difficultyAdjustment = 0f;
    public static float difficultyAdjustmentMultiplier = 1.0f;

    // Special toggleable abilities:
    public static bool diagonalGems = false;
    public static bool chanceGemBombs = false;
    public static bool scoreSiphon = false;
    public static bool hasOneUp = false;

    public static bool skipAllReds = false;
    public static bool comboCatalyst = false;

    public delegate void PowerUpListChanged();
    public static event PowerUpListChanged OnPowerUpListChanged;
    
    // List of active power-ups

    public static List<PowerUp> activePowerUps = new List<PowerUp>();

    // Method to add power-ups and apply their effects
    public static void ApplyPowerUp(PowerUp powerUp)
    {
        Debug.Log($"[PowerUpManager] ApplyPowerUp called for '{powerUp?.Name}' (type: {powerUp?.GetType()})");
        if (activePowerUps.Count < powerUpCapacity)
        {
            activePowerUps.Add(powerUp);
            powerUp.ApplyPowerUp();
            Debug.Log($"[PowerUpManager] After calling ApplyPowerUp, hasOneUp = {hasOneUp}");

            OnPowerUpListChanged?.Invoke();
        }
    }

    public static void OnPowerUpRemove(PowerUp powerUp)
    {
        if (activePowerUps.Contains(powerUp))
        {
            activePowerUps.Remove(powerUp);
            powerUp.OnPowerUpRemove();

            OnPowerUpListChanged?.Invoke();
        }
    }

    public static List<CorruptedGem> activeCorruptedGems = new List<CorruptedGem>();

    public static void ApplyCorruptedGem(CorruptedGem gem)
    {
        // The user picks it, or it’s forced upon them
        activeCorruptedGems.Add(gem);
        gem.ApplyCorruptedGem();
    }

    // No public method for removing a single gem, so user can’t remove them
    // Instead, we only have a private or internal method for the game to remove them all
    internal static void RemoveAllCorruptedGems()
    {
        foreach (var gem in activeCorruptedGems)
        {
            gem.OnCorruptedGemRemove(); // revert buff & nerf
        }
        activeCorruptedGems.Clear();
    }
}

