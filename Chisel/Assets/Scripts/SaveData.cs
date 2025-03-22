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
     * Last Date Changed: 2025-03-21
     *
     *  -> 1.0 - Created SaveData.cs and created some stats to track.
     *   v1.0
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
}
