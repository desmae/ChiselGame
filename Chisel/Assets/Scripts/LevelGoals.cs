using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
     * LevelGoals.cs
     * Created by: Nicolas Kaplan
     * Date Created: 2025-03-18
     * 
     * Description: This script is in charge of keeping a compiled list of special level types.
     * 
     * Last Changed by: Nicolas Kaplan
     * Last Date Changed: 2025-03-18
     *
     *  -> 1.0 - Created LevelGoals.cs and created 3 special level goals.

     *   v1.0
     */
public class ScoreCapGoal : ISpecialGoal
{
    private int scoreCap;
    private ScoreManager scoreManager;

    public ScoreCapGoal(int levelNumber)
    {
        scoreCap = 50000 * levelNumber;
    }

    public string GetGoalDescription()
    {
        return $"Reach at least {scoreCap} points!";
    }
    public void InitializeGoal()
    {
        scoreManager = GameObject.FindObjectOfType<ScoreManager>();
        Debug.Log($"[ScoreCapGoal] Need to reach {scoreCap} points to complete this goal.");
    }

    public bool IsGoalMet()
    {
        if (scoreManager == null) return false;
        int currentStageScore = scoreManager.GetStageScore();
        return (currentStageScore >= scoreCap);
    }


    public void OnGoalComplete()
    {
        GameStateControl gsc = GameObject.FindObjectOfType<GameStateControl>();
        if (gsc != null)
        {
            gsc.SetCustomWinText($"You got more than the par score ({scoreCap}), nice!");
        }
    }
}

public class ComboMultCapGoal : ISpecialGoal
{
    private int comboCap;
    private ScoreManager scoreManager;
    private GameStateControl gameState;

    public ComboMultCapGoal(int levelNumber)
    {
        if (levelNumber > 1)
        {
            comboCap = levelNumber;
        }
        else
        {
            // if level number is 1, make the cap 2x.
            comboCap = 2;
        }
    }

    public string GetGoalDescription()
    {
        return $"Finish the level with at least a {comboCap}x multiplier on your combo!";
    }   


    public void InitializeGoal()
    {
        scoreManager = GameObject.FindObjectOfType<ScoreManager>();
        gameState = GameObject.FindObjectOfType<GameStateControl>();
    }

    public bool IsGoalMet()
    {
        // if no blocks are left and combo cap is above or equal to the cap = true
        if (gameState != null && gameState.blockList.Count <= 0)
        {
            if (scoreManager != null && scoreManager.CurrentMultiplier >= comboCap)
            {
                return true;
            }
        }
        return false;
    }

    public void OnGoalComplete()
    {
        GameStateControl gsc = GameObject.FindObjectOfType<GameStateControl>();
        if (gsc != null)
        {
            gsc.SetCustomWinText($"You ended with at least a {comboCap}x multiplier!");
        }
    }
}
public class TimeLimitGoal : ISpecialGoal
{
    private float timeLimit;
    private float elapsedTime;

    public TimeLimitGoal(float limit)
    {
        timeLimit = limit;
    }

    public void InitializeGoal()
    {
        elapsedTime = timeLimit;
        Debug.Log($"Finish before {timeLimit} seconds pass!"); // maybe consider doing UI later
    }

    public bool IsGoalMet()
    {
        elapsedTime -= Time.deltaTime;

        if (elapsedTime <= 0f)
        {
            GameStateControl gsc = GameObject.FindObjectOfType<GameStateControl>();
            if (gsc != null)
            {
                gsc.DisplayGameOverScreen();
                return false;
            }
        }

        bool boardCleared = (GameObject.FindObjectOfType<GameStateControl>().blockList.Count <= 0);
        return boardCleared && (elapsedTime <= timeLimit);
    }

    public void OnGoalComplete()
    {
        GameStateControl gsc = GameObject.FindObjectOfType<GameStateControl>();
        if (gsc != null)
        {
            gsc.SetCustomWinText($"You beat the clock with 0:{elapsedTime} left.");
        }
    }


    public string GetGoalDescription()
    {
        return $"Clear the level in under {timeLimit} seconds!";
    }
}
