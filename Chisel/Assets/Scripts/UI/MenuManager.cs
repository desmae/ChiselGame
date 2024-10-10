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
* Last Date Changed: 2024-10-10
* 
* 
*   -> 1.0 - Created MenuManager.cs
*   
*   v1.0
*/
public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject titleMenu;
    [SerializeField] GameObject settingsMenu;
    public void ToggleMenu(GameObject menu)
    {
        titleMenu.SetActive(!titleMenu.activeSelf);
        menu.SetActive(!menu.activeSelf);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void Start()
    {
        
    }
}
