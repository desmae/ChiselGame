using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/*
     * AchievementManager.cs
     * Created by: Evan Robertson
     * Date Created: 2025-03-25
     * 
     * Description: This script is used in managing the unlocking of achievements. 
     * The script can be used in-editor to create new achievements by adding it to the list and
     * clicking the Generate Achievements Enum button. After which, the achievement is added to the AchievementID enum
     * to use in calling the UnlockAchievement method
     * 
     * Last Changed by: Evan Robertson
     * Last Date Changed: 2025-03-26
     *
     *  -> 1.0 - Created AchievementManager.cs and added the UnlockAchievement method.
     *  -> 1.1 - Fixed various issues with saving and loading achievements when a new achievement is added.
     *   v1.1
     */
public class AchievementManager : PersistentSingleton<AchievementManager>
{
    public List<Achievement> achievements = new();

    public GameObject achievementUIPrefab;

    private void Start()
    {
        if (AreListsEqual()) 
        {
            achievements = SaveDataManager.Instance.achievements;
        }
        else
        {
            SaveDataManager.Instance.achievements = achievements;
            SaveDataManager.Instance.SaveGame();
        }
    }

    void OnLevelWasLoaded(int level)
    {
        if (AreListsEqual())
        {
            achievements = SaveDataManager.Instance.achievements;
        }
        else
        {
            SaveDataManager.Instance.achievements = achievements;
        }
    }

    /**
     * Unlocks achievement if the provided value exeeds the value required
     * If additive is true, the provided value is added to the achievement's current progress
     * If additive is false, the provided value is set to the achievement's current progress
     * Additive is true by default
     */
    public void UnlockAchievement(AchievementID id, int value, bool additive = true)
    {
        if (achievements.Count == 0) return;

        var achievement = achievements[(int)id];

        if (achievement.unlocked) return;

        if (additive) achievement.currentValue += value;
        else achievement.currentValue = value;

        if (achievement.currentValue >= achievement.requiredValue)
        {
            achievement.unlocked = true;
            Debug.Log($"Achievement Unlocked: {achievement.title}!");
            ShowAchievement(achievement);

            SaveDataManager.Instance.SaveGame();
        }
    }

    void ShowAchievement(Achievement achievement)
    {
        // todo - have achievement notification popup
    }

    public bool AreListsEqual()
    {
        var managerList = AchievementManager.Instance.achievements.Select(a => a.title).OrderBy(title => title);
        var savedList = SaveDataManager.Instance.achievements.Select(a => a.title).OrderBy(title => title);

        return managerList.SequenceEqual(savedList);
    }
}
