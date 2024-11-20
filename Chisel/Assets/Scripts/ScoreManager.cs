using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/*
     * ScoreManager.cs
     * Created by: Aetria Rescan
     * Date Created: 2024-11-19
     * 
     * Description: This code is written to increase the score as the player changes blocks colours, destroys blocks and completes combos
     * 
     * Last Changed by: Aetria Rescan
     * Last Date Changed: 2024-11-19
     * 
     * 
     *   -> 1.0 -Created ScoreManager and created the logic to get the score to change when a block changes colour or is destroyed.
     *   will include increases in score when player completes combos.
     */
public class ScoreManager : MonoBehaviour
{
    public int score = 0; // The player's score
    public TextMeshProUGUI scoreText; // Use TextMeshProUGUI for the score display

    // Method to increase the score for changing color
    public void AddScoreForColorChange()
    {
        score += 10;
        UpdateScoreText();
    }

    // Method to increase the score for breaking a block
    public void AddScoreForBlockBreak()
    {
        score += 20;
        UpdateScoreText();
    }

    // Method to update the score display on the UI
    private void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
    }
}
