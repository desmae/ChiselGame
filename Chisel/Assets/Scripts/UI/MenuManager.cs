using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
/*
* MenuManager.cs
* Created by: Evan Robertson
* Date Created: 2024-10-10
* 
* Description: Manager class for managing all button actions in the main menu
* 
* Last Changed by: Evan Robertson
* Last Date Changed: 2025-03-24
* 
* 
*   -> 1.0 - Created MenuManager.cs
*   -> 1.1 - Changed SceneManager.LoadScene from 0 to 1 since the build settings were changed.
*   -> 1.2 - Updated for more menuscreens
*   -> 1.3 - Added functionality for populating the stats screen
*   
*   v1.3
*/
public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject statUIPrefab;
    [SerializeField] private GameObject statContent;

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
        //Populate stat page with all stats
        foreach (var stat in SaveDataManager.Instance.playerStats)
        {
            GameObject statObj = Instantiate(statUIPrefab, statContent.transform);
            statObj.GetComponent<StatUI>().SetText(stat.Key, stat.Value.ToString());
        }
    }

    private void Start()
    {
        AudioController.Instance.PlayMusic("MenuMusic");
    }
}
