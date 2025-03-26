using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
/*
* MenuManager.cs
* Created by: Evan Robertson
* Date Created: 2024-10-10
* 
* Description: Manager class for managing all button actions in the main menu
* 
* Last Changed by: Evan Robertson
* Last Date Changed: 2025-03-26
* 
* 
*   -> 1.0 - Created MenuManager.cs
*   -> 1.1 - Changed SceneManager.LoadScene from 0 to 1 since the build settings were changed.
*   -> 1.2 - Updated for more menuscreens
*   -> 1.3 - Added functionality for populating the stats screen
*   -> 1.4 - Added fucntionality for populating the achievements screen
*   v1.4
*/
public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject statUIPrefab;
    [SerializeField] private GameObject statContent;

    [SerializeField] private GameObject achieveUIPrefab;
    [SerializeField] private GameObject achieveContent;

    public void ToggleMenu(GameObject menu)
    {
        menu.SetActive(!menu.activeSelf);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void PopulateStats()
    {
        DeletePrefabs(statContent.transform);

        //Populate stat page with all stats
        foreach (var stat in SaveDataManager.Instance.playerStats)
        {
            GameObject statObj = Instantiate(statUIPrefab, statContent.transform);
            statObj.GetComponent<StatUI>().SetText(stat.Key, stat.Value.ToString());
        }
    }

    public void PopulateAchievements()
    {
        DeletePrefabs(achieveContent.transform);

        //Populate achievement page with all achievements
        foreach (var achievement in AchievementManager.Instance.achievements)
        {
            GameObject achieveObj = Instantiate(achieveUIPrefab, achieveContent.transform);
            if (achievement.unlocked) achieveObj.GetComponent<Image>().color = new Color(0.4097098f, 0.9339623f, 0.4229233f, 0.39f);

            achieveObj.GetComponent<AchievementUI>().SetValues(achievement.title, achievement.description, achievement.currentValue, achievement.requiredValue);
        }
    }

    void DeletePrefabs(Transform transform)
    {
        int children = transform.childCount;

        for (int i = 0; i < children; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    private void Start()
    {
        AudioController.Instance.PlayMusic("MenuMusic");
    }
}
