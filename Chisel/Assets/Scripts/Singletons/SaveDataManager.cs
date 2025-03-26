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
     * Last Date Changed: 2025-03-26
     *
     *  -> 1.0 - Created SaveDataManager.cs and created some stats to track.
     *  -> 1.1 - Added saving and loading methods.
     *  -> 1.2 - Added saving and loading settings.
     *  -> 1.3 - Added default values for audio settings to prevent no audio when no save file is detected
     *  -> 1.4 - Added saving and loading achievements and achievement progress
     *   v1.4
     */
public class SaveDataManager : PersistentSingleton<SaveDataManager>
{
    // Game Stats
    public int gemsBroken;
    public int powerupsCollected;
    public int consumablesUsed; // not impl
    public int corruptedGemsCollected;
    public int levelsCleared;
    public int highscore;
    public int mostMoves;
    public int bossesDefeated; //not impl
    public int wins; //not impl
    public int gamesPlayed;

    // Settings
    public List<Color> colors = new();
    public float sfxVol = 1;
    public float musicVol = 1;

    // Achievements
    public List<Achievement> achievements = new();

    // Stat Dictionary for UI
    public Dictionary<string, int> playerStats = new Dictionary<string, int>();

    string path = "";

    private void Awake()
    {
        path = Application.persistentDataPath + "/saveData.json";

        LoadGame();

        // Populate Dictionary
        playerStats["Wins"] = wins;
        playerStats["Highscore"] = highscore;
        playerStats["Games Played"] = gamesPlayed;
        playerStats["Levels Cleared"] = levelsCleared;
        playerStats["Gems Broken"] = gemsBroken;
        playerStats["Powerups Collected"] = powerupsCollected;
        playerStats["Consumables Used"] = consumablesUsed;
        playerStats["Corrupted Gems Collected"] = corruptedGemsCollected;
        playerStats["Most Moves Held"] = mostMoves;
        playerStats["Bosses Defeated"] = bossesDefeated;
    }

    public void SaveGame()
    {
        SaveData saveData = new SaveData();

        // Save new stats
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

        // Save color customization
        List<SerializableColor> tempColors = new();
        foreach (Color color in colors)
        {
            tempColors.Add(new SerializableColor(color));
        }
        saveData.colors = tempColors;

        // Save audio settings
        saveData.sfxVol = sfxVol;
        saveData.musicVol = musicVol;

        // Save achievements
        saveData.achievements = achievements;

        string json = JsonUtility.ToJson(saveData);
        File.WriteAllText(path, json);

        print($"Saved to {Application.persistentDataPath}");
    }



    public void LoadGame()
    {
        if (!File.Exists(path)) return;

        string json = File.ReadAllText(path);
        SaveData saveData = JsonUtility.FromJson<SaveData>(json);

        // Load game stats
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

        // Load custom colors
        List<Color> colors = new();
        foreach (SerializableColor sColor in saveData.colors)
        {
            colors.Add(sColor.ToColor());
        }
        this.colors = colors;
        SettingsManager.Instance.colors = colors;

        // Load audio settings
        sfxVol = saveData.sfxVol;
        musicVol = saveData.musicVol;

        // Load achievements
        achievements = saveData.achievements;

        print($"Loaded from {Application.persistentDataPath}");
    }
}
