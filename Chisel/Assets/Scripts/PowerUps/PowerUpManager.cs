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
     * Last Date Changed: 2025-03-05
     *
     *  -> 1.0 - Created PowerUpManager.cs and wrote a couple of changeable stats.
     *   v1.0
     */
public static class PowerUpManager
{
    // Player stats that can be changed:
    public static int extraStartingMoves = 0; 
    public static float startingMovesMultiplier = 1.0f;
    public static float addedEndScore = 0f;
    public static float endScoreMultiplier = 1.0f;
    public static float addedScore = 0f;
    public static float scoreMultiplier = 1.0f; // ADD to this multiplier and any others.

    public static int powerUpCapacity = 3; // How many power-ups the player can have
    public static float difficultyAdjustment = 0f;
    public static float difficultyAdjustmentMultiplier = 1.0f;

    // Special toggleable abilities:
    public static bool diagonalGems = false;
    public static bool chanceGemBombs = false;
    public static bool scoreSiphon = false;

    // List of active power-ups
    public static List<PowerUp> activePowerUps = new List<PowerUp>();

    // Method to add power-ups and apply their effects
    public static void ApplyPowerUp(PowerUp powerUp)
    {
        if (activePowerUps.Count < powerUpCapacity)
        {
            activePowerUps.Add(powerUp);
            powerUp.ApplyPowerUp(); // Apply the effect immediately
        }
    }
    public static void OnPowerUpRemove(PowerUp powerUp)
    {
        if (activePowerUps.Contains(powerUp))
        {
            activePowerUps.Remove(powerUp);
            powerUp.OnPowerUpRemove();
        }
    }
}

