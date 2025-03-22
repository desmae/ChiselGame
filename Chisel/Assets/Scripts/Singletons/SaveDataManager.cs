using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/*
     * SaveDataManager.cs
     * Created by: Evan Robertson
     * Date Created: 2025-03-20
     * 
     * Description: This script is used in saving and loading statistics accumulated from all 
     * game-sessions through JSON serialization.
     * 
     * Last Changed by: Evan Robertson
     * Last Date Changed: 2025-03-21
     *
     *  -> 1.0 - Created SaveDataManager.cs and created some stats to track.
     *  -> 1.1 - Added saving and loading methods.
     *   v1.1
     */
public class SaveDataManager : PersistentSingleton<SaveDataManager>
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

    string path = "";

    private void Awake()
    {
        path = Application.persistentDataPath + "/saveData.json";
        LoadGame();
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData();
        saveData.gemsBroken = gemsBroken;
        saveData.powerupsCollected = powerupsCollected;
        saveData.consumablesUsed = consumablesUsed;
        saveData.corruptedGemsCollected = corruptedGemsCollected;
        saveData.levelsCleared = levelsCleared;
        saveData.highscore = highscore;
        saveData.mostMoves = mostMoves;
        saveData.bossesDefeated = bossesDefeated;
        saveData.wins = wins;
        saveData.gamesPlayed = gamesPlayed;

        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(path, json);

        print($"Saved to {Application.persistentDataPath}");
    }



    public void LoadGame()
    {
        if (!File.Exists(path)) return;

        string json = File.ReadAllText(path);
        SaveData saveData = JsonUtility.FromJson<SaveData>(json);

        gemsBroken = saveData.gemsBroken;
        powerupsCollected = saveData.powerupsCollected;
        consumablesUsed = saveData.consumablesUsed;
        corruptedGemsCollected = saveData.corruptedGemsCollected;
        levelsCleared = saveData.levelsCleared;
        highscore = saveData.highscore;
        mostMoves = saveData.mostMoves;
        bossesDefeated = saveData.bossesDefeated;
        wins = saveData.wins;
        gamesPlayed = saveData.gamesPlayed;

        print($"Loaded from {Application.persistentDataPath}");
    }
}
