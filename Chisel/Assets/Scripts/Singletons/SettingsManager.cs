using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/*
* SettingsManager.cs
* Created by: Evan Robertson
* Date Created: 2024-10-10
* 
* Description: A persistent singleton used to carry player settings across scenes
* 
* Last Changed by: Evan Robertson
* Last Date Changed: 2025-03-24
* 
* 
*   -> 1.0 - Created SettingsManager.cs
*   -> 1.1 - Moved assignment of colorVals to search everytime the scene changes, added getter for empty color, added audio settings
*   -> 1.2 - Updated to save and load settings from save file
*   v1.2
*/
public class SettingsManager : PersistentSingleton<SettingsManager>
{
    [SerializeField] Color emptyColor;
    [SerializeField] List<Color> defaultColors = new List<Color>();

    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider musicSlider;


    public ColorPickers colorVals;

    public List<Color> colors = new List<Color>();

    private void Start()
    {
        GetColorVals();

        // Update settings from save
        SaveDataManager.Instance.colors = colors;
        if (sfxSlider != null && musicSlider != null)
        {
            UpdateSFX(SaveDataManager.Instance.sfxVol);
            UpdateMusic(SaveDataManager.Instance.musicVol);
        }
    }

    void OnLevelWasLoaded(int level)
    {
        GetColorVals();


        if (sfxSlider != null && musicSlider != null)
        {
            UpdateSFX(SaveDataManager.Instance.sfxVol);
            UpdateMusic(SaveDataManager.Instance.musicVol);
        }
    }


    #region Colors
    public Color GetEmptyColor()
    {
        return emptyColor;
    } 

    void GetColorVals()
    {
        colorVals = FindObjectOfType<ColorPickers>(true);

        if (colorVals != null)
        {
            colorVals.SetDefaultColors(colors);
        }
    }

    public void SetColors()
    {
        if (colorVals != null)
        {
            colors = colorVals.GetColors();
        }
    }

    public List<Color> GetDefaultColors()
    {
        return defaultColors;
    }

    public List<Color> GetColors()
    {
        return colors;
    }
    #endregion

    #region Audio
    public void UpdateSFX(System.Single val)
    {
        AudioController.Instance.SetSFXVolume(val);
        SaveDataManager.Instance.sfxVol = val;
        sfxSlider.value = val;
    }

    public void UpdateMusic(System.Single val)
    {
        AudioController.Instance.SetMusicVolume(val);
        SaveDataManager.Instance.musicVol = val;
        musicSlider.value = val;
    }
    #endregion
}
