using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
     * PowerUp.cs
     * Created by: Nicolas Kaplan
     * Date Created: 2025-03-05
     * 
     * Description: This script keeps track of all PowerUps as derived from the PowerUp
     *              class. They will essentially send messages to the PowerUpManager
     *              to change certain player stats or enable new functionalities.
     * 
     * Last Changed by: Nicolas Kaplan
     * Last Date Changed: 2025-03-05
     *   v1.0
     */
public abstract class PowerUp
{
    public string Name;
    public string Description;
    public float Rarity;

    public abstract void ApplyPowerUp();
    public abstract void OnPowerUpRemove();
}
// Note: All power-up names are not final and are placeholders until a permanent name
// is decided upon.

// Score Multiplier Power-Up
public class ScoreMultiplierPowerUp : PowerUp
{
    float scoreMultAmount = 0.1f;
    public ScoreMultiplierPowerUp()
    {
        Name = "Score Multiplier";
        Description = "Multiply all score by 1.1x. (Stackable)";
        Rarity = 3.0f;
    }

    public override void ApplyPowerUp()
    {
        PowerUpManager.scoreMultiplier += scoreMultAmount; // 10% multiplier each time
    }
    public override void OnPowerUpRemove()
    {
        PowerUpManager.scoreMultiplier -= scoreMultAmount; // -10% multiplier each time
    }
}

public class DiagonalGemsPowerUp : PowerUp
{
    public DiagonalGemsPowerUp()
    {
        Name = "Diagonal Gems";
        Description = "Gems can now also change colors of other gems diagonally.";
        Rarity = 5.0f;
    }
    public override void ApplyPowerUp()
    {
        // set bool to true, send message to console saying it did it properly.
        PowerUpManager.diagonalGems = true;
    }
    public override void OnPowerUpRemove()
    {
        PowerUpManager.diagonalGems = false;
    }
}

public class MovesMultiplierPowerUp : PowerUp
{
    float movesMultAmount = 0.1f;
    public MovesMultiplierPowerUp()
    {
        Name = "Moves Multiplier";
        Description = "Moves are multiplied by 1.1x at the beginning of the round.";
        Rarity = 3.0f;
    }
    public override void ApplyPowerUp()
    {
        // set bool to true, send message to console saying it did it properly.
        PowerUpManager.startingMovesMultiplier += movesMultAmount;
    }
    public override void OnPowerUpRemove()
    {
        PowerUpManager.startingMovesMultiplier -= movesMultAmount;
    }
}