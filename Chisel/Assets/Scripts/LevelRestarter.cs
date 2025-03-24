using UnityEngine;
using UnityEngine.SceneManagement;

/*
     * LevelRestarter.cs
     * Created by: Sylvia Yi
     * Date Created: 2025-03-24
     * 
     * Description: This script is in charge of restart when user is at game scene.
     * 
     * Last Changed by: Sylvia Yi
     * Last Date Changed: 2025-03-24
     * 
     *   -> 1.0 - Created LevelRestarter.cs and a button on the gameplay screen to let player restart from game scene.
     *
     */

public class LevelRestarter : MonoBehaviour
{
    public void RestartLevel()
    {
       
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
