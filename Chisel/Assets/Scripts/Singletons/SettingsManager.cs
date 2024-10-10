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
* Last Date Changed: 2024-10-10
* 
* 
*   -> 1.0 - Created SettingsManager.cs
*   
*   v1.0
*/
public class SettingsManager : PersistentSingleton<SettingsManager>
{
    [SerializeField] Color emptyColor;
    [SerializeField] List<Color> defaultColors = new List<Color>();

    public ColorPickers colorVals;

    public List<Color> colors = new List<Color>();

    private void Start()
    {
        colorVals = FindObjectOfType<ColorPickers>(true);

        if (colorVals != null)
        {
            colorVals.SetDefaultColors(defaultColors);
        }
    }

    public void SetColors()
    {
        if (colorVals != null)
        {
            colors = colorVals.GetColors();
            colors.Insert(0, emptyColor);
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
}
