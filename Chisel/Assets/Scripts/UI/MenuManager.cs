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
* Last Changed by: Nicolas Kaplan
* Last Date Changed: 2024-10-11
* 
* 
*   -> 1.0 - Created MenuManager.cs
*   -> 1.1 - Changed SceneManager.LoadScene from 0 to 1 since the build settings were changed.
*   
*   v1.1
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
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void Start()
    {
        
    }
}
