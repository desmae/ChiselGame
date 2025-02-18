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
     * Last Date Changed: 2025-02-17
     * 
     * 
     *   -> 1.0 -Created ScoreManager and created the logic to get the score to change when a block changes colour or is destroyed.
     *   will include increases in score when player completes combos.
     *   -> 1.1 Altered logic to fit UI standards better, changed scoreText to private,
     *   also added combo logic as well as UI functionality for combos.
     *   -> 1.2 Changed score text to not display a + before the score anymore, added moves back system
     *   -> 1.3 Changed combo text to be more cool, and added a multiplier feature with animations
     *   v1.3
     */
public class ScoreManager : MonoBehaviour
{
    [SerializeField] private RectTransform comboFillImage;
    public int score = 0;
    private float currentMultiplier = 1f;
    private float multiplierIncrease = 1f;
    private float maxMultiplier = 5f; 

    [SerializeField] Animator comboAnimator;
    [SerializeField] TextMeshProUGUI comboMultTextLight;
    [SerializeField] TextMeshProUGUI comboMultTextDark;
    private int comboBarScore = 0; 
    private int maxComboBarScore = 25000;
    private int comboDrainRate = 1000;
    private bool isDraining = false;

    public int minimumScoreForLevel = 0; 

    [SerializeField] private GameObject scoreObject;
    private int comboAccumulatedScore = 0;
    private int currentComboCount = 0;
    private int minimumComboForMoves = 10;
    public delegate void OnMoveGained();
    public static event OnMoveGained MoveGained;
    private bool isInCombo = false;
    private GameObject currentScoreObject;
    private Coroutine scoreObjectDeleteCoroutine;

    [SerializeField] private TextMeshProUGUI scoreTextLight;
    [SerializeField] private TextMeshProUGUI scoreTextDark;
    [SerializeField] private Transform worldCanvas;
    private void Start()
    {
        StartCoroutine(DecrementComboBar());

        if (comboMultTextLight == null || comboMultTextDark == null)
        {
            Debug.LogError("[ScoreManager] ERROR: Combo multiplier UI text is NULL. Assign it in the Inspector!");
        }
    }


    private void Update()
    {
        if (comboBarScore > 0)
        {
            UpdateComboBar();
            UpdateComboAnimation();
        }
    }
    private void UpdateComboAnimation()
    {
        if (comboAnimator != null)
        {
            comboAnimator.SetFloat("multAmount", currentMultiplier);
        }

        if (comboMultTextLight != null && comboMultTextDark != null)
        {
            int displayMultiplier = Mathf.FloorToInt(currentMultiplier); // Ensure it's an integer
            comboMultTextDark.text = $"x{displayMultiplier}";
            comboMultTextLight.text = $"x{displayMultiplier}";

            // Force UI text to refresh
            comboMultTextDark.ForceMeshUpdate();
            comboMultTextLight.ForceMeshUpdate();

            Debug.Log($"[ScoreManager] UI Updated: Multiplier Display = x{displayMultiplier}");
        }
        else
        {
            Debug.LogError("[ScoreManager] ERROR: One or both multiplier text objects are NULL!");
        }
    }

    IEnumerator DecrementComboBar()
    {
        while (true)
        {
            if (comboBarScore > 0)
            {
                comboBarScore = Mathf.Max(0, comboBarScore - comboDrainRate);
                UpdateComboBar();
            }
            yield return new WaitForSeconds(1f);
        }
    }

    void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
        UpdateScoreText();
    }

    public void AddScoreForColorChange()
    {
        AddScore(50);
    }

    private void UpdateComboBar()
    {
        if (comboFillImage != null)
        {
            float fillAmount = (float)comboBarScore / maxComboBarScore;  
            float fullWidth = comboFillImage.parent.GetComponent<RectTransform>().rect.width; 

            comboFillImage.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, fullWidth * fillAmount);

            comboFillImage.anchoredPosition = new Vector2(0, comboFillImage.anchoredPosition.y);

        }
    }

    public void AddScoreForBlockBreak(int scoreAmount)
    {
        if (!isInCombo)
        {
            isInCombo = true;
            comboAccumulatedScore = 0;
            currentComboCount = 0;
        }

        int baseScore = scoreAmount; // Base score before any multipliers
        int finalScore = baseScore;

        // Add base score to combo bar (capped at 25,000)
        comboBarScore = Mathf.Min(comboBarScore + baseScore, maxComboBarScore);

        // Determine Multiplier
        float percentageFull = (float)comboBarScore / maxComboBarScore;
        if (percentageFull >= 0.8f)
            currentMultiplier = 5f; // 80%+ x5
        else if (percentageFull >= 0.6f)
            currentMultiplier = 4f; // 60%+ x4
        else if (percentageFull >= 0.4f)
            currentMultiplier = 3f; // 40%+ x3
        else if (percentageFull >= 0.2f)
            currentMultiplier = 2f; // 20%+ x2
        else
            currentMultiplier = 1f; // Default

        Debug.Log($"[ScoreManager] Multiplier Updated: {currentMultiplier} | ComboBar: {comboBarScore}/{maxComboBarScore} ({percentageFull * 100:F1}%)");

        // Apply multiplier
        finalScore *= (int)currentMultiplier;

        // Update Score and UI
        AddScore(finalScore);
        UpdateComboAnimation();  // Ensure UI is updated immediately

        currentComboCount++;
        comboAccumulatedScore += finalScore;
        if (currentComboCount >= minimumComboForMoves)
        {
            AwardExtraMove();
        }
        UpdateComboBar();
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
        }
    }

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
            if (!currentScoreObject.GetComponent<TextMeshProUGUI>().enabled)
            {
                currentScoreObject.GetComponent<TextMeshProUGUI>().enabled = true;
            }
        }
        currentScoreObject.GetComponent<RectTransform>().position = textPosition;
        scoreObject.GetComponent<TextMeshProUGUI>().text = $"{score}";

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
