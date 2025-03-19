using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameLoopManager;
/*
     * LevelSelectionPanel.cs
     * Created by: Nicolas Kaplan
     * Date Created: 2025-03-17
     * 
     * Description: This script is in charge of the level selection panel used to let the player choose and begin a level.
     * 
     * Last Changed by: Nicolas Kaplan
     * Last Date Changed: 2025-03-17
     * 
     *   -> 1.0 - Created LevelSelectionPanel.cs and hooked it to GameLoopManager.cs to be used there.
     *   -> 1.1 - Now each level card displays the actual level difficulty instead of some arbitrary number
     *   v1.1
     */
[Serializable]
public class LevelOptionUI
{
    public Image previewImage;       // Drag the PreviewImage here
    public TextMeshProUGUI levelNameText;       // Drag the LevelNameText here
    public TextMeshProUGUI difficultyText;      // Drag the DifficultyText here
    public Button selectButton;      // Drag the Button component here
}
public class LevelSelectionPanel : MonoBehaviour
{
    

    public LevelOptionUI[] levelOptions;

    private Action<LevelOption> onOptionSelected;

    public void SetupOptions(LevelOption[] options, Action<LevelOption> callback)
    {
        onOptionSelected = callback;
        for (int i = 0; i < levelOptions.Length; i++)
        {
            if (i < options.Length)
            {
                levelOptions[i].previewImage.sprite = options[i].levelImage;
                levelOptions[i].levelNameText.text = options[i].levelName;
                levelOptions[i].difficultyText.text = options[i].difficultyName;
                int index = i; 
                levelOptions[i].selectButton.onClick.RemoveAllListeners();
                levelOptions[i].selectButton.onClick.AddListener(() => OptionSelected(options[index]));
                levelOptions[i].selectButton.gameObject.SetActive(true);
            }
            else
            {
                levelOptions[i].selectButton.gameObject.SetActive(false);
            }
        }
        gameObject.SetActive(true);
    }

    private void OptionSelected(LevelOption option)
    {
        onOptionSelected?.Invoke(option);
        gameObject.SetActive(false);
    }

}
