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
     * Last Date Changed: 2025-03-06
     * 
     *
     *   -> 1.0 Create PowerUp.cs and made a couple of starter power ups to set up
     *         a basic template for making more power ups.
     *   -> 1.1 Added 1up power-up and added Icon (Sprite) functionality to power-ups
     *   v1.1
     */
public abstract class PowerUp
{
    public string Name;
    public string Description;
    public float Rarity;
    public Sprite Icon;
    public PowerUp()
    {
        Icon = null; // set to null if no icon is found
    }
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
        Icon = Resources.Load<Sprite>("Icons/DefaultPowerUp");;
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
        Icon = Resources.Load<Sprite>("Icons/DefaultPowerUp");
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
        Icon = Resources.Load<Sprite>("Icons/DefaultPowerUp");;
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

public class OneUpPowerUp : PowerUp 
{
    public OneUpPowerUp()
    {
        Name = "1 UP";
        Description = "If you lose all your moves before meeting the criteria, gain an additional 20 moves and remove this power-up.";
        Rarity = 4.0f;
        Icon = Resources.Load<Sprite>("Icons/DefaultPowerUp");;
    }
    public override void ApplyPowerUp()
    {
        PowerUpManager.hasOneUp = true;
    }
    public override void OnPowerUpRemove()
    {
        PowerUpManager.hasOneUp = false;
    }
}
public class InventoryUpgradePowerUp : PowerUp
{
    int inventoryAddAmount = 2;
    public InventoryUpgradePowerUp()
    {
        Name = "Inventory Upgrade";
        Description = "Upgrade inventory by +2, counting the space this power-up takes up.";
        Rarity = 4.0f;
        Icon = Resources.Load<Sprite>("Icons/DefaultPowerUp");;
    }
    public override void ApplyPowerUp()
    {
        PowerUpManager.powerUpCapacity += inventoryAddAmount;
    }
    public override void OnPowerUpRemove()
    {
        PowerUpManager.powerUpCapacity -= inventoryAddAmount;
    }
}