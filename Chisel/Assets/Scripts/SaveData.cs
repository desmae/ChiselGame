using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
     * SaveData.cs
     * Created by: Evan Robertson
     * Date Created: 2025-03-21
     * 
     * Description: A serializable data container for storing all important game statistics
     * 
     * Last Changed by: Evan Robertson
     * Last Date Changed: 2025-03-25
     *
     *  -> 1.0 - Created SaveData.cs and created some stats to track.
     *  -> 1.1 - Added tracking settings like custom colors and audio volumes.
     *  -> 1.2 - Added achievements
     *   v1.2
     */
[System.Serializable]
public class SaveData
{
    public int gemsBroken;
    public int powerupsCollected;
    public int consumablesUsed;
    public int corruptedGemsCollected;
    public int levelsCleared;
    public int highscore;
    public int mostMoves;
    public int bossesDefeated;
    public int wins;
    public int gamesPlayed;
    public List<SerializableColor> colors;
    public float sfxVol;
    public float musicVol;
    public List<Achievement> achievements;
}


/**
 * A serializable color class, allowing custom 
 * colors to be stored in the save file
 */
[System.Serializable]
public struct SerializableColor
{
    public float r, g, b, a;

    public SerializableColor(Color color)
    {
        r = color.r;
        g = color.g;
        b = color.b;
        a = color.a;
    }

    public Color ToColor()
    {
        return new Color(r, g, b, a);
    }
}
