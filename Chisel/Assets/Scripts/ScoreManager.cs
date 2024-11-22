using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/*
     * ScoreManager.cs
     * Created by: Aetria Rescan
     * Date Created: 2024-11-19
     * 
     * Description: This code is written to increase the score as the player changes blocks colours, destroys blocks and completes combos
     * 
     * Last Changed by: Nicolas Kaplan
     * Last Date Changed: 2024-11-21
     * 
     * 
     *   -> 1.0 -Created ScoreManager and created the logic to get the score to change when a block changes colour or is destroyed.
     *   will include increases in score when player completes combos.
     *   -> 1.1 Altered logic to fit UI standards better, changed scoreText to private,
     *   also added combo logic as well as UI functionality for combos.
     *   -> 1.2 Changed score text to not display a + before the score anymore, added moves back system
     *      
     *   v1.2
     */
public class ScoreManager : MonoBehaviour
{
    [SerializeField] private RectTransform comboFillImage; // image that will be masked
    public int score = 0; // The player's score
    private float currentMultiplier = 1f; // current multiplier value for combo points
    private float multiplierIncrease = 0.2f; // at which rate to increase the multiplier value
    private float maxMultiplier = 3f; // the max multiplier value before it stops increasing

    [SerializeField] private GameObject scoreObject; // score object to instantiate when score is added.
    private int comboAccumulatedScore = 0; // tracks score per combo
    private int currentComboCount = 0;
    private int minimumComboForMoves = 10;
    public delegate void OnMoveGained();
    public static event OnMoveGained MoveGained;
    private bool isInCombo = false; // bool checks whether code is in combo at the moment
    private GameObject currentScoreObject;
    private Coroutine scoreObjectDeleteCoroutine;
    
    public int minimumScoreForLevel;


    [SerializeField] private TextMeshProUGUI scoreTextLight; // Use TextMeshProUGUI for the score display
    [SerializeField] private TextMeshProUGUI scoreTextDark; // Use TextMeshProUGUI for the score display
    [SerializeField] private Transform worldCanvas; // world canvas
    

    void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        UpdateScoreText();
    }

    // Method to increase the score for changing color
    public void AddScoreForColorChange()
    {
        AddScore(50);
    }

    private void UpdateComboBar()
    {
        if (comboFillImage != null)
        {
            float fillAmount = (currentMultiplier - 1f) / (maxMultiplier - 1f);
            float fullWidth = comboFillImage.parent.GetComponent<RectTransform>().rect.width;
            float maskAmount = fullWidth * (1 - fillAmount);
            comboFillImage.GetComponent<RectMask2D>().padding = new Vector4(0, 0, maskAmount, 0);
        }
    }

    // Method to increase the score for breaking a block
    public void AddScoreForBlockBreak(int scoreMultiplier, int blockHealth)
    {
        int calculatedScore = Mathf.RoundToInt(scoreMultiplier * currentMultiplier);
        if (!isInCombo)
        {
            isInCombo = true;
            comboAccumulatedScore = 0;
            currentComboCount = 0;
        }

        currentComboCount++;
        comboAccumulatedScore += calculatedScore;
        AddScore(calculatedScore);

        if (currentComboCount >= minimumComboForMoves && blockHealth == 1)
        {
            AwardExtraMove();
        }

        currentMultiplier = Mathf.Min(currentMultiplier + multiplierIncrease, maxMultiplier);
        UpdateComboBar();
        // play sound?
    }
    
    private void AwardExtraMove()
    {
        MoveGained?.Invoke();
    }
    public void EndCombo()
    {
        if (isInCombo)
        {
            if (comboAccumulatedScore > 0)
            {
                ScoreTooltip(comboAccumulatedScore);
            }
            
            isInCombo = false;
            comboAccumulatedScore = 0;
            currentComboCount = 0;
            ResetMultiplier();
        }
    }
    public int GetCurrentComboCount()
    {
        return currentComboCount;
    }
    public void ResetMultiplier()
    {
        currentMultiplier = 1f;
    }


    // Method to update the score display on the UI
    private void UpdateScoreText()
    {
        if (scoreTextLight != null && scoreTextDark != null)
        {
            scoreTextLight.text = $"{score}";
            scoreTextDark.text = $"{score}";
        }
        
    }
    public void ScoreTooltip(int score)
    {
        Vector2 textPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        textPosition += new Vector2(0, -6f);
        currentScoreObject = Instantiate(scoreObject, textPosition, Quaternion.identity, worldCanvas);
        if (currentScoreObject.GetComponent<TextMeshProUGUI>() != null) 
        { 
            if (currentScoreObject.GetComponent<TextMeshProUGUI>().enabled == false)
            {
                currentScoreObject.GetComponent<TextMeshProUGUI>().enabled = true;
            }
        }
        currentScoreObject.GetComponent<RectTransform>().position = textPosition;
        scoreObject.GetComponent<TextMeshProUGUI>().text = $"{score}";
        Debug.Log($"Spawned a score tooltip @ {textPosition}");
        
        if (scoreObjectDeleteCoroutine != null)
        {
            StopCoroutine(DeleteScoreObject(currentScoreObject));
        }
        scoreObjectDeleteCoroutine = StartCoroutine(DeleteScoreObject(currentScoreObject));
    }
    IEnumerator DeleteScoreObject(GameObject scoreObject)
    {
        yield return new WaitForSeconds(1.3f);
        Destroy(scoreObject);
    }
}
