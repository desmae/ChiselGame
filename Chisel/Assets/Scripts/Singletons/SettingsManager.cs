using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
* SettingsManager.cs
* Created by: Evan Robertson
* Date Created: 2024-10-10
* 
* Description: A persistent singleton used to carry player settings across scenes
* 
* Last Changed by: Evan Robertson
* Last Date Changed: 2024-11-11
* 
* 
*   -> 1.0 - Created SettingsManager.cs
*   -> 1.1 - Moved assignment of colorVals to search everytime the scene changes, added getter for empty color, added audio settings
*   
*   v1.1
*/
public class SettingsManager : PersistentSingleton<SettingsManager>
{
    [SerializeField] Color emptyColor;
    [SerializeField] List<Color> defaultColors = new List<Color>();

    public ColorPickers colorVals;

    public List<Color> colors = new List<Color>();

    private void Start()
    {
        GetColorVals();
    }

    void OnLevelWasLoaded(int level)
    {
        GetColorVals();
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
            colorVals.SetDefaultColors();
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
    }

    public void UpdateMusic(System.Single val)
    {
        AudioController.Instance.SetMusicVolume(val);
    }
    #endregion
}
