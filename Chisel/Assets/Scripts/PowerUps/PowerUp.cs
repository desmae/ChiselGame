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
     * Last Date Changed: 2025-04-08
     * 
     *
     *   -> 1.0 Create PowerUp.cs and made a couple of starter power ups to set up
     *         a basic template for making more power ups.
     *   -> 1.1 Added 1up power-up and added Icon (Sprite) functionality to power-ups
     *   -> 1.2 Removed inventory capacity system and power up that changed it.
     *   -> 1.3 Added stat tracking for save file
     *   -> 1.4 Changed Powerups to have two more properties, unique and color.
     *   v1.4
     */
public abstract class PowerUp
{
    public string Name;
    public string Description;
    public float Rarity;
    public Sprite Icon;
    public bool unique = false; // if true, only one of this power-up can be obtained.
    public Color powerUpColor = Color.white;
    public PowerUp()
    {
        Icon = null; // set to null if no icon is found
    }
    public abstract void ApplyPowerUp();
    public abstract void OnPowerUpRemove();

    // Update stats in save manager
    protected void UpdateStats(string name)
    {
        SaveDataManager.Instance.powerupsCollected++;

        //todo - keep track of how many times each powerup is selected 
    }
}
// Note: All power-up names are not final and are placeholders until a permanent name
// is decided upon.

// Score Multiplier Power-Up
public class ScoreMultiplierPowerUp : PowerUp
{
    float scoreMultAmount = 0.1f;
    public ScoreMultiplierPowerUp()
    {
        Name = "Square Spinel";
        Description = "Multiply all score by 1.1x. (Stackable)";
        Rarity = 3.0f;
        Icon = Resources.Load<Sprite>("Icons/Square Spinel");
        unique = false;
        powerUpColor = new Color(.75f, .65f, 1f);
    }

    public override void ApplyPowerUp()
    {
        PowerUpManager.scoreMultiplier += scoreMultAmount; // 10% multiplier each time

        UpdateStats(Name);
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
        Name = "Octagonal Onyx";
        Description = "Gems can now also change colors of other gems diagonally.";
        Rarity = 5.0f;
        Icon = Resources.Load<Sprite>("Icons/Octagonal Onyx");
        unique = true;
        powerUpColor = new Color(.25f, .25f, .25f);
    }
    public override void ApplyPowerUp()
    {
        // set bool to true, send message to console saying it did it properly.
        PowerUpManager.diagonalGems = true;

        UpdateStats(Name);
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
        Name = "Tempered Baguette Topaz";
        Description = "Moves are multiplied by 1.1x at the beginning of the round.";
        Rarity = 3.0f;
        Icon = Resources.Load<Sprite>("Icons/Tempered Baguette Topaz");
        unique = false;
        powerUpColor = new Color(1, .8f, 0f);
    }
    public override void ApplyPowerUp()
    {
        // set bool to true, send message to console saying it did it properly.
        PowerUpManager.startingMovesMultiplier += movesMultAmount;

        UpdateStats(Name);
    }
    public override void OnPowerUpRemove()
    {
        PowerUpManager.startingMovesMultiplier -= movesMultAmount;
    }
}

public class OneUpPowerUp : PowerUp 
{
    public OneUpPowerUp()
    {
        Name = "Heart Hessonite";
        Description = "If you lose all your moves before meeting the criteria, gain an additional 20 moves and remove this power-up.";
        Rarity = 4.0f;
        Icon = Resources.Load<Sprite>("Icons/Heart Hessonite");
        unique = true;
        powerUpColor = new Color(.65f, 0f, 0f);

    }
    public override void ApplyPowerUp()
    {
        Debug.Log("OneUpPowerUp ApplyPowerUp called. Setting hasOneUp to true.");
        PowerUpManager.hasOneUp = true;
        UpdateStats(Name);
    }

    public override void OnPowerUpRemove()
    {
        PowerUpManager.hasOneUp = false;
    }
}
public class MovesBackChancePowerUp : PowerUp
{
    public MovesBackChancePowerUp()
    {
        Name = "Pear Drop Peridot";
        Description = "The chance of you getting your moves back during a large combo are increased by 10%.";
        Rarity = 4.0f;
        Icon = Resources.Load<Sprite>("Icons/Pear Drop Peridot");
        unique = true;
        powerUpColor = new Color(.6f, 1f, .35f);
    }
    public override void ApplyPowerUp()
    {
        PowerUpManager.movesBackChance += 0.1f; // +10%
        UpdateStats(Name);
    }
    public override void OnPowerUpRemove()
    {
        PowerUpManager.movesBackChance -= 0.1f; 
    }
}